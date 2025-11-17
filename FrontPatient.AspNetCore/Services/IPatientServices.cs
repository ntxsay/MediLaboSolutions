using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services;

public interface IPatientServices
{
    public Task<PatientViewModel[]> GetAllAsync();
    public Task<PatientViewModel?> DetailAsync(int id);
    public Task<PatientViewModel?> CreateEmptyAsync();
    public Task<PatientViewModel?> CreateAsync(PatientViewModel value);
    public Task<PatientViewModel?> UpdateAsync(int id);
    public Task<PatientViewModel?> UpdateAsync(int id, PatientViewModel value);
    public Task<bool> DeleteAsync(int id);
}