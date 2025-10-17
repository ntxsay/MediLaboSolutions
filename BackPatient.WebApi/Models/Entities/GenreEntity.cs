namespace BackPatient.WebApi.Models.Entities;

/// <summary>
/// Modèle de données du genre du patient
/// </summary>
public class GenreEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public virtual List<PatientEntity> Patients { get; set; } = [];
}