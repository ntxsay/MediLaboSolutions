using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FrontPatient.AspNetCore.Models;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace FrontPatient.AspNetCore.Controllers;

[Authorize]
public class PatientController(ILogger<PatientController> logger, IPatientServices patientServices) : Controller
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