namespace BackPatient.Models.Entities;

/// <summary>
/// Modèle de données du patient
/// </summary>
public class PatientEntity
{
    public int Id { get; set; }
    public int GenreId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PostalAddress { get; set; }
    public string? NoTelephone { get; set; }
    
    public virtual GenreEntity Genre { get; set; } = null!;
}