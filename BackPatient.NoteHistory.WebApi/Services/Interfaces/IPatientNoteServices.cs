using BackPatient.NoteHistory.WebApi.Models.Dtos;
using BackPatient.NoteHistory.WebApi.Models.ViewModels;

namespace BackPatient.NoteHistory.WebApi.Services.Interfaces;

/// <summary>
/// Interface de service permettant de gérer les notes de patients
/// </summary>
public interface IPatientNoteServices
{
    /// <summary>
    /// Retourne toutes les notes de tous les patients confondus
    /// </summary>
    /// <returns></returns>
    Task<PatientNoteDto[]> GetAllAsync();
    
    /// <summary>
    /// Retourne toutes les notes d'un patient spécifique
    /// </summary>
    /// <param name="patientId">Id du patient</param>
    /// <returns></returns>
    Task<PatientNoteDto[]> GetAllByPatientIdAsync(int patientId);
    
    /// <summary>
    /// Retourne toutes les notes d'un patient spécifique, contenant juste l'id du patient et la note
    /// </summary>
    /// <param name="patientId"></param>
    /// <returns></returns>
    public Task<PatientNoteMinimalDto[]> GetAllMinimalByPatientIdAsync(int patientId);
    
    /// <summary>
    /// Retourne une note spécifique
    /// </summary>
    /// <param name="id">Identifiant de la note à récupérer</param>
    /// <returns></returns>
    Task<PatientNoteDto?> GetAsync(string id);
    
    /// <summary>
    /// Insère une note dans la base de données
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<PatientNoteDto?> CreateAsync(PatientNoteViewModel data);
    
    /// <summary>
    /// Insère plusieurs notes dans la base de données
    /// </summary>
    /// <param name="datas"></param>
    /// <returns></returns>
    Task<PatientNoteDto[]> CreateRangeAsync(PatientNoteViewModel[] datas);
    
    /// <summary>
    /// Met à jour une note dans la base de données
    /// </summary>
    /// <param name="id">Identifiant de la note à mettre à jour</param>
    /// <param name="data">Données de la note à mettre à jour</param>
    /// <returns></returns>
    Task<PatientNoteDto?> UpdateAsync(string id, PatientNoteViewModel data);
   
    /// <summary>
    /// Supprime une note de la base de données
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(string id);
}
