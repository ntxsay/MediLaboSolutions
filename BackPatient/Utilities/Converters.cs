using BackPatient.Models.Dtos;
using BackPatient.Models.Entities;

namespace BackPatient.Utilities;

internal static class Converters
{
    public static GenreDto ConvertToDto(this GenreEntity genre)
    {
        return new GenreDto()
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };
    }

    public static GenreEntity ConvertToEntity(this GenreDto dto)
    {
        return new GenreEntity()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }
    
    public static PatientDto ConvertToDto(this PatientEntity patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            PostalAddress = patient.PostalAddress,
            NoTelephone = patient.NoTelephone,
            GenreId = patient.GenreId,
            Genre = patient.Genre.ConvertToDto()
        };
    }
    
    public static PatientEntity ConvertToEntity(this PatientDto dto)
    {
        return new PatientEntity()
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            PostalAddress = dto.PostalAddress,
            NoTelephone = dto.NoTelephone,
            GenreId = dto.GenreId,
            Genre = dto.Genre.ConvertToEntity()
        };
    }
}