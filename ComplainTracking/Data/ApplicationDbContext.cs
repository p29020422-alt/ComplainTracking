using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ComplainTracking.Models.Entities;

namespace ComplainTracking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Ticket relationships
            builder.Entity<Ticket>()
                .HasOne(t => t.Submitter)
                .WithMany(u => u.SubmittedTickets)
                .HasForeignKey(t => t.SubmitterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.AssignedAgent)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedAgentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Ticket>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Ticket)
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TicketComment relationships
            builder.Entity<TicketComment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for performance
            builder.Entity<Ticket>()
                .HasIndex(t => t.Status);

            builder.Entity<Ticket>()
                .HasIndex(t => t.CreatedAt);

            builder.Entity<Ticket>()
                .HasIndex(t => t.SubmitterId);

            builder.Entity<Ticket>()
                .HasIndex(t => t.AssignedAgentId);
        }
    }
}
