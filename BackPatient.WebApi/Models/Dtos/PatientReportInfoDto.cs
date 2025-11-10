namespace BackPatient.WebApi.Models.Dtos;

public class PatientReportInfoDto
{
    public int PatientId { get; set; }
    public byte PatientAge { get; set; }
    public string PatientGender { get; set; } = string.Empty;
}