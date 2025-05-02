using System.ComponentModel.DataAnnotations;
using Patient.Core.Enums;
using Patient.Core.Services.Patient.Enums;

namespace Patient.Core.DAL.Models;

/// <summary>
/// Patient registered in the hospital system.
/// </summary>
public class Patient : BaseModel
{
    /// <summary>
    /// Navigation property for patient's name information.
    /// </summary>
    public PatientName Name { get; set; } = null!;

    /// <summary>
    /// Biological gender of the patient.
    /// </summary>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Patient's date of birth.
    /// </summary>
    [Required]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Indicates whether the patient record is active.
    /// </summary>
    [Required]
    public bool Active { get; set; }
}