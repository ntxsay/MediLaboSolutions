using System.ComponentModel.DataAnnotations;

namespace FrontPatient.AspNetCore.Attributes;

public class DateValidationAttribute(string minimumDate, string maximumDate) : ValidationAttribute
{
    public string MinimumDate { get; } = minimumDate;
    public string MaximumDate { get; } = maximumDate;
    
    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        if (value is DateOnly date)
        {
            if (!DateOnly.TryParse(MinimumDate, out var minDate))
            {
                return new ValidationResult("La date minimale n'est pas valide");
            }

            if (!DateOnly.TryParse(MaximumDate, out var maxDate))
            {
                return new ValidationResult("La date maximale n'est pas valide");
            }
            
            return (date >= minDate && date <= maxDate) 
                ? ValidationResult.Success 
                : new ValidationResult(ErrorMessage); 
        }

        return ValidationResult.Success;
    }
}