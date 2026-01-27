using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services.Interfaces;

public interface IPatientNoteServices
{
    Task<PatientNoteViewModel[]> GetAllByPatientIdAsync(int id);
    Task<PatientNoteViewModel?> CreateAsync(PatientNoteViewModel value);
    public Task<PatientNoteViewModel[]> CreateRangeAsync(PatientNoteViewModel[] values);
}
