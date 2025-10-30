using BackPatient.WebApi.Utilities;
using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BackPatient.WebApi.Services;

public interface IGenreServices
{
    public Task<GenreDto[]> GetAllAsync();
    public Task<bool> ExistsAsync(string name);
    public Task<bool> ExistsAsync(int id);
    public Task<GenreDto?> CreateAsync(GenreViewModel value);
    public Task<GenreDto[]> CreateAsync(GenreViewModel[] values);
    public Task<GenreDto?> GetAsync(int id);
    public Task<GenreViewModel?> GetViewModelAsync(int id);
    public Task<GenreDto?> GetAsync(string name);
    public Task<GenreViewModel?> GetViewModelAsync(string name);
    public Task<GenreDto?> UpdateAsync(int id, GenreViewModel value);
    public Task<bool> DeleteAsync(int id);
}

public class GenreServices(BackPatientDbContext context, ILogger<GenreServices> logger) : IGenreServices
{
    public async Task<GenreDto[]> GetAllAsync()
    {
        try
        {
            var datas = await context.Genres.AsNoTracking()
                .OrderBy(o => o.Name)
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
            logger.LogError(ex, "Une erreur est survenue lors de la vérification de l'existence du genre");
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
            logger.LogError(ex, "Une erreur est survenue lors de la vérification de l'existence du genre");
            return false;
        }
    }

    public async Task<GenreDto?> CreateAsync(GenreViewModel value)
    {
        try
        {
            var entity = value.ConvertToEntity();
            entity.Id = 0;
            
            await context.Genres.AddAsync(entity);
            await context.SaveChangesAsync();

            var createdEntity = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == entity.Id);
            
            if (createdEntity == null)
            {
                logger.LogWarning("Le genre n°{id} a été créé mais n'a pas été retourné", entity.Id);
                return null;
            }
            
            logger.LogInformation("Le genre {name} a été créé avec succès", createdEntity.Name);
            return createdEntity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création du genre");
            return null;
        }  
    }
    
    public async Task<GenreDto[]> CreateAsync(GenreViewModel[] values)
    {
        if (values.Length == 0)
        {
            logger.LogWarning("Il n'y a pas de genre à ajouter.");
            return [];
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
            
            var createdIds = entities.Select(s => s.Id).ToHashSet();
            
            var createdDtos = await context.Genres.AsNoTracking()
                .OrderBy(o => o.Name)
                .Where(w => createdIds.Contains(w.Id))
                .Select(s => s.ConvertToDto())
                .ToArrayAsync();
            
            if (createdDtos.Length == 0)
            {
                logger.LogWarning("Les genre ont bien été créés mais n'ont pas pu être retournés.");
                return [];
            }
            
            logger.LogInformation("Les genre ont été créés avec succès");
            return createdDtos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création des genres");
            return [];
        }  
    }

    public async Task<GenreViewModel?> GetViewModelAsync(int id)
    {
        try
        {
            var entity = await context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
            if (entity == null)
            {
                logger.LogWarning("Le genre {id} n'a pas été trouvé", id);
                return null;
            }
            
            return entity.ConvertToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du genre");
            return null;
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
                logger.LogWarning("Le genre {id} n'a pas été trouvé", id);
                return null;
            }
            
            return entity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du genre");
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
                logger.LogWarning("Le genre {name} n'a pas été trouvé", name);
                return null;
            }
            
            return entity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du genre");
            return null;
        }        
    }
    
    public async Task<GenreViewModel?> GetViewModelAsync(string name)
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
                logger.LogWarning("Le genre {name} n'a pas été trouvé", name);
                return null;
            }
            
            return entity.ConvertToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération du genre");
            return null;
        }        
    }

    public async Task<GenreDto?> UpdateAsync(int id, GenreViewModel value)
    {
        try
        {
            var entity = await context.Genres.FindAsync(id);
            if (entity == null)
            {
                logger.LogWarning("Le genre {id} n'a pas été trouvé", id);
                return null;
            }
            
            entity.Name = value.Name;
            entity.Description = value.Description;
            
            await context.SaveChangesAsync();
            
            logger.LogInformation("Le genre {name} a été mis à jour avec succès", entity.Name);
            return entity.ConvertToDto();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la mise à jour du genre");
            return null;
        }        
    }

    public async Task<bool> DeleteAsync(int id)
    {
        /*
         Attention : ExecuteDeleteAsync() n'est pas supporté par tous les fournisseurs de base de données Ex : InMemory Database
        await context.Genres.Where(w => w.Id == id).ExecuteDeleteAsync();
        */

        try
        {
            var entity = await context.Genres.FindAsync(id);
            if (entity == null)
            {
                logger.LogWarning("Le genre {id} n'a pas été trouvé", id);
                return false;
            }
            
            context.Genres.Remove(entity);
            await context.SaveChangesAsync();
            
            logger.LogInformation("Le genre n°{id} a été supprimé avec succès", id);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la suppression du genre n°{id}", id);
            return false;
        }
    }
}