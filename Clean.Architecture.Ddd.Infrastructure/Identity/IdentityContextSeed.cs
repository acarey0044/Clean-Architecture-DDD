using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Clean.Architecture.Ddd.Infrastructure.Identity
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // create admin role
            var adminRoleTask = roleManager.CreateAsync(new IdentityRole("Administrator"));
            var employeeRoleTask = roleManager.CreateAsync(new IdentityRole("Employee"));
            await Task.WhenAll(adminRoleTask, employeeRoleTask);

            var employee = new ApplicationUser { UserName = "buffalobill@gmail.com", Email = "buffalobill@gmail.com" };
            var admin = new ApplicationUser { UserName = "bigbossman@gmail.com", Email = "bigbossman@gmail.com" };
            var employeeTask = userManager.CreateAsync(employee, "Password123!");
            var adminTask = userManager.CreateAsync(admin, "SecurePassword123!");
            await Task.WhenAll(employeeTask, adminTask);

            // add admin role to admin user
            var adminUser = await userManager.FindByNameAsync(admin.UserName);
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}
