using EstoqueNutris.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EstoqueNutris.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                // Aplica as migrações pendentes
                await context.Database.MigrateAsync();

                // Cria as roles se não existirem
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new ApplicationRole("Admin"));
                    logger.LogInformation("Role Admin criada");
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new ApplicationRole("User"));
                    logger.LogInformation("Role User criada");
                }

                // Cria o usuário admin se não existir
                var adminEmail = "admin@nutris.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        Nome = "Administrador",
                        EmailConfirmed = true,
                        IsAdmin = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        logger.LogInformation("Usuário admin criado com sucesso");
                    }
                    else
                    {
                        logger.LogError("Erro ao criar usuário admin: {errors}", 
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao inicializar o banco de dados");
                throw;
            }
        }
    }
} 