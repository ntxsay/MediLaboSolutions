namespace FrontPatient.AspNetCore.Services.Interfaces;

/// <summary>
/// Interface de service permettant de peupler la base de données de notes de patients
/// </summary>
public interface IPatientNoteSeedServices
{
    /// <summary>
    /// Permet de peupler la base de données de notes de patients
    /// </summary>
    /// <param name="patientLastNameIdDictionary">Dictionnaire contenant le nom et l'id des patients existants dans la base de données</param>
    /// <returns></returns>
    /// <remarks>Attention au risque de duplication des données si exécuté plusieurs fois</remarks> 
    public Task SeedNotesAsync(Dictionary<string, int> patientLastNameIdDictionary);
}