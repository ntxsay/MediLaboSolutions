using BackPatient.WebApi.Utilities;
using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackPatient.WebApi.Services;

public interface IPatientServices
{
    public Task<PatientDto[]> GetAllPatientsAsync();
    public Task<bool> ExistsAsync(string firstName, string lastName, DateTime birthDate);
    public Task<bool> ExistsAsync(int id);
    public Task<bool> CreateAsync(PatientDto value);
    public Task<bool> CreateAsync(PatientDto[] values);
    public Task<PatientDto?> GetAsync(int id);
    public Task<bool> UpdateAsync(int id, PatientDto value);
    public Task<bool> DeleteAsync(int id);
}

public class PatientServices(BackPatientDbContext context, ILogger<PatientServices> logger) : IPatientServices
{
    public async Task<PatientDto[]> GetAllPatientsAsync()
    {
        try
        {
            var datas = await context.Patients.AsNoTracking().Select(s => s.ConvertToDto()).
                OrderBy(o => o.LastName)
                .ThenBy(o => o.FirstName)
                .ToArrayAsync();
            return datas;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération des patients: {ex.Message}");
            return [];
        }
    }
    
    public async Task<bool> ExistsAsync(string firstName, string lastName, DateTime birthDate)
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
            logger.LogError($"Une erreur est survenue lors de la vérification de l'existence du patient: {ex.Message}");
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
            logger.LogError($"Une erreur est survenue lors de la vérification de l'existence du patient: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> CreateAsync(PatientDto value)
    {
        try
        {
            var entity = value.ConvertToEntity();
            entity.Id = 0;
            
            await context.Patients.AddAsync(entity);
            await context.SaveChangesAsync();
            
            value.Id = entity.Id;      
            
            logger.LogInformation($"Le patient {value.FirstName} {value.LastName} a été créé avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la création du patient: {ex.Message}");
            return false;
        }  
    }
    
    public async Task<bool> CreateAsync(PatientDto[] values)
    {
        if (values.Length == 0)
        {
            logger.LogWarning("Il n'y a pas de patient à ajouter.");
            return false;
        }
        
        try
        {
            var entities = values.Select(s =>
            {
                var entity = s.ConvertToEntity();
                entity.Id = 0;
                return entity;
            }).ToArray();
            
            await context.Patients.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            
            foreach (var entity in entities)
            {
                var dto = values.Single(s => string.Equals(s.FirstName, entity.FirstName, StringComparison.CurrentCultureIgnoreCase) && string.Equals(s.LastName, entity.LastName, StringComparison.CurrentCultureIgnoreCase) && s.BirthDate == entity.BirthDate);
                dto.Id = entity.Id;
            }
            
            logger.LogInformation("Les patients ont été créés avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la création des patients: {ex.Message}");
            return false;
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
                logger.LogError($"Le patient {id} n'a pas été trouvé");
                return null;
            }
            
            return entity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération du patient: {ex.Message}");
            return null;
        }        
    }

    public async Task<bool> UpdateAsync(int id, PatientDto value)
    {
        try
        {
            var entity = await context.Patients.FindAsync(id);
            if (entity == null)
            {
                logger.LogError($"Le patient {id} n'a pas été trouvé");
                return false;
            }
            
            entity.FirstName = value.FirstName;
            entity.LastName = value.LastName;
            entity.GenreId = value.GenreId;
            entity.BirthDate = value.BirthDate;
            entity.PostalAddress = value.PostalAddress;
            entity.NoTelephone = value.NoTelephone;
            
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Le patient {value.FirstName} {value.LastName} a été mis à jour avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la mise à jour du patient: {ex.Message}");
            return false;
        }        
    }

    public async Task<bool> DeleteAsync(int id)
    {
        /*
         Attention : ExecuteDeleteAsync() n'est pas supporté par tous les fournisseurs de base de données Ex:InMemory Database
        await context.Patients.Where(w => w.Id == id).ExecuteDeleteAsync();
        */

        try
        {
            var entity = await context.Patients.FindAsync(id);
            if (entity == null)
            {
                logger.LogError($"Le genre {id} n'a pas été trouvé");
                return false;
            }
            
            context.Patients.Remove(entity);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Le patient n°{id} a été supprimé avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la suppression du patient n°{id}: {ex.Message}");
            return false;
        }
    }
}