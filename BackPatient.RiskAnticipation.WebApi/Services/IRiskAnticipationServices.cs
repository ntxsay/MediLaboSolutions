using BackPatient.RiskAnticipation.WebApi.Models.Dtos;

namespace BackPatient.RiskAnticipation.WebApi.Services;

public interface IRiskAnticipationServices
{
    public Task<PatientRiskReportDto?> GeneratePatientRiskReport(int patientId);
}