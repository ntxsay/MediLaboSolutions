namespace BackPatient.NoteHistory.WebApi.Models;

/// <summary>
/// Configuration de la base de données MongoDB
/// </summary>
public class MediLaboSolutionsDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string CollectionName { get; set; } = string.Empty;
}