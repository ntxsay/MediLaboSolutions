namespace FrontPatient.AspNetCore.Services.Interfaces;

public interface IPatientRiskReportServices
{
    public Task<string?> GetPatientRiskReportAsync(int patientId);
}