using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services;

public interface ILoginServices
{
    public Task<TokenDto?> LoginAsync(LoginViewModel value);
}