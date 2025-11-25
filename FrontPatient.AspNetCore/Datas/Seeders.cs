using FrontPatient.AspNetCore.Services;

namespace FrontPatient.AspNetCore.Datas;

public static class Seeders
{
    public static async Task SeedPatientNotesAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var patientNoteSeedServices = scope.ServiceProvider.GetRequiredService<IPatientNoteSeedServices>();
        await patientNoteSeedServices.SeedNotesAsync();
    }
}