using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.ViewModels;

namespace BackPatient.WebApi.Services;
public interface IPatientServices
{
    public Task<PatientDto[]> GetAllAsync();
    public Task<bool> ExistsAsync(string firstName, string lastName, DateOnly birthDate);
    public Task<bool> ExistsAsync(int id);
    public Task<PatientDto?> CreateEmptyAsync();
    public Task<PatientDto?> CreateAsync(PatientViewModel value);
    public Task<PatientDto[]> CreateAsync(PatientViewModel[] values);
    public Task<PatientDto?> DetailsAsync(int id);
    public Task<PatientDto?> GetAsync(int id);
    public Task<PatientViewModel?> GetViewModelAsync(int id);
    public Task<PatientReportInfoDto?> GetReportInfoAsync(int id);
    public Task<PatientDto?> UpdateAsync(int id, PatientViewModel value);
    public Task<bool> DeleteAsync(int id);
}