using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Services.Interfaces;

namespace FrontPatient.AspNetCore.Services.Implementations;

/// <summary>
/// Service permettant de peupler la base de données de notes de patients
/// </summary>
/// <param name="logger"></param>
/// <param name="patientNoteServices"></param>
public class PatientNoteSeedServices(ILogger<PatientNoteSeedServices> logger, IPatientNoteServices patientNoteServices) : IPatientNoteSeedServices
{
    public async Task SeedNotesAsync(Dictionary<string, int> patientLastNameIdDictionary)
    {
        if (patientLastNameIdDictionary.Count > 0)
        {
            var currentMinimalNotes = await patientNoteServices.GetMinimalNotesByPatientIds(patientLastNameIdDictionary.Values.ToArray());
            var listModel = new List<PatientNoteViewModel>();
            foreach (var seedPatientNameNotes in _seedDictionary)
            {
                if (!patientLastNameIdDictionary.TryGetValue(seedPatientNameNotes.Key, out var patientId)) 
                    continue;
                
                //Retourne les notes existantes du patient, les comparent avec les notes à insérer et ajoute les notes manquantes
                var currentNotes = currentMinimalNotes.Where(w => w.PatientId == patientId)
                    .Select(s => s.Note)
                    .ToArray();
                
                if (currentNotes.Length > 0)
                {
                    var notExitingNoteData = seedPatientNameNotes.Value
                        .Except(currentNotes, StringComparer.OrdinalIgnoreCase).Select(s => new PatientNoteViewModel()
                        {
                            Id = null,
                            Note = s,
                            PatientId = patientId,
                            PatientName =  seedPatientNameNotes.Key
                        }).ToArray();

                    if (notExitingNoteData.Length > 0)
                        listModel.AddRange(notExitingNoteData);
                    else
                        logger.LogInformation("Aucune note manquante pour le patient {patientName}",
                            seedPatientNameNotes.Key);

                    continue;
                }

                listModel.AddRange(seedPatientNameNotes.Value.Select(s => new PatientNoteViewModel()
                {
                    Id = null,
                    Note = s,
                    PatientId = patientId,
                    PatientName =  seedPatientNameNotes.Key
                }));
            }
            
            if (listModel.Count > 0)
            {
                var datas = await  patientNoteServices.CreateRangeAsync(listModel.ToArray());
                if (datas.Length > 0)
                    logger.LogInformation("{countCreated} note(s) de patients créées",  datas.Length);
                else
                    logger.LogWarning("0 note de patient créée.");
            }
            else
            {
                logger.LogInformation("Aucune correspondance trouvée pour l'insertion des données de seed.");
            }
        }
    }
    

    /// <summary>
    /// Dictionnaire contenant les notes de patients à insérer. La clé est le nom du patient et la valeur est le tableau de notes.
    /// </summary>
    private readonly Dictionary<string, string[]> _seedDictionary =
        new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "TestNone", ["Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé"]
            },
            {
                "TestBorderline",
                [
                    "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement",
                    "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale"
                ]
            },
            {
                "TestInDanger",
                [
                    "Le patient déclare qu'il fume depuis peu",
                    "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d’apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé"
                ]
            },
            {
                "TestEarlyOnset",
                [
                    "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d’être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments",
                    "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps",
                    "Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé",
                    "Taille, Poids, Cholestérol, Vertige et Réaction"
                ]
            }
        };
}