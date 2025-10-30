namespace BackPatient.WebApi.Models.Dtos;

public class PatientDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public string? PostalAddress { get; set; }
    public string? NoTelephone { get; set; }
    public int GenreId { get; set; }
    public GenreDto Genre { get; set; } = null!;
    public GenreDto[] Genres { get; set; } = [];
}