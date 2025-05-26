using EstoqueNutris.DTOs;
using EstoqueNutris.Models;
using EstoqueNutris.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueNutris.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly GoogleAuthService _googleAuthService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService,
            GoogleAuthService googleAuthService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _googleAuthService = googleAuthService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (!dto.IsValid())
                {
                    var errors = dto.GetValidationErrors();
                    return BadRequest(new { message = "Dados de login inválidos", errors });
                }

                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Tentativa de login com email não encontrado: {dto.Email}");
                    return Unauthorized(new { message = "Email ou senha inválidos" });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Tentativa de login com senha inválida para o usuário: {dto.Email}");
                    return Unauthorized(new { message = "Email ou senha inválidos" });
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarning($"Usuário bloqueado tentando fazer login: {dto.Email}");
                    return Unauthorized(new { message = "Conta temporariamente bloqueada. Tente novamente mais tarde." });
                }

                var token = _tokenService.GenerateToken(user);

                return Ok(new
                {
                    token,
                    user = new
                    {
                        user.Id,
                        user.Email,
                        user.Nome,
                        user.EscolaId
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar login para o email: {dto.Email}");
                return StatusCode(500, new { message = "Ocorreu um erro ao processar o login" });
            }
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDto dto)
        {
            try
            {
                if (!dto.IsValid())
                {
                    var errors = dto.GetValidationErrors();
                    return BadRequest(new { message = "Token do Google inválido", errors });
                }

                var user = await _googleAuthService.ValidateGoogleToken(dto.GoogleToken);
                if (user == null)
                {
                    return Unauthorized(new { message = "Token do Google inválido ou expirado" });
                }

                var token = _tokenService.GenerateToken(user);

                return Ok(new
                {
                    token,
                    user = new
                    {
                        user.Id,
                        user.Email,
                        user.Nome,
                        user.EscolaId
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar login com Google");
                return StatusCode(500, new { message = "Ocorreu um erro ao processar o login com Google" });
            }
        }
    }
}
