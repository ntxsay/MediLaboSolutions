using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BackPatient.Tests;

public class PatientTests : IDisposable
{
    private readonly BackPatientDbContext _dbContext;
    private readonly IPatientServices _patientServices;
    
    public PatientTests()
    {
        var options = new DbContextOptionsBuilder<BackPatientDbContext>()
            .UseInMemoryDatabase("mediLaboDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new BackPatientDbContext(options);
        _patientServices = new PatientServices(_dbContext, 
            new LoggerFactory().CreateLogger<PatientServices>());
        
        _dbContext.Database.EnsureCreated();
    }
    
    [Fact]
    public async Task CreatePatientAsync()
    {
        // Arrange
        var patientDto = new PatientDto
        {
            LastName = "Last name of atient" + new Random().Next(1, 100),
            FirstName = "First name of atient" + new Random().Next(1, 100),
            BirthDate = DateTime.Now,
            PostalAddress = "Postal address of atient" + new Random().Next(1, 100),
            NoTelephone = "No telephone of atient" + new Random().Next(1, 100),
            GenreId = 0,
            Genre = new GenreDto()
            {
                Name = new Random().Next(1, 4) > 2 ? "M" : "F"
            }
        };

        // Act
        var result = await _patientServices.CreateAsync(patientDto);

        // Assert
        Assert.True(result);
    }
 
    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}