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
    public async Task<ActionResult<IEnumerable<Models.Patient>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Patient>> Get(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Models.Patient patient)
    {
        await _patientService.UpdateAsync(id, patient);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Models.Patient>> Post(Models.Patient patient)
    {
        var createdPatient = await _patientService.CreateAsync(patient);
        return CreatedAtAction("Get", new { id = createdPatient.Id }, createdPatient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }
}