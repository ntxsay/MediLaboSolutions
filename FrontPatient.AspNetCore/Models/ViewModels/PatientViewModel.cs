using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FrontPatient.AspNetCore.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrontPatient.AspNetCore.Models.ViewModels;

public class PatientViewModel
{
    [ValidateNever]
    public int Id { get; set; }
    
    [DisplayName("Prénom")]
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [MaxLength(60, ErrorMessage = "Le prénom ne peut pas contenir plus de 60 caractères")]
    public string FirstName { get; set; } = string.Empty;
    
    [DisplayName("Nom")]
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [MaxLength(60, ErrorMessage = "Le nom ne peut pas contenir plus de 60 caractères")]
    public string LastName { get; set; } = string.Empty;
    
    [DisplayName("Date de naissance")]
    [Required(ErrorMessage = "La date de naissance est obligatoire")]
    [DateValidation("1920-01-01", "2100-01-01", ErrorMessage = "La date de naissance doit être entre le 01/01/1920 et 01/01/2100")]
    [DataType(DataType.Date)]
    public DateOnly BirthDate { get; set; }
    
    [DisplayName("Adresse postale")]
    [MaxLength(255, ErrorMessage = "L'adresse postale ne peut pas contenir plus de 255 caractères")]
    public string? PostalAddress { get; set; }
    
    [DisplayName("Numéro de téléphone")]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(20, ErrorMessage = "Le numéro de téléphone ne peut pas contenir plus de 20 caractères")]
    public string? NoTelephone { get; set; }
    
    [DisplayName("Id du genre")]
    [Required(ErrorMessage = "Le genre est obligatoire")]
    public int GenreId { get; set; }
    
    [ValidateNever]
    public GenreViewModel Genre { get; set; } = null!;
    
    public SelectList? Genres { get; set; }
}