using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OcelotGatewayApi.Data.Seeders;

/// <summary>
/// Classe de peuplement des données d'identité dans la base de données.
/// </summary>
public static class IdentityDataSeeder
{
    public static async Task EnsurePopulatedAsync(IApplicationBuilder app, IConfiguration config)
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await dbContext.Database.MigrateAsync();
        
        var adminUserName = config.GetSection("UserIds:AdminUser").Value;
        var adminPassword = config.GetSection("UserIds:AdminPassword").Value;
        
        if (string.IsNullOrEmpty(adminUserName) || string.IsNullOrWhiteSpace(adminUserName) ||
            string.IsNullOrEmpty(adminPassword) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }
        
        var userManager = (UserManager<IdentityUser>)scope.ServiceProvider.GetRequiredService(typeof(UserManager<IdentityUser>));
       
        var adminUser = await userManager.FindByIdAsync(adminUserName);
        if (adminUser == null)
        {
            adminUser = new IdentityUser(adminUserName)
            {
                Email = adminUserName,
                UserName = adminUserName,
                EmailConfirmed = true
            };
            
            await userManager.CreateAsync(adminUser, adminPassword);
        }
    }
}