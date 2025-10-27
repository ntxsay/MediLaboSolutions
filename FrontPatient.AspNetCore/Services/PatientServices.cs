using Microsoft.AspNetCore.Mvc.Rendering;
using PatientShared.Models.Dtos;
using PatientDto = FrontPatient.AspNetCore.Models.Dtos.PatientDto;

namespace FrontPatient.AspNetCore.Services;

public interface IPatientServices
{
    public Task<PatientDto[]> GetAllAsync();
    public Task<PatientDto?> DetailAsync(int id);
    public Task<bool> CreateAsync(PatientDto value);
    public Task<PatientDto?> UpdateAsync(int id);
    public Task<bool> UpdateAsync(int id, PatientDto value);
    public Task<bool> DeleteAsync(int id);
}

public class PatientServices(ILogger<PatientServices> logger, IHttpClientFactory clientFactory) : IPatientServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient("GatewayClient");
    
    public async Task<PatientDto[]> GetAllAsync()
    {
        try
        {
            var response = await _client.GetAsync("patient/All");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la récupération des patients");
            }
            
            var datas = await response.Content.ReadFromJsonAsync<PatientDto[]>();
            if (datas == null)
            {
                throw new Exception("Une erreur est survenue lors de la récupération des patients");
            }
            
            return datas;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération des patients: {ex.Message}");
            return [];
        }
    }
    
    public async Task<PatientDto?> DetailAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"patient/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la récupération du patient");
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientDto>();
            if (data == null)
            {
                throw new Exception("Une erreur est survenue lors de la récupération du patient");
            }
            
            return data;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération du patient: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> CreateAsync(PatientDto value)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("patient/Create", value);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la création du patient");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la création du patient: {ex.Message}");
            return false;
        }
    }
    
    public async Task<PatientDto?> UpdateAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"patient/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la récupération du patient");
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientDto>();
            if (data == null)
            {
                throw new Exception("Une erreur est survenue lors de la récupération du patient");
            }

            data.GenresSelectList = new SelectList(data.Genres, nameof(GenreDto.Id), nameof(GenreDto.Name));
            
            return data;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération du patient: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> UpdateAsync(int id, PatientDto value)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"patient/Update/{id}", value);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la mise à jour du patient");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la mise à jour du patient: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _client.DeleteAsync($"patient/Delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Une erreur est survenue lors de la suppression du patient");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la suppression du patient: {ex.Message}");
            return false;
        }
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}