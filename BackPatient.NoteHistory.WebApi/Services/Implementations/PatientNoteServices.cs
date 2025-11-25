using BackPatient.NoteHistory.WebApi.Models;
using BackPatient.NoteHistory.WebApi.Models.Dtos;
using BackPatient.NoteHistory.WebApi.Models.Entities;
using BackPatient.NoteHistory.WebApi.Models.ViewModels;
using BackPatient.NoteHistory.WebApi.Utilities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BackPatient.NoteHistory.WebApi.Services.Implementations;

public class PatientNoteServices : IPatientNoteServices
{
    private readonly IMongoCollection<PatientNoteEntity> _patientNotesCollection;
    private readonly ILogger<PatientNoteServices> _logger;

    public PatientNoteServices(ILogger<PatientNoteServices> logger,
        IOptions<MediLaboSolutionsDatabaseSettings> bookStoreDatabaseSettings)
    {
        _logger = logger;
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _patientNotesCollection = mongoDatabase.GetCollection<PatientNoteEntity>(
            bookStoreDatabaseSettings.Value.CollectionName);
    }

    public async Task<PatientNoteDto[]> GetAllAsync()
    {
        try
        {
            var datas =  await _patientNotesCollection.Find(_ => true).ToListAsync();
            return datas.Select(s => s.ConvertToDto()).ToArray();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la récupération des notes");
            return [];
        }
    }
    
    public async Task<PatientNoteDto[]> GetAllByPatientIdAsync(int patientId)
    {
        try
        {
            var datas =  await _patientNotesCollection.Find(x => x.PatientId == patientId).ToListAsync();
            return datas.Select(s => s.ConvertToDto()).ToArray();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la récupération des notes");
            return [];
        }
    }
    
    public async Task<PatientNoteMinimalDto[]> GetAllMinimalByPatientIdAsync(int patientId)
    {
        try
        {
            var datas =  await _patientNotesCollection.Find(x => x.PatientId == patientId).ToListAsync();
            return datas.Select(s => s.ConvertToMinimalDto()).ToArray();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la récupération des notes");
            return [];
        }
    }

    public async Task<PatientNoteDto?> GetAsync(string id)
    {
        try
        {
            var data = await _patientNotesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (data == null)
            {
                _logger.LogWarning("La note n°{id} n'a pas été trouvée.", id);
                return null;
            }

            return data.ConvertToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la récupération de la note");
            return null;
        }
    }

    public async Task<PatientNoteDto?> CreateAsync(PatientNoteViewModel data)
    {
        if (string.IsNullOrEmpty(data.PatientName) || string.IsNullOrWhiteSpace(data.PatientName))
        {
            _logger.LogError("Le nom du patient ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return null;
        }
        
        if (string.IsNullOrEmpty(data.Note) || string.IsNullOrWhiteSpace(data.Note))
        {
            _logger.LogError("La note concernant le patient ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return null;
        }
        
        try
        {
            var entity = data.ConvertToEntity();
            entity.Id = null;

            await _patientNotesCollection.InsertOneAsync(entity);
            data.Id = entity.Id;
            
            _logger.LogInformation("La note n°{id} a été créée avec succès", entity.Id);
            return data.ConvertToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la création de la note");
            return null;
        }
    }
    
    public async Task<PatientNoteDto[]> CreateRangeAsync(PatientNoteViewModel[] datas)
    {
        if (datas.Length == 0)
        {
            _logger.LogError("Le tableau de données est vide.");
            return [];
        }
        if (datas.Any(a => string.IsNullOrEmpty(a.PatientName) || string.IsNullOrWhiteSpace(a.PatientName)))
        {
            _logger.LogError("Le nom du patient ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return [];
        }
        
        if (datas.Any(a => string.IsNullOrEmpty(a.Note) || string.IsNullOrWhiteSpace(a.Note)))
        {
            _logger.LogError("La note concernant le patient ne peut pas être null, vide ou ne contenir que des espaces blancs.");
            return [];
        }
        
        try
        {
            var entities = datas.Select(s => s.ConvertToEntity()).ToArray();

            await _patientNotesCollection.InsertManyAsync(entities);
            
            _logger.LogInformation("Les notes patients ont été créées avec succès");
            return entities.Select(s => s.ConvertToDto()).ToArray();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la création des notes patients");
            return [];
        }
    }

    public async Task<PatientNoteDto?> UpdateAsync(string id, PatientNoteViewModel data)
    {
        try
        {
            var equalIdFilter = Builders<PatientNoteEntity>.Filter.Eq(nameof(PatientNoteEntity.Id), id);
            if (!await _patientNotesCollection.Find(equalIdFilter).AnyAsync())
            {
                _logger.LogWarning("La note n°{id} n'a pas été trouvée.", id);
                return null;
            }
            
            var entity = data.ConvertToEntity();

            var result = await _patientNotesCollection.ReplaceOneAsync(x => x.Id == id, entity);
            
            if (result.MatchedCount == 0)
            {
                _logger.LogWarning("La note n°{id} n'a pas été trouvée.", id);
                return null;
            }
            
            _logger.LogInformation("La note n°{id} a été mise à jour avec succès", id);
            return data.ConvertToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la mise à jour de la note");
            return null;
        }
    }

    public async Task<bool> RemoveAsync(string id)
    {
        try
        {
            var equalIdFilter = Builders<PatientNoteEntity>.Filter.Eq(nameof(PatientNoteEntity.Id), id);
            if (!await _patientNotesCollection.Find(equalIdFilter).AnyAsync())
            {
                _logger.LogWarning("La note n°{id} n'a pas été trouvée.", id);
                return false;
            }
            
            var result = await _patientNotesCollection.DeleteOneAsync(x => x.Id == id);
            
            if (result.DeletedCount == 0)
            {
                _logger.LogWarning("La note n°{id} n'a pas été trouvée.", id);
                return false;
            }
            
            _logger.LogInformation("La note n°{id} a été supprimée avec succès", id);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Une erreur est survenue lors de la suppression de la note");
            return false;
        }
    }
}