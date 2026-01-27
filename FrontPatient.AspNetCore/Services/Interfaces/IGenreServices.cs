using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services.Interfaces;

public interface IGenreServices
{
    public Task<GenreViewModel[]> GetAllAsync();
    public Task<GenreViewModel?> GetAsync(int id);
    public Task<GenreViewModel?> CreateAsync(GenreViewModel value);
    public Task<GenreViewModel?> UpdateAsync(int id, GenreViewModel value);
    public Task<bool> DeleteAsync(int id);
}