using BackPatient.NoteHistory.WebApi.Models.Dtos;
using BackPatient.NoteHistory.WebApi.Models.Entities;
using BackPatient.NoteHistory.WebApi.Models.ViewModels;

namespace BackPatient.NoteHistory.WebApi.Utilities;

internal static class Converters
{
    public static PatientNoteDto ConvertToDto(this PatientNoteEntity patientNoteEntity)
    {
        return new PatientNoteDto
        {
            Id = patientNoteEntity.Id,
            PatientId = patientNoteEntity.PatientId,
            PatientName = patientNoteEntity.PatientName,
            Note = patientNoteEntity.Note
        };
    }
    
    public static PatientNoteEntity ConvertToEntity(this PatientNoteDto patientNoteDto)
    {
        return new PatientNoteEntity
        {
            Id = patientNoteDto.Id,
            PatientId = patientNoteDto.PatientId,
            PatientName = patientNoteDto.PatientName,
            Note = patientNoteDto.Note
        };
    }
    
    public static PatientNoteViewModel ConvertToViewModel(this PatientNoteDto patientNoteDto)
    {
        return new PatientNoteViewModel
        {
            Id = patientNoteDto.Id,
            PatientId = patientNoteDto.PatientId,
            PatientName = patientNoteDto.PatientName,
            Note = patientNoteDto.Note
        };
    }
    
    public static PatientNoteViewModel ConvertToViewModel(this PatientNoteEntity patientNoteEntity)
    {
        return new PatientNoteViewModel
        {
            Id = patientNoteEntity.Id,
            PatientId = patientNoteEntity.PatientId,
            PatientName = patientNoteEntity.PatientName,
            Note = patientNoteEntity.Note
        };
    }
    
    public static PatientNoteDto ConvertToDto(this PatientNoteViewModel patientNoteViewModel)
    {
        return new PatientNoteDto
        {
            Id = patientNoteViewModel.Id,
            PatientId = patientNoteViewModel.PatientId,
            PatientName = patientNoteViewModel.PatientName,
            Note = patientNoteViewModel.Note
        };
    }
    
    public static PatientNoteEntity ConvertToEntity(this PatientNoteViewModel patientNoteViewModel)
    {
        return new PatientNoteEntity
        {
            Id = patientNoteViewModel.Id,
            PatientId = patientNoteViewModel.PatientId,
            PatientName = patientNoteViewModel.PatientName,
            Note = patientNoteViewModel.Note
        };
    }
}