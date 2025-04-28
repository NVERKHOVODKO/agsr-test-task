using Patient.API.Enums;

namespace Patient.API.DTOs;

/// <summary>
/// Patient data transfer object for API responses
/// </summary>
public class GetPatientDto
{
    /// <summary>
    /// Unique patient identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Patient's name information
    /// </summary>
    public PatientNameDto Name { get; set; } = null!;

    /// <summary>
    /// Patient's gender
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Patient's birth date
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Indicates if the patient is active
    /// </summary>
    public bool Active { get; set; }
}