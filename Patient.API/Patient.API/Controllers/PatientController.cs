using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<Models.Patient>>> GetPatients()
    {
        var patients = await _patientService.GetAllPatientsAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Patient>> GetPatient(Guid id)
    {
        var patient = await _patientService.GetPatientByIdAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPatient(Guid id, Models.Patient patient)
    {
        await _patientService.UpdatePatientAsync(id, patient);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Models.Patient>> PostPatient(Models.Patient patient)
    {
        var createdPatient = await _patientService.CreatePatientAsync(patient);
        return CreatedAtAction("GetPatient", new { id = createdPatient.Id }, createdPatient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        await _patientService.DeletePatientAsync(id);
        return NoContent();
    }
}