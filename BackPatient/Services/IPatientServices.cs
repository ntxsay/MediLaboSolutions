using BackPatient.Datas;
using BackPatient.Models.Dtos;
using BackPatient.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BackPatient.Services;

public interface IPatientServices
{
    public Task<PatientDto[]> GetAllPatientsAsync();
    public Task<bool> CreateAsync(PatientDto value);
    public Task<PatientDto?> GetAsync(int id);
    public Task<bool> UpdateAsync(int id, PatientDto value);
    public Task DeleteAsync(int id);
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

    public async Task<bool> CreateAsync(PatientDto value)
    {
        try
        {
            var entity = value.ConvertToEntity();
            entity.Id = 0;
            
            await context.Patients.AddAsync(entity);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Le patient {value.FirstName} {value.LastName} a été créé avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la création du patient: {ex.Message}");
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

    public async Task DeleteAsync(int id)
    {
        await context.Patients.Where(w => w.Id == id).ExecuteDeleteAsync();
    }
}