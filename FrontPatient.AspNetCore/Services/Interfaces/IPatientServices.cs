using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services.Interfaces;

/// <summary>
/// Interface de service permettant de gérer les patients.
/// </summary>
/// <remarks>Ce service effectue des appels vers l'API BackPatient.WebApi via le Gateway</remarks>
public interface IPatientServices
{
    /// <summary>
    /// Retourne tous les patients
    /// </summary>
    /// <returns></returns>
    public Task<PatientViewModel[]> GetAllAsync();
    
    /// <summary>
    /// Retourne un patient spécifique 
    /// </summary>
    /// <param name="id">Id du patient à retourner</param>
    /// <returns></returns>
    public Task<PatientViewModel?> GetAsync(int id);
    
    /// <summary>
    /// Crée une nouvelle instance vide d'un patient à utiliser dans la méthode GET du formulaire de création d'un patient
    /// </summary>
    /// <returns></returns>
    public Task<PatientViewModel?> CreateEmptyAsync();
    
    /// <summary>
    /// Insère un nouveau patient dans la base de données
    /// </summary>
    /// <param name="value">Données du patient à insérer en provenance du formulaire de création d'un patient</param>
    /// <returns></returns>
    public Task<PatientViewModel?> CreateAsync(PatientViewModel value);
    
    /// <summary>
    /// Met à jour un patient existant dans la base de données
    /// </summary>
    /// <param name="id">Id du patient à mettre à jour</param>
    /// <param name="value">Données du patient à mettre à jour en provenance du formulaire d'édition d'un patient</param>
    /// <returns></returns>
    public Task<PatientViewModel?> UpdateAsync(int id, PatientViewModel value);
    public Task<bool> DeleteAsync(int id);
}