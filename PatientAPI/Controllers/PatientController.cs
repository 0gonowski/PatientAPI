using PatientAPI.DTOs;
using PatientAPI.Exceptions;
using PatientAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace PatientAPI.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        var patient = await _patientService.GetPatientDetailsAsync(id);
        if (patient == null)
        {
            throw new NotFoundException($"Patient with ID {id} not found.");
        }
        return Ok(patient);
    }
}