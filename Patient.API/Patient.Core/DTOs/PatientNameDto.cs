using System.ComponentModel.DataAnnotations;

namespace Patient.Core.Patient;

/// <summary>
/// Represents a patient's complete name information.
/// </summary>
public class PatientNameDto
{
    /// <summary>
    /// Name usage type (e.g., official, maiden, nickname).
    /// </summary>
    [MaxLength(50)]
    public string Use { get; set; } = string.Empty;

    /// <summary>
    /// Family name (surname) of the patient.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Family { get; set; } = string.Empty;

    /// <summary>
    /// List of given names (first and middle names) for the patient.
    /// </summary>
    [MinLength(1, ErrorMessage = "At least one given name is required")]
    public List<string> Given { get; set; } = new();
}