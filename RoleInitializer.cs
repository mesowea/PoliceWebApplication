using PoliceWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;using Microsoft.Extensions.Configuration;

namespace PoliceWebApplication
{
    public class RoleInitializer
    {
        private static IConfiguration _config;

        public RoleInitializer(IConfiguration config)
        {
            _config = config;
        }
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = _config.GetValue<string>("Admin:AdminEmail");
            string password = _config.GetValue<string>("Admin:AdminPassword");
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, Code = "premined"};
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
