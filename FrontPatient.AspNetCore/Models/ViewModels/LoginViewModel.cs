using System.ComponentModel.DataAnnotations;

namespace FrontPatient.AspNetCore.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}