﻿using EstoqueNutris.DTOs;
using EstoqueNutris.Models;
using EstoqueNutris.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueNutris.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleService _roleService;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(
            UserManager<ApplicationUser> userManager,
            RoleService roleService,
            ILogger<RegisterController> logger)
        {
            _userManager = userManager;
            _roleService = roleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                    return BadRequest(new { message = "Este e-mail já está registrado." });

                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    Nome = dto.Nome,
                    IsAdmin = dto.IsAdmin
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { errors });
                }

                await _roleService.AssignRoleToUser(user, dto.IsAdmin);

                _logger.LogInformation($"Novo usuário registrado: {user.Email}");

                return CreatedAtAction(nameof(Register), new { id = user.Id }, new
                {
                    message = "Usuário registrado com sucesso",
                    user = new
                    {
                        user.Id,
                        user.Email,
                        user.Nome,
                        user.IsAdmin
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao registrar usuário: {dto.Email}");
                return StatusCode(500, new { message = "Erro ao registrar usuário" });
            }
        }
    }
}
