using System.ComponentModel.DataAnnotations;
using Patient.Core.Enums;

namespace Patient.Core.DTOs;

/// <summary>
/// Data transfer object for creating a new patient record.
/// </summary>
public class CreatePatientDto
{
    /// <summary>
    /// Complete name information of the patient.
    /// </summary>
    [Required]
    public PatientNameDto? Name { get; set; }

    /// <summary>
    /// Biological gender of the patient.
    /// </summary>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Date when the patient was born.
    /// </summary>
    [Required]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Indicates if the patient record is currently active in the system.
    /// </summary>
    public bool Active { get; set; } = true;
}