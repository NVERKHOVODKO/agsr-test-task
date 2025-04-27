using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient.API.Models;

public class PatientName
{
    /// <summary>
    /// Уникальный идентификатор имени пациента.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Тип использования имени (например, официальное имя).
    /// </summary>
    [MaxLength(50)]
    public string? Use { get; set; }

    /// <summary>
    /// Фамилия пациента.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Family { get; set; } = null!;

    /// <summary>
    /// Идентификатор пациента.
    /// </summary>
    [Required]
    public Guid PatientId { get; set; }

    /// <summary>
    /// Навигационное свойство на пациента.
    /// </summary>
    [ForeignKey(nameof(PatientId))]
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Список собственных имен пациента.
    /// </summary>
    public List<PatientGivenName> Given { get; set; } = new();
}