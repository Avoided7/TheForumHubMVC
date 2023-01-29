using Microsoft.AspNetCore.Identity;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data
{
    public static class SeedDB
    {
        public static async void SeedUsers(IApplicationBuilder builder)
        {
            using (var service = builder.ApplicationServices.CreateScope())
            {
                var role = service.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!role.Roles.Any())
                {
                    await role.CreateAsync(new IdentityRole(Roles.Admin));
                    await role.CreateAsync(new IdentityRole(Roles.User));
                }
            }
        }
    }
}
