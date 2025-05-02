using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient.Core.DAL.Models;

/// <summary>
/// Represents a patient's complete name information.
/// </summary>
public class PatientName
{
    /// <summary>
    /// Unique identifier for the name record.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Name usage type (e.g., official, maiden, nickname).
    /// </summary>
    [MaxLength(50)]
    public string? Use { get; set; }

    /// <summary>
    /// Patient's family name (surname).
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string? Family { get; set; } = null!;

    /// <summary>
    /// Foreign key reference to the associated patient.
    /// </summary>
    [Required]
    public Guid PatientId { get; set; }

    /// <summary>
    /// Navigation property to the patient record.
    /// </summary>
    [ForeignKey(nameof(PatientId))]
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Collection of patient's given names (first and middle names).
    /// </summary>
    public List<PatientGivenName> Given { get; set; } = new();
}