using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Utilities;

namespace FrontPatient.AspNetCore.Services.Implementations;

public class PatientServices(ILogger<PatientServices> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : IPatientServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient(configuration["MyHttpClients:GatewayClientName"]!);
    
    public async Task<PatientViewModel[]> GetAllAsync()
    {
        try
        {
            using var response = await _client.GetAsync("patient/All");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération des patients : {response}", response.ReasonPhrase);
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
            using var response = await _client.GetAsync($"patient/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du patient : {response}", response.ReasonPhrase);
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
            using var response = await _client.GetAsync("patient/CreateEmpty");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du patient : {respnse}", response.ReasonPhrase);
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
    
    public async Task<PatientViewModel?> CreateAsync(PatientViewModel value)
    {
        try
        {
            using var response = await _client.PostAsJsonAsync("patient/Create", value.ToDto());
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la création du patient : {response}", response.ReasonPhrase);
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientDto>();
            if (data == null)
            {
                logger.LogWarning("Le patient n'a pas été créé.");
                return null;
            }

            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création du patient");
            return null;
        }
    }
    
    public async Task<PatientViewModel?> UpdateAsync(int id)
    {
        try
        {
            using var response = await _client.GetAsync($"patient/Get/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du patient : {response}", response.ReasonPhrase);
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
    
    public async Task<PatientViewModel?> UpdateAsync(int id, PatientViewModel value)
    {
        try
        {
            using var response = await _client.PutAsJsonAsync($"patient/Update/{id}", value.ToDto());
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la mise à jour du patient : {response}", response.ReasonPhrase);
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientDto>();
            if (data == null)
            {
                logger.LogWarning("Une erreur est survenue lors de la mise à jour du patient");
                return null;
            }
            
            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la mise à jour du patient");
            return null;
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var response = await _client.DeleteAsync($"patient/Delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la suppression du patient : {response}", response.ReasonPhrase);
                return false;
            }
            
            var data = await response.Content.ReadFromJsonAsync<bool>();
            if (!data)
            {
                logger.LogWarning("Une erreur est survenue lors de la suppression du patient");
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