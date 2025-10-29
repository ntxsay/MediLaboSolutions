using System.ComponentModel.DataAnnotations;

namespace PatientShared.Models.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}