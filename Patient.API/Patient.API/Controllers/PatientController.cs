using Microsoft.AspNetCore.Mvc;
using Patient.API.DTOs;
using Patient.API.Services.Interfaces;

namespace Patient.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPatientDto>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPatientDto>> Get(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, UpdatePatientDto patient)
    {
        await _patientService.UpdateAsync(id, patient);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<GetPatientDto>> Post(CreatePatientDto patient)
    {
        var createdPatient = await _patientService.CreateAsync(patient);
        return Ok(createdPatient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }
}