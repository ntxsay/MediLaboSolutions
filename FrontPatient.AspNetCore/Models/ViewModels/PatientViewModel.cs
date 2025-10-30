using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrontPatient.AspNetCore.Models.ViewModels;

public class PatientViewModel
{
    [ValidateNever]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [MaxLength(60, ErrorMessage = "Le prénom ne peut pas contenir plus de 60 caractères")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [MaxLength(60, ErrorMessage = "Le nom ne peut pas contenir plus de 60 caractères")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La date de naissance est obligatoire")]
    public DateOnly BirthDate { get; set; }
    public string? PostalAddress { get; set; }
    public string? NoTelephone { get; set; }
    
    [Required(ErrorMessage = "Le genre est obligatoire")]
    public int GenreId { get; set; }
    
    [ValidateNever]
    public GenreViewModel Genre { get; set; } = null!;
    
    public SelectList? Genres { get; set; }
}