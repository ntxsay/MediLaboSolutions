namespace FrontPatient.AspNetCore.Services;

public interface IPatientRiskReportServices
{
    public Task<string?> GetPatientRiskReportAsync(int patientId);
}