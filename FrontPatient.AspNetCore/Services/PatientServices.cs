using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Utilities;
namespace FrontPatient.AspNetCore.Services;

public interface IPatientServices
{
    public Task<PatientViewModel[]> GetAllAsync();
    public Task<PatientViewModel?> DetailAsync(int id);
    public Task<PatientViewModel?> CreateEmptyAsync();
    public Task<bool> CreateAsync(PatientViewModel value);
    public Task<PatientViewModel?> UpdateAsync(int id);
    public Task<bool> UpdateAsync(int id, PatientViewModel value);
    public Task<bool> DeleteAsync(int id);
}

public class PatientServices(ILogger<PatientServices> logger, IHttpClientFactory clientFactory) : IPatientServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient("GatewayClient");
    
    public async Task<PatientViewModel[]> GetAllAsync()
    {
        try
        {
            var response = await _client.GetAsync("patient/All");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération des patients : {0}", response.ReasonPhrase);
                return [];
            }
            
            var datas = await response.Content.ReadFromJsonAsync<PatientDto[]>();
            if (datas == null)
            {
                logger.LogWarning("Les patients n'ont pas été trouvés.");
                return [];  
            }

            return datas.Select(s => s.ToViewModel()).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération des patients");
            return [];
        }
    }
    
    public async Task<PatientViewModel?> DetailAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"patient/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du patient : {0}", response.ReasonPhrase);
                return null;      
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientDto>();
            if (data == null)
            {
                logger.LogWarning("Le patient n'a pas été trouvé.");
                return null;        
            }

            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du patient");
            return null;
        }
    }

    public async Task<PatientViewModel?> CreateEmptyAsync()
    {
        try
        {
            var response = await _client.GetAsync("patient/CreateEmpty");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du patient : {0}", response.ReasonPhrase);
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientDto>();
            if (data == null)
            {
                logger.LogWarning("Le patient n'a pas été trouvé.");
                return null;        
            }

            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du patient");
            return null;
        }
    }
    
    public async Task<bool> CreateAsync(PatientViewModel value)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("patient/Create", value);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la création du patient : {0}", response.ReasonPhrase);
                return false;
            }
            
            var isSuccess = await response.Content.ReadFromJsonAsync<bool>();
            if (!isSuccess)
            {
                logger.LogWarning("Le patient n'a pas été créé.");
                return false;
            }

            return isSuccess;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création du patient");
            return false;
        }
    }
    
    public async Task<PatientViewModel?> UpdateAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"patient/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du patient : {0}", response.ReasonPhrase);
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientDto>();
            if (data == null)
            {
                logger.LogWarning("La patient n'a pas été trouvé.");
                return null;
            }

            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du patient");
            return null;
        }
    }
    
    public async Task<bool> UpdateAsync(int id, PatientViewModel value)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"patient/Update/{id}", value);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la mise à jour du patient : {0}", response.ReasonPhrase);
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la mise à jour du patient");
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
                logger.LogWarning("Une erreur est survenue lors de la suppression du patient : {0}", response.ReasonPhrase);
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la suppression du patient");
            return false;
        }
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}