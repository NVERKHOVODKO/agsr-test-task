using Microsoft.AspNetCore.Mvc;
using Patient.Core.DTOs;
using Patient.Core.Enums;
using Patient.Core.Services.Interfaces;

namespace Patient.API.Controllers;

/// <summary>
/// Controller for managing patient-related operations.
/// Provides endpoints for creating, retrieving, updating, and deleting patient records.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientsController"/> class.
    /// </summary>
    /// <param name="patientService">The patient service for handling business logic.</param>
    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Retrieves all patient records.
    /// </summary>
    /// <returns>A list of all patients.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPatientDto>>> Get()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }
    
    /// <summary>
    /// Retrieves a specific patient record by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <returns>The patient record if found; otherwise, returns NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetPatientDto>> Find(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return patient is null ? Problem(statusCode: StatusCodes.Status404NotFound, detail: "Patient not found") : Ok(patient);
    }

    /// <summary>
    /// Updates an existing patient record.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to update.</param>
    /// <param name="patient">The updated patient data.</param>
    /// <returns>Ok if the update was successful.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdatePatientDto patient)
    {
        var result = await _patientService.UpdateAsync(id, patient);
        return result switch
        {
            Status.Success => Ok(),
            Status.NotFound => Problem(statusCode: StatusCodes.Status404NotFound, detail: "Patient not found"),
            _ => Ok(result)
        };
    }

    /// <summary>
    /// Creates a new patient record.
    /// </summary>
    /// <param name="patient">The patient data to create.</param>
    /// <returns>The newly created patient record.</returns>
    [HttpPost]
    public async Task<ActionResult<GetPatientDto>> Create(CreatePatientDto patient)
    {
        var createdPatient = await _patientService.CreateAsync(patient);
        return Ok(createdPatient);
    }

    /// <summary>
    /// Deletes a specific patient record by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to delete.</param>
    /// <returns>NoContent if the deletion was successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }
    
    /// <summary>
    /// Searches patients by birthdate using FHIR date search parameters
    /// </summary>
    /// <param name="date" example="eq2013-01-14">FHIR date search parameter with optional prefix:
    /// <br/>- eq = equal (default)
    /// <br/>- ne = not equal
    /// <br/>- lt = less than
    /// <br/>- gt = greater than
    /// <br/>- le = less or equal
    /// <br/>- ge = greater or equal
    /// <br/>- sa = starts after
    /// <br/>- eb = ends before
    /// <br/>- ap = approximately
    /// </param>
    /// <returns>List of patients matching the date criteria</returns>
    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<GetPatientDto>>> Search([FromQuery] string date)
    {
        var patients = await _patientService.SearchByBirthDateAsync(date);
        return Ok(patients);
    }
}