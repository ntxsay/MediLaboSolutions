using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Utilities;

namespace FrontPatient.AspNetCore.Services;

public interface IGenreServices
{
    public Task<GenreViewModel[]> GetAllAsync();
    public Task<GenreViewModel?> GetAsync(int id);
    public Task<GenreViewModel?> CreateAsync(GenreViewModel value);
    public Task<GenreViewModel?> UpdateAsync(int id, GenreViewModel value);
    public Task<bool> DeleteAsync(int id);
}

public class GenreServices(ILogger<GenreServices> logger, IHttpClientFactory clientFactory) : IGenreServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient("GatewayClient");
    
    public async Task<GenreViewModel[]> GetAllAsync()
    {
        try
        {
            using var response = await _client.GetAsync("genre/All");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération des genres : {response}", response.ReasonPhrase);
                return [];
            }
            
            var datas = await response.Content.ReadFromJsonAsync<GenreDto[]>();
            if (datas == null)
            {
                logger.LogWarning("Les genres n'ont pas été trouvés.");
                return [];  
            }
            
            return datas.Select(s => s.ToViewModel()).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération des genres");
            return [];
        }
    }
    
    public async Task<GenreViewModel?> GetAsync(int id)
    {
        try
        {
            using var response = await _client.GetAsync($"genre/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du genre : {response}", response.ReasonPhrase);
                return null;       
            }
            
            var data = await response.Content.ReadFromJsonAsync<GenreDto>();
            if (data == null)
            {
                logger.LogWarning("Le genre n'a pas été trouvé.");
                return null;       
            }
            
            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du genre");
            return null;
        }
    }
    
    public async Task<GenreViewModel?> CreateAsync(GenreViewModel value)
    {
        try
        {
            using var response = await _client.PostAsJsonAsync("genre/Create", value.ToDto());
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la création du genre : {response}", response.ReasonPhrase);
                return null;              
            }
            
            var data = await response.Content.ReadFromJsonAsync<GenreDto>();
            if (data == null)
            {
                logger.LogWarning("Une erreur est survenue lors de la création du genre");
                return null; 
            }
            
            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création du genre");
            return null;
        }
    }
    
    public async Task<GenreViewModel?> UpdateAsync(int id, GenreViewModel value)
    {
        try
        {
            using var response = await _client.PutAsJsonAsync($"genre/Update/{id}", value.ToDto());
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la mise à jour du genre : {response}", response.ReasonPhrase);
                return null;              
            }
            
            var data = await response.Content.ReadFromJsonAsync<GenreDto>();
            if (data == null)
            {
                logger.LogWarning("Une erreur est survenue lors de la mise à jour du genre");
                return null;
            }
            
            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la mise à jour du genre");
            return null;
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var response = await _client.DeleteAsync($"genre/Delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la suppression du genre : {response}", response.ReasonPhrase);
                return false;              
            }
            
            var data = await response.Content.ReadFromJsonAsync<bool>();
            if (!data)
            {
                logger.LogWarning("Une erreur est survenue lors de la suppression du genre");
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la suppression du genre.");
            return false;
        }
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}