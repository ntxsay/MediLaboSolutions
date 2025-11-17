using BackPatient.WebApi.Utilities;
using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BackPatient.WebApi.Services;

public interface IGenreServices
{
    public Task<GenreDto[]> GetAllAsync();
    public Task<bool> ExistsAsync(string name);
    public Task<bool> ExistsAsync(int id);
    public Task<GenreDto?> CreateAsync(GenreViewModel value);
    public Task<GenreDto[]> CreateAsync(GenreViewModel[] values);
    public Task<GenreDto?> GetAsync(int id);
    public Task<GenreViewModel?> GetViewModelAsync(int id);
    public Task<GenreDto?> GetAsync(string name);
    public Task<GenreViewModel?> GetViewModelAsync(string name);
    public Task<GenreDto?> UpdateAsync(int id, GenreViewModel value);
    public Task<bool> DeleteAsync(int id);
}