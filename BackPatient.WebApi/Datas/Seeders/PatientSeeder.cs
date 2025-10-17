using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Services;

namespace BackPatient.WebApi.Datas.Seeders;

public class PatientSeeder
{
    public static async Task SeedPatientsAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var patientServices = scope.ServiceProvider.GetRequiredService<IPatientServices>();
        var genreServices = scope.ServiceProvider.GetRequiredService<IGenreServices>();

        var genresToAdd = new HashSet<GenreDto>();
        var femaleGenre = await genreServices.GetAsync("F");
        if (femaleGenre == null)
        {
            femaleGenre = new GenreDto
            {
                Name = "F",
                Description = "Représente un patient de sexe féminin."
            };
            genresToAdd.Add(femaleGenre);
        }
        
        var maleGenre = await genreServices.GetAsync("M");
        if (maleGenre == null)
        {
            maleGenre = new GenreDto
            {
                Name = "M",
                Description = "Représente un patient de sexe masculin."
            };
            genresToAdd.Add(maleGenre);
        }
        
        if (genresToAdd.Count > 0)
            await genreServices.CreateAsync(genresToAdd.ToArray());
        
        var patients = new List<PatientDto>();

        //Patient 1
        const string patient1FirstName = "Test";
        const string patient1LastName = "TestNone";
        var patient1BirthDate = new DateTime(1966, 12, 31);
        
        //Patient 2
        const string patient2FirstName = "Test";
        const string patient2LastName = "TestBorderline";
        var patient2BirthDate = new DateTime(1945, 06, 24);   
        
        //Patient 3
        const string patient3FirstName = "Test";
        const string patient3LastName = "TestInDanger";
        var patient3BirthDate = new DateTime(2004, 06, 18);   
        
        //Patient 4
        const string patient4FirstName = "Test";
        const string patient4LastName = "TestEarlyOnset";
        var patient4BirthDate = new DateTime(2002, 06, 28);   
        
        if (!await patientServices.ExistsAsync(patient1FirstName, patient1LastName, patient1BirthDate))
        {
            patients.Add(new PatientDto
            {
                FirstName = patient1FirstName,
                LastName = patient1LastName,
                BirthDate = patient1BirthDate,
                GenreId = femaleGenre.Id,
                PostalAddress = "1 Brookside St",
                NoTelephone = "100-222-3333",
            });
        }
        
        if (!await patientServices.ExistsAsync(patient2FirstName, patient2LastName, patient2BirthDate))
        {
            patients.Add(new PatientDto
            {
                FirstName = patient2FirstName,
                LastName = patient2LastName,
                BirthDate = patient2BirthDate,
                GenreId = maleGenre.Id,
                PostalAddress = "2 High St",
                NoTelephone = "200-333-4444",
            });
        }
        
        if (!await patientServices.ExistsAsync(patient3FirstName, patient3LastName, patient3BirthDate))
        {
            patients.Add(new PatientDto
            {
                FirstName = patient3FirstName,
                LastName = patient3LastName,
                BirthDate = patient3BirthDate,
                GenreId = maleGenre.Id,
                PostalAddress = "3 Club Road",
                NoTelephone = "300-444-5555",
            });
        }
        
        if (!await patientServices.ExistsAsync(patient4FirstName, patient4LastName, patient4BirthDate))
        {
            patients.Add(new PatientDto
            {
                FirstName = patient4FirstName,
                LastName = patient4LastName,
                BirthDate = patient4BirthDate,
                GenreId = femaleGenre.Id,
                PostalAddress = "4 Valley Dr",
                NoTelephone = "400-555-6666",
            });
        }
        
        if (patients.Count > 0)
            await patientServices.CreateAsync(patients.ToArray());
    }
}