using System.ComponentModel.DataAnnotations;

namespace Patient.Core.DTOs;

/// <summary>
/// Data transfer object for updating an existing patient record.
/// </summary>
public class UpdatePatientDto : CreatePatientDto
{
    /// <summary>
    /// Unique identifier of the patient to be updated.
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}