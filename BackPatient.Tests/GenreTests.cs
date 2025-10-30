using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.ViewModels;
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
        var genreDto = new GenreViewModel
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var result = await _genreServices.CreateAsync(genreDto);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetGenreAsync()
    {
        // Arrange
        var genreDto = new GenreViewModel()
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var data = await _genreServices.CreateAsync(genreDto);
        

        // Assert
        Assert.NotNull(data);
    }
    
    [Fact]
    public async Task UpdateGenreAsync()
    {
        // Arrange
        var viewModel = new GenreViewModel()
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var data = await _genreServices.CreateAsync(viewModel);
        if (data == null)
        {
            Assert.Fail("Le genre n'a pas été créé");
            return;
        }

        viewModel.Description = "Nouvelle description";
        var result = await _genreServices.UpdateAsync(viewModel.Id, viewModel);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task DeleteGenreAsync()
    {
        // Arrange
        var viewModel = new GenreViewModel()
        {
            Name = "Test Genre" + new Random().Next(1, 100)
        };

        // Act
        var data = await _genreServices.CreateAsync(viewModel);
        if (data == null)
        {
            Assert.Fail("Le genre n'a pas été créé");
            return;
        }

        var isDeleted = await _genreServices.DeleteAsync(viewModel.Id);

        // Assert
        Assert.True(isDeleted);
    }
    
    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}