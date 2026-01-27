using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services.Interfaces;

/// <summary>
/// Interface de service permettant de gérer les genres (Féminin/Masculin) de patients.
/// </summary>
/// <remarks>Ce service effectue des appels vers l'API BackPatient.WebApi via le Gateway</remarks>
public interface IGenreServices
{
    /// <summary>
    /// Retourne tous les genres.
    /// </summary>
    /// <returns></returns>
    public Task<GenreViewModel[]> GetAllAsync();
    
    /// <summary>
    /// Retourne un genre spéficié
    /// </summary>
    /// <param name="id">Id du genre à retourner</param>
    /// <returns></returns>
    public Task<GenreViewModel?> GetAsync(int id);
    
    /// <summary>
    /// Insère un genre dans la base de données
    /// </summary>
    /// <param name="value">Données du genre à insérer en provenance du formulaire de création</param>
    /// <returns></returns>
    public Task<GenreViewModel?> CreateAsync(GenreViewModel value);
    
    /// <summary>
    /// Met à jour un genre existant dans la base de données
    /// </summary>
    /// <param name="id">Id du genre à mettre à jour</param>
    /// <param name="value">Données du genre à mettre à jour en provenance du formulaire de mise à jour</param>
    /// <returns></returns>
    public Task<GenreViewModel?> UpdateAsync(int id, GenreViewModel value);
}