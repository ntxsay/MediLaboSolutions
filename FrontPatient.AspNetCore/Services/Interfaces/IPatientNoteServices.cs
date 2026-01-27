using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services.Interfaces;

/// <summary>
/// Interface de service permettant de gérer les notes de patients
/// </summary>
/// <remarks>Ce service effectue des appels vers l'API BackPatient.NoteHistory.WebApi via le Gateway</remarks>
public interface IPatientNoteServices
{
    /// <summary>
    /// Retourne toutes les notes du patient spécifié
    /// </summary>
    /// <param name="id">Id du patient</param>
    /// <returns></returns>
    Task<PatientNoteViewModel[]> GetAllByPatientIdAsync(int id);
    
    /// <summary>
    /// Insère une note de patient dans la base de données (MongoDb)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    Task<PatientNoteViewModel?> CreateAsync(PatientNoteViewModel value);
    
    #warning  Finir d'implémenter l'idée complete ou la supprimer
    
    public Task<PatientNoteViewModel[]> CreateRangeAsync(PatientNoteViewModel[] values);
}
