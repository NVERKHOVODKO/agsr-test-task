using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient.Core.Models;

/// <summary>
/// Represents a given (first) name of a patient.
/// </summary>
public class PatientGivenName
{
    /// <summary>
    /// Unique identifier for the given name record.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The actual given name value (e.g., "John", "Mary").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Value { get; set; } = null!;

    /// <summary>
    /// Foreign key reference to the associated patient name.
    /// </summary>
    [Required]
    public Guid PatientNameId { get; set; }

    /// <summary>
    /// Navigation property to the full patient name record.
    /// </summary>
    [ForeignKey(nameof(PatientNameId))]
    public PatientName PatientName { get; set; } = null!;
}