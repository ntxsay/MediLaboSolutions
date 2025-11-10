using BackPatient.WebApi.Utilities;
using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BackPatient.WebApi.Services;

public interface IPatientServices
{
    public Task<PatientDto[]> GetAllAsync();
    public Task<bool> ExistsAsync(string firstName, string lastName, DateOnly birthDate);
    public Task<bool> ExistsAsync(int id);
    public Task<PatientDto?> CreateEmptyAsync();
    public Task<PatientDto?> CreateAsync(PatientViewModel value);
    public Task<PatientDto[]> CreateAsync(PatientViewModel[] values);
    public Task<PatientDto?> DetailsAsync(int id);
    public Task<PatientDto?> GetAsync(int id);
    public Task<PatientViewModel?> GetViewModelAsync(int id);
    public Task<PatientReportInfoDto?> GetReportInfoAsync(int id);
    public Task<PatientDto?> UpdateAsync(int id, PatientViewModel value);
    public Task<bool> DeleteAsync(int id);
}

public class PatientServices(BackPatientDbContext context, IGenreServices genreServices, ILogger<PatientServices> logger) : IPatientServices
{
    public async Task<PatientDto[]> GetAllAsync()
    {
        try
        {
            var datas = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .OrderBy(o => o.LastName)
                .ThenBy(o => o.FirstName)
                .Select(s => s.ConvertToDto())
                .ToArrayAsync();
            return datas;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération des patients");
            return [];
        }
    }
    
    public async Task<bool> ExistsAsync(string firstName, string lastName, DateOnly birthDate)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(firstName))
        {
            logger.LogWarning("Le prénom du patient ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return false;
        }
        
        if (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName))
        {
            logger.LogWarning("Le nom du patient ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return false;
        }
        
        try
        {
            return await context.Patients.AnyAsync(g => g.FirstName == firstName && g.LastName == lastName && g.BirthDate == birthDate);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la vérification de l'existence du patient");
            return false;
        }
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await context.Patients.AnyAsync(g => g.Id == id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la vérification de l'existence du patient");
            return false;
        }
    }
    
    public async Task<PatientDto?> CreateEmptyAsync()
    {
        var dto = new PatientDto
        {
            Genres = await genreServices.GetAllAsync()
        };
        return dto;  
    }

    public async Task<PatientDto?> CreateAsync(PatientViewModel value)
    {
        try
        {
            var entity = value.ConvertToEntity();
            entity.Id = 0;
            entity.Genre = null!;
            
            await context.Patients.AddAsync(entity);
            await context.SaveChangesAsync();
            
            var createdEntity = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .FirstOrDefaultAsync(i => i.Id == entity.Id);
            
            if (createdEntity == null)
            {
                logger.LogWarning("Le patient {firstName} {lastName} a bien été créé mais n'a pas pu être retourné.", value.FirstName, value.LastName);
                return null;
            }
            
            logger.LogInformation("Le patient {firstName} {lastName} a été créé avec succès", value.FirstName, value.LastName);
            return createdEntity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création du patient");
            return null;
        }  
    }
    
    public async Task<PatientDto[]> CreateAsync(PatientViewModel[] values)
    {
        if (values.Length == 0)
        {
            logger.LogWarning("Il n'y a pas de patient à ajouter.");
            return [];
        }
        
        try
        {
            var entities = values.Select(s =>
            {
                var entity = s.ConvertToEntity();
                entity.Id = 0;
                entity.Genre = null!;
                return entity;
            }).ToArray();
            
            await context.Patients.AddRangeAsync(entities);
            await context.SaveChangesAsync();

            var createdIds = entities.Select(s => s.Id).ToHashSet();
            var createdDtos = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .OrderBy(o => o.LastName)
                .ThenBy(o => o.FirstName)
                .Where(w => createdIds.Contains(w.Id))
                .Select(s => s.ConvertToDto())
                .ToArrayAsync();
            
            if (createdDtos.Length == 0)
            {
                logger.LogInformation("Les patients ont bien été créés mais n'ont pas pu être retournés.");
                return [];
            }    
            
            logger.LogInformation("Les patients ont été créés avec succès");
            return createdDtos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création des patients.");
            return [];
        }  
    }

    public async Task<PatientDto?> DetailsAsync(int id)
    {
        try
        {
            var entity = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (entity == null)
            {
                logger.LogWarning("Le patient {id} n'a pas été trouvé", id);
                return null;
            }
            
            return entity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du patient");
            return null;
        }        
    }
    
    public async Task<PatientDto?> GetAsync(int id)
    {
        try
        {
            var entity = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (entity == null)
            {
                logger.LogWarning("Le patient {id} n'a pas été trouvé", id);
                return null;
            }
            
            var dto = entity.ConvertToDto();
            dto.Genres = await genreServices.GetAllAsync();
            return dto;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du patient");
            return null;
        }        
    }
    
    public async Task<PatientViewModel?> GetViewModelAsync(int id)
    {
        try
        {
            var entity = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (entity == null)
            {
                logger.LogWarning("Le patient {id} n'a pas été trouvé", id);
                return null;
            }
            
            return entity.ConvertToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du patient");
            return null;
        }        
    }
    
    public async Task<PatientReportInfoDto?> GetReportInfoAsync(int id)
    {
        try
        {
            var entity = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (entity == null)
            {
                logger.LogWarning("Le patient n°{id} n'a pas été trouvé", id);
                return null;
            }
            
            return new PatientReportInfoDto
            {
                PatientId = entity.Id,
                PatientAge = (byte)(DateTime.Now.Year - entity.BirthDate.Year),
                PatientGender = entity.Genre.Name
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du patient n°{id}", id);
            return null;
        }        
    }

    public async Task<PatientDto?> UpdateAsync(int id, PatientViewModel value)
    {
        try
        {
            var entity = await context.Patients.FindAsync(id);
            if (entity == null)
            {
                logger.LogWarning("Le patient {id} n'a pas été trouvé", id);
                return null;
            }
            
            entity.FirstName = value.FirstName;
            entity.LastName = value.LastName;
            entity.GenreId = value.GenreId;
            entity.BirthDate = value.BirthDate;
            entity.PostalAddress = value.PostalAddress;
            entity.NoTelephone = value.NoTelephone;
            
            await context.SaveChangesAsync();
            
            var updatedEntity = await context.Patients.AsNoTracking()
                .Include(i => i.Genre)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (updatedEntity == null)
            {
                logger.LogWarning("Le patient n°{id} a été mis à jour mais n'a pas été retourné", id);
                return null;
            }
            
            logger.LogInformation("Le patient {firstName} {lastName} a été mis à jour avec succès", value.FirstName, value.LastName);
            return updatedEntity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la mise à jour du patient");
            return null;
        }        
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await context.Patients.FindAsync(id);
            if (entity == null)
            {
                logger.LogWarning("Le genre {id} n'a pas été trouvé", id);
                return false;
            }
            
            context.Patients.Remove(entity);
            await context.SaveChangesAsync();
            
            logger.LogInformation("Le patient n°{id} a été supprimé avec succès", id);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Une erreur est survenue lors de la suppression du patient n°{id}", id);
            return false;
        }
    }
}