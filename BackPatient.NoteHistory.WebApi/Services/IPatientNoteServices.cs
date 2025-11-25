using BackPatient.NoteHistory.WebApi.Models.Dtos;
using BackPatient.NoteHistory.WebApi.Models.ViewModels;

namespace BackPatient.NoteHistory.WebApi.Services;

public interface IPatientNoteServices
{
    Task<PatientNoteDto[]> GetAllAsync();
    Task<PatientNoteDto[]> GetAllByPatientIdAsync(int patientId);
    public Task<PatientNoteMinimalDto[]> GetAllMinimalByPatientIdAsync(int patientId);
    Task<PatientNoteDto?> GetAsync(string id);
    Task<PatientNoteDto?> CreateAsync(PatientNoteViewModel data);
    Task<PatientNoteDto[]> CreateRangeAsync(PatientNoteViewModel[] datas);
    Task<PatientNoteDto?> UpdateAsync(string id, PatientNoteViewModel data);
    Task<bool> RemoveAsync(string id);
}
