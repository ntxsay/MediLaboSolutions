using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrontPatient.AspNetCore.Models.ViewModels;

/// <summary>
/// Modèle de vue pour le formulaire de connexion
/// </summary>
public class LoginViewModel
{
    [DisplayName("Nom d'utilisateur")]
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string UserName { get; set; } = string.Empty;
    
    [DisplayName("Mot de passe")]
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}