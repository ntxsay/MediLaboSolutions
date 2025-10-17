using BackPatient.WebApi.Utilities;
using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackPatient.WebApi.Services;

public interface IGenreServices
{
    public Task<GenreDto[]> GetAllPatientsAsync();
    public Task<bool> ExistsAsync(string name);
    public Task<bool> ExistsAsync(int id);
    public Task<bool> CreateAsync(GenreDto value);
    public Task<bool> CreateAsync(GenreDto[] values);
    public Task<GenreDto?> GetAsync(int id);
    public Task<GenreDto?> GetAsync(string name);
    public Task<bool> UpdateAsync(int id, GenreDto value);
    public Task<bool> DeleteAsync(int id);
}

public class GenreServices(BackPatientDbContext context, ILogger<GenreServices> logger) : IGenreServices
{
    public async Task<GenreDto[]> GetAllPatientsAsync()
    {
        try
        {
            var datas = await context.Genres.AsNoTracking().Select(s => s.ConvertToDto()).
                OrderBy(o => o.Name)
                .ToArrayAsync();
            return datas;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération des patients: {ex.Message}");
            return [];
        }
    }

    public async Task<bool> ExistsAsync(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            logger.LogWarning("Le nom du genre ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return false;
        }
        
        try
        {
            return await context.Genres.AnyAsync(g => g.Name == name);
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la vérification de l'existence du genre: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await context.Genres.AnyAsync(g => g.Id == id);
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la vérification de l'existence du genre: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> CreateAsync(GenreDto value)
    {
        try
        {
            var entity = value.ConvertToEntity();
            entity.Id = 0;
            
            await context.Genres.AddAsync(entity);
            await context.SaveChangesAsync();

            value.Id = entity.Id;
            
            logger.LogInformation($"Le genre {value.Name} a été créé avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la création du genre: {ex.Message}");
            return false;
        }  
    }
    
    public async Task<bool> CreateAsync(GenreDto[] values)
    {
        if (values.Length == 0)
        {
            logger.LogWarning("Il n'y a pas de genre à ajouter.");
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
            
            await context.Genres.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            
            foreach (var entity in entities)
            {
                var dto = values.Single(s => string.Equals(s.Name, entity.Name, StringComparison.CurrentCultureIgnoreCase));
                dto.Id = entity.Id;
            }
            
            logger.LogInformation("Les genre ont été créés avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la création des genres: {ex.Message}");
            return false;
        }  
    }

    public async Task<GenreDto?> GetAsync(int id)
    {
        try
        {
            var entity = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
            if (entity == null)
            {
                logger.LogError($"Le genre {id} n'a pas été trouvé");
                return null;
            }
            
            return entity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération du genre: {ex.Message}");
            return null;
        }        
    }
    
    public async Task<GenreDto?> GetAsync(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            logger.LogWarning("Le nom du genre ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return null;
        }
        
        try
        {
            var entity = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Name == name);
            if (entity == null)
            {
                logger.LogError($"Le genre {name} n'a pas été trouvé");
                return null;
            }
            
            return entity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la récupération du genre: {ex.Message}");
            return null;
        }        
    }

    public async Task<bool> UpdateAsync(int id, GenreDto value)
    {
        try
        {
            var entity = await context.Genres.FindAsync(id);
            if (entity == null)
            {
                logger.LogError($"Le genre {id} n'a pas été trouvé");
                return false;
            }
            
            entity.Name = value.Name;
            entity.Description = value.Description;
            
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Le genre {value.Name} a été mis à jour avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la mise à jour du genre: {ex.Message}");
            return false;
        }        
    }

    public async Task<bool> DeleteAsync(int id)
    {
        /*
         Attention : ExecuteDeleteAsync() n'est pas supporté par tous les fournisseurs de base de données Ex:InMemory Database
        await context.Genres.Where(w => w.Id == id).ExecuteDeleteAsync();
        */

        try
        {
            var entity = await context.Genres.FindAsync(id);
            if (entity == null)
            {
                logger.LogError($"Le genre {id} n'a pas été trouvé");
                return false;
            }
            
            context.Genres.Remove(entity);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Le genre n°{id} a été supprimé avec succès");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Une erreur est survenue lors de la suppression du genre n°{id}: {ex.Message}");
            return false;
        }
    }
}