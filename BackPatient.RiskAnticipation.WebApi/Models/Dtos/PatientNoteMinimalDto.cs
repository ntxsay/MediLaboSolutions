namespace BackPatient.RiskAnticipation.WebApi.Models.Dtos;

public class PatientNoteMinimalDto
{
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
}