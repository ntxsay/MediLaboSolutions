using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Services.Interfaces;

namespace FrontPatient.AspNetCore.Services.Implementations;

public class PatientNoteSeedServices(ILogger<PatientNoteSeedServices> logger, IPatientServices patientServices, IPatientNoteServices patientNoteServices) : IPatientNoteSeedServices
{
    public async Task SeedNotesAsync()
    {
        var allPatientDictionary = (await patientServices.GetAllAsync())
            .ToDictionary(k => k.LastName, v => v.Id);
        if (allPatientDictionary.Count > 0)
        {
            var listModel = new List<PatientNoteViewModel>();
            foreach (var patient in _seedDictionary)
            {
                if (allPatientDictionary.TryGetValue(patient.Key, out var patientId))
                {
                    listModel.AddRange(patient.Value.Select(s => new PatientNoteViewModel()
                    {
                        Id = null,
                        Note = s,
                        PatientId = patientId,
                        PatientName =  patient.Key
                    }));
                }
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
                logger.LogWarning("Aucune correspondance trouvée pour l'insertion des données de seed.");
            }
        }
    }

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