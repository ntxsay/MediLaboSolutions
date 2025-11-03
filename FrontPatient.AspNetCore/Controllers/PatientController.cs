using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FrontPatient.AspNetCore.Models;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace FrontPatient.AspNetCore.Controllers;

[Authorize]
public class PatientController(ILogger<PatientController> logger, IPatientServices patientServices, IPatientNoteServices patientNoteServices) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var patients = await patientServices.GetAllAsync();
        return View(patients);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = await patientServices.CreateEmptyAsync();
        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatientViewModel value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour du patient ne sont pas valides.");
            var viewModel = await patientServices.CreateEmptyAsync();
            return View(viewModel);
        }
           
        var data = await patientServices.CreateAsync(value);
        if (data == null)
        {
            logger.LogWarning("Le patient n'a pas été créé.");
            var viewModel = await patientServices.CreateEmptyAsync();
            return View(viewModel);
        }

        logger.LogInformation("Le patient {FirstName} {LastName} a été créé avec succès", data.FirstName, data.LastName);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null)
        {
            logger.LogWarning("L'ID du patient à éditer n'est pas valide.");
            return NotFound();
        }

        var viewModel = await patientServices.UpdateAsync(id.Value);
        if (viewModel == null)
        {
            logger.LogWarning("Le patient avec l'ID {Id} n'a pas été trouvé pour l'édition.", id);
            return NotFound();
        }

        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, PatientViewModel value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour du patient ne sont pas valides.");
            return BadRequest();
        }
           
        var data = await patientServices.UpdateAsync(id, value);
        if (data == null)
            return BadRequest();

        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet("{patientId:int}")]
    public async Task<IActionResult> Observations(int patientId)
    {
        var patient = await patientServices.DetailAsync(patientId);
        if (patient == null)
        {
            logger.LogWarning("Le patient n°{patientId} n'a pas été trouvé.", patientId);
            return NotFound();
        }
        
        var notes = await patientNoteServices.GetAllByPatientIdAsync(patientId);
        
        ViewBag.PatientId = patientId;
        ViewBag.PatientName = patient.FirstName + " " + patient.LastName;
        return View(notes);
    }
    
    [HttpGet("Create-Observation/{patientId:int}")]
    public async Task<IActionResult> CreateObservation(int patientId)
    {
        var patient = await patientServices.DetailAsync(patientId);
        if (patient == null)
        {
            logger.LogWarning("Le patient n°{patientId} n'a pas été trouvé.", patientId);
            return NotFound();
        }
        
        ViewBag.PatientId = patientId;
        ViewBag.PatientName = patient.FirstName + " " + patient.LastName;
        return View(new PatientNoteViewModel()
        {
            PatientId = patientId,
            PatientName = patient.FirstName + " " + patient.LastName
        });
    }
    
    [HttpPost("Create-Observation/{patientId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateObservation(int patientId, PatientNoteViewModel value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création de la note du patient n°{patientId} ne sont pas valides.", patientId);
            var patient = await patientServices.DetailAsync(patientId);
            if (patient == null)
            {
                logger.LogWarning("Le patient n°{patientId} n'a pas été trouvé.", patientId);
                return NotFound();
            }
        
            ViewBag.PatientId = patientId;
            ViewBag.PatientName = patient.FirstName + " " + patient.LastName;
            return View(new PatientNoteViewModel()
            {
                PatientId = patientId,
                PatientName = patient.FirstName + " " + patient.LastName
            });
        }
           
        value.PatientId = patientId;
        var data = await patientNoteServices.CreateAsync(value);
        if (data == null)
        {
            logger.LogWarning("Le patient n'a pas été créé.");
            var patient = await patientServices.DetailAsync(patientId);
            if (patient == null)
            {
                logger.LogWarning("Le patient n°{patientId} n'a pas été trouvé.", patientId);
                return NotFound();
            }
        
            ViewBag.PatientId = patientId;
            ViewBag.PatientName = patient.FirstName + " " + patient.LastName;
            return View(new PatientNoteViewModel()
            {
                PatientId = patientId,
                PatientName = patient.FirstName + " " + patient.LastName
            });
        }

        logger.LogInformation("La note du patient n°{patientId} a été créée avec succès", patientId);
        return RedirectToAction(nameof(Observations), new { patientId });
    }
    

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}