using ComplainTracking.Data;
using ComplainTracking.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComplainTracking.Data.Seeders
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Ensure database is created
                await context.Database.MigrateAsync();

                // Check if data already exists
                if (context.Users.Any())
                {
                    return;
                }

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Create Roles
                var roles = new[] { "Admin", "Agent", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create Default Admin User
                var adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin@complain.com",
                    Email = "admin@complain.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                if (await userManager.FindByEmailAsync(adminUser.Email) == null)
                {
                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                // Create Default Support Agents
                var agents = new[]
                {
                    new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "agent1@complain.com",
                        Email = "agent1@complain.com",
                        EmailConfirmed = true,
                        FirstName = "John",
                        LastName = "Agent"
                    },
                    new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "agent2@complain.com",
                        Email = "agent2@complain.com",
                        EmailConfirmed = true,
                        FirstName = "Jane",
                        LastName = "Support"
                    }
                };

                foreach (var agent in agents)
                {
                    if (await userManager.FindByEmailAsync(agent.Email ?? string.Empty) == null)
                    {
                        var result = await userManager.CreateAsync(agent, "Agent@123");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(agent, "Agent");
                        }
                    }
                }

                // Create Default Regular User
                var regularUser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "user@complain.com",
                    Email = "user@complain.com",
                    EmailConfirmed = true,
                    FirstName = "Regular",
                    LastName = "User"
                };

                if (await userManager.FindByEmailAsync(regularUser.Email) == null)
                {
                    var result = await userManager.CreateAsync(regularUser, "User@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(regularUser, "User");
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
