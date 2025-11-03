using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrontPatient.AspNetCore.Utilities;

public static class Converters
{
    public static GenreDto ToDto(this GenreViewModel viewModel)
    {
        return new GenreDto
        {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Description = viewModel.Description
        };
    }
    
    public static GenreViewModel ToViewModel(this GenreDto dto)
    {
        return new GenreViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }
    
    public static PatientDto ToDto(this PatientViewModel viewModel)
    {
        return new PatientDto
        {
            Id = viewModel.Id,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            BirthDate = viewModel.BirthDate,
            PostalAddress = viewModel.PostalAddress,
            NoTelephone = viewModel.NoTelephone,
            GenreId = viewModel.GenreId,
            Genre = viewModel.Genre?.ToDto(),
            Genres = []
        };
    }
    public static PatientViewModel ToViewModel(this PatientDto dto)
    {
        return new PatientViewModel
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            PostalAddress = dto.PostalAddress,
            NoTelephone = dto.NoTelephone,
            GenreId = dto.GenreId,
            Genre = dto.Genre?.ToViewModel(),
            Genres = new SelectList(dto.Genres.Select(s => s.ToViewModel()).ToArray(), nameof(GenreViewModel.Id), nameof(GenreViewModel.Name)), 
        };
    }
    
    public static PatientNoteViewModel ToViewModel(this PatientNoteDto dto)
    {
        return new PatientNoteViewModel
        {
            Id = dto.Id,
            PatientId = dto.PatientId,
            PatientName = dto.PatientName,
            Note = dto.Note
        };
    }
    
    public static PatientNoteDto ToDto(this PatientNoteViewModel viewModel)
    {
        return new PatientNoteDto
        {
            Id = viewModel.Id,
            PatientId = viewModel.PatientId,
            PatientName = viewModel.PatientName,
            Note = viewModel.Note
        };
    }
}