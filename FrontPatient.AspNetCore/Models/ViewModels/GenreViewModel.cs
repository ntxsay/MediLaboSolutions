using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FrontPatient.AspNetCore.Models.ViewModels;

/// <summary>
/// Modèle de vue pour le formulaire de création et d'édition d'un genre
/// </summary>
public class GenreViewModel
{
    [ValidateNever]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [MaxLength(60, ErrorMessage = "Le prénom ne peut pas contenir plus de 60 caractères")]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}