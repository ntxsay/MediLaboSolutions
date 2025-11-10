namespace FrontPatient.AspNetCore.Models.Dtos;

public class PatientRiskReportDto
{
    public int PatientId { get; set; }
    public string Report { get; set; } = string.Empty;
}