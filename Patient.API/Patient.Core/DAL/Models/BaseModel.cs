using System.ComponentModel.DataAnnotations;

namespace Patient.Core.DAL.Models;

public abstract class BaseModel
{
    /// <summary>
    /// Unique identifier of the patient.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}