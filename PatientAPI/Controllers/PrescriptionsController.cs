using Microsoft.AspNetCore.Mvc;
using PatientAPI.DTOs;
using PatientAPI.Services;

namespace PatientAPI.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionCreateDto dto)
    {
        try
        {
            var created = await _service.CreatePrescriptionAsync(dto);
            return CreatedAtAction(nameof(CreatePrescription), new { id = created.IdPrescription }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}