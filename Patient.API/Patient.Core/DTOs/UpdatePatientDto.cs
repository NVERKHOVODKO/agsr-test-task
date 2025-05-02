using System.ComponentModel.DataAnnotations;
using Patient.Core.Services.Patient.Enums;

namespace Patient.Core.Patient;

/// <summary>
/// Data transfer object for updating an existing patient record.
/// </summary>
public class UpdatePatientDto
{
    /// <summary>
    /// Complete name information of the patient.
    /// </summary>
    [Required]
    public PatientNameDto Name { get; set; } = new();

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