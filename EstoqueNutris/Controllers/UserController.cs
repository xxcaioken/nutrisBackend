using EstoqueNutris.DTOs;
using EstoqueNutris.Models;
using EstoqueNutris.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueNutris.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleService _roleService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleService roleService,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = _userManager.Users.ToList();
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Nome = user.Nome,
                        EscolaId = user.EscolaId,
                        IsAdmin = user.IsAdmin,
                        Roles = roles.ToList()
                    });
                }

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários");
                return StatusCode(500, new { message = "Erro ao buscar usuários" });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado" });

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nome = user.Nome,
                    EscolaId = user.EscolaId,
                    IsAdmin = user.IsAdmin,
                    Roles = roles.ToList()
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário {id}");
                return StatusCode(500, new { message = "Erro ao buscar usuário" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado" });

                user.Nome = dto.Nome;
                user.Email = dto.Email;
                user.EscolaId = dto.EscolaId;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { errors });
                }

                if (dto.IsAdmin != user.IsAdmin)
                {
                    await _roleService.AssignRoleToUser(user, dto.IsAdmin);
                }

                return Ok(new { message = "Usuário atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar usuário {id}");
                return StatusCode(500, new { message = "Erro ao atualizar usuário" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado" });

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { errors });
                }

                return Ok(new { message = "Usuário excluído com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir usuário {id}");
                return StatusCode(500, new { message = "Erro ao excluir usuário" });
            }
        }

        [HttpPut("{id}/change-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado" });

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { errors });
                }

                return Ok(new { message = "Senha alterada com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao alterar senha do usuário {id}");
                return StatusCode(500, new { message = "Erro ao alterar senha" });
            }
        }
    }
} 