using BackPatient.NoteHistory.WebApi.Models.Dtos;
using BackPatient.NoteHistory.WebApi.Models.Entities;

namespace BackPatient.NoteHistory.WebApi.Utilities;

internal static class Converters
{
    public static PatientNoteDto ConvertToDto(this PatientNoteEntity patientNoteEntity)
    {
        return new PatientNoteDto
        {
            PatientId = patientNoteEntity.PatientId,
            PatientName = patientNoteEntity.PatientName,
            Note = patientNoteEntity.Note
        };
    }
    
    public static PatientNoteEntity ConvertToEntity(this PatientNoteDto patientNoteDto)
    {
        return new PatientNoteEntity
        {
            PatientId = patientNoteDto.PatientId,
            PatientName = patientNoteDto.PatientName,
            Note = patientNoteDto.Note
        };
    }
}