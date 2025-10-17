using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BackPatient.Tests;

public class GenreTests : IDisposable
{
    private readonly BackPatientDbContext _dbContext;
    private readonly IGenreServices _genreServices;
    
    public GenreTests()
    {
        var options = new DbContextOptionsBuilder<BackPatientDbContext>()
            .UseInMemoryDatabase("mediLaboDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new BackPatientDbContext(options);
        _genreServices = new GenreServices(_dbContext, 
            new LoggerFactory().CreateLogger<GenreServices>());
        
        _dbContext.Database.EnsureCreated();
    }
    
    [Fact]
    public async Task CreateGenreAsync()
    {
        // Arrange
        var genreDto = new GenreDto
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var result = await _genreServices.CreateAsync(genreDto);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task GetGenreAsync()
    {
        // Arrange
        var genreDto = new GenreDto
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var isCreated = await _genreServices.CreateAsync(genreDto);
        if (!isCreated)
        {
            Assert.Fail("Le genre n'a pas été créé");
            return;
        }
        
        var result = await _genreServices.GetAsync(genreDto.Id);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task UpdateGenreAsync()
    {
        // Arrange
        var genreDto = new GenreDto
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var isCreated = await _genreServices.CreateAsync(genreDto);
        if (!isCreated)
        {
            Assert.Fail("Le genre n'a pas été créé");
            return;
        }

        genreDto.Description = "Nouvelle description";
        var result = await _genreServices.UpdateAsync(genreDto.Id, genreDto);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteGenreAsync()
    {
        // Arrange
        var genreDto = new GenreDto
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var isCreated = await _genreServices.CreateAsync(genreDto);
        if (!isCreated)
        {
            Assert.Fail("Le genre n'a pas été créé");
            return;
        }

        await _genreServices.DeleteAsync(genreDto.Id);

        // Assert
        Assert.False(await _genreServices.ExistsAsync(genreDto.Id));
    }
    
    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}