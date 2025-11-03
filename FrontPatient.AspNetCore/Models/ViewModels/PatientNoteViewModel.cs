using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FrontPatient.AspNetCore.Models.ViewModels;

public class PatientNoteViewModel
{
    [ValidateNever]
    public string? Id { get; set; }
    
    [DisplayName("Id du patient")]
    [Required(ErrorMessage = "Le nom du patient est requis")]
    [Range(1, int.MaxValue, ErrorMessage = "L'id du patient doit être supérieur à 0")]
    public int PatientId { get; set; }
    
    [DisplayName("Nom du patient")]
    [Required(ErrorMessage = "Le nom du patient est requis")]
    public string PatientName { get; set; } = string.Empty;
    
    [DisplayName("Note")]
    [Required(ErrorMessage = "La note est requise")]
    public string Note { get; set; } = string.Empty;
}