using PatientShared.Models.Dtos;

namespace FrontPatient.AspNetCore.Services;

public interface IGenreServices
{
    public Task<GenreDto[]> GetAllAsync();
    public Task<GenreDto?> GetAsync(int id);
    public Task<bool> CreateAsync(GenreDto value);
    public Task<bool> UpdateAsync(int id, GenreDto value);
    public Task<bool> DeleteAsync(int id);
}

public class GenreServices(ILogger<GenreServices> logger, IHttpClientFactory clientFactory) : IGenreServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient("GatewayClient");
    
    public async Task<GenreDto[]> GetAllAsync()
    {
        try
        {
            var response = await _client.GetAsync("genre/All");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la récupération des genres");
            }
            
            var datas = await response.Content.ReadFromJsonAsync<GenreDto[]>();
            if (datas == null)
            {
                throw new Exception("Une erreur est survenue lors de la récupération des genres");
            }
            
            return datas;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération des genres: {ex.Message}");
            return [];
        }
    }
    
    public async Task<GenreDto?> GetAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"genre/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la récupération du genre");
            }
            
            var data = await response.Content.ReadFromJsonAsync<GenreDto>();
            if (data == null)
            {
                throw new Exception("Une erreur est survenue lors de la récupération du genre");
            }
            
            return data;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération du genre: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> CreateAsync(GenreDto value)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("genre/Create", value);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la création du genre");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la création du genre: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> UpdateAsync(int id, GenreDto value)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"genre/Update/{id}", value);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la mise à jour du genre");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la mise à jour du genre: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"genre/Delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la suppression du genre");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la suppression du genre: {ex.Message}");
            return false;
        }
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}