using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackPatient.NoteHistory.WebApi.Models.Entities;

public class PatientNoteEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("patId")]
    public int PatientId { get; set; }
    
    [BsonElement("patient")]
    public string PatientName { get; set; } = null!;

    [BsonElement("note")]
    public string Note { get; set; } = null!;
}