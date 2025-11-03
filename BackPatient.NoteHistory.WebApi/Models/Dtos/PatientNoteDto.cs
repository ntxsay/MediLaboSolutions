namespace BackPatient.NoteHistory.WebApi.Models.Dtos;

public class PatientNoteDto
{
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}