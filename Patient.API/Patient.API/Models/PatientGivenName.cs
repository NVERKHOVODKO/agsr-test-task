using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient.API.Models;

/// <summary>
/// Собственное имя пациента.
/// </summary>
public class PatientGivenName
{
    /// <summary>
    /// Уникальный идентификатор собственного имени.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Значение собственного имени.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Value { get; set; } = null!;

    /// <summary>
    /// Идентификатор имени пациента.
    /// </summary>
    [Required]
    public Guid PatientNameId { get; set; }

    /// <summary>
    /// Навигационное свойство на имя пациента.
    /// </summary>
    [ForeignKey(nameof(PatientNameId))]
    public PatientName PatientName { get; set; } = null!;
}