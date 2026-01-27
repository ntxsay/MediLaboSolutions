using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Services.Interfaces;

namespace FrontPatient.AspNetCore.Services.Implementations;

public sealed class LoginServices(ILogger<LoginServices> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : ILoginServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient(configuration["MyHttpClients:AuthorizedClientName"]!);
    
    public async Task<TokenDto?> LoginAsync(LoginViewModel value)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("Login", value);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Une erreur est survenue lors de la connexion");
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<TokenDto>();
            if (data == null)
            {
                logger.LogError("Une erreur est survenue lors de la récupération du token");
                return null;
            }

            return data;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la connexion");
            return null;
        }
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}