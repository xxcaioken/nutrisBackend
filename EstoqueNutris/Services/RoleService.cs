using EstoqueNutris.Models;
using Microsoft.AspNetCore.Identity;

namespace EstoqueNutris.Services
{
    public class RoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleService> _logger;

        public const string ADMIN_ROLE = "Admin";
        public const string USER_ROLE = "User";

        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task InitializeRoles()
        {
            try
            {
                if (!await _roleManager.RoleExistsAsync(ADMIN_ROLE))
                {
                    await _roleManager.CreateAsync(new ApplicationRole(ADMIN_ROLE));
                    _logger.LogInformation("Role Admin criada com sucesso");
                }

                if (!await _roleManager.RoleExistsAsync(USER_ROLE))
                {
                    await _roleManager.CreateAsync(new ApplicationRole(USER_ROLE));
                    _logger.LogInformation("Role User criada com sucesso");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inicializar roles");
                throw;
            }
        }

        public async Task AssignRoleToUser(ApplicationUser user, bool isAdmin = false)
        {
            try
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                var role = isAdmin ? ADMIN_ROLE : USER_ROLE;
                await _userManager.AddToRoleAsync(user, role);
                user.IsAdmin = isAdmin;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation($"Role {role} atribuída ao usuário {user.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atribuir role ao usuário {user.Email}");
                throw;
            }
        }

        public async Task<bool> IsUserAdmin(ApplicationUser user)
        {
            return await _userManager.IsInRoleAsync(user, ADMIN_ROLE);
        }
    }
} 