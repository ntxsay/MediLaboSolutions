using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrontPatient.AspNetCore.Models.Dtos;

public class PatientDto : PatientShared.Models.Dtos.PatientDto
{
    public SelectList? GenresSelectList { get; set; }
}