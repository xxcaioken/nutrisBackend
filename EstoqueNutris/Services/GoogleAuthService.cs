using System.Net.Http.Headers;
using System.Text.Json;
using EstoqueNutris.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace EstoqueNutris.Services
{
    public class GoogleAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(
            HttpClient httpClient,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<GoogleAuthService> logger)
        {
            _httpClient = httpClient;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ApplicationUser?> ValidateGoogleToken(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Falha ao validar token do Google. Status: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Resposta do Google: {content}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var googleUser = JsonSerializer.Deserialize<GoogleUserInfo>(content, options);
                _logger.LogInformation($"Usuário deserializado: {JsonSerializer.Serialize(googleUser)}");

                if (googleUser == null)
                {
                    _logger.LogWarning("Falha ao deserializar resposta do Google");
                    return null;
                }

                if (string.IsNullOrEmpty(googleUser.Email))
                {
                    _logger.LogWarning("Email do usuário Google está vazio");
                    return null;
                }

                var user = await _userManager.FindByEmailAsync(googleUser.Email);
                if (user == null)
                {
                    _logger.LogInformation($"Criando novo usuário para email: {googleUser.Email}");
                    user = new ApplicationUser
                    {
                        UserName = googleUser.Email,
                        Email = googleUser.Email,
                        Nome = googleUser.Name,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        _logger.LogError($"Erro ao criar usuário do Google: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        return null;
                    }
                    _logger.LogInformation($"Usuário criado com sucesso: {user.Email}");
                }
                else
                {
                    _logger.LogInformation($"Usuário encontrado: {user.Email}");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar token do Google");
                return null;
            }
        }
    }

    public class GoogleUserInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("picture")]
        public string Picture { get; set; } = string.Empty;

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; } = string.Empty;

        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; } = string.Empty;
    }
} 