namespace FrontPatient.AspNetCore.Models.Settings;

/// <summary>
/// Classe contenant les paramètres de seeders
/// </summary>
public record SeederSettings
{
    /// <summary>
    /// Indique si le seeder de notes de patients doit être exécuté
    /// </summary>
    public bool SeedPatientNotes { get; init; }
}