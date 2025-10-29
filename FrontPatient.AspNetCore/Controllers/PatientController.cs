using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FrontPatient.AspNetCore.Models;
using FrontPatient.AspNetCore.Services;
using FrontPatient.AspNetCore.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace FrontPatient.AspNetCore.Controllers;

public class PatientController(ILogger<PatientController> logger, IPatientServices patientServices) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var patients = await patientServices.GetAllAsync();
        return View(patients);
    }
    
    [Authorize]
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
    public async Task<IActionResult> Update(int id, PatientDto value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour du patient ne sont pas valides.");
            return BadRequest();
        }
           
        var isUpdated = await patientServices.UpdateAsync(id, value);
        if (!isUpdated)
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