using System.ComponentModel.DataAnnotations;
using Patient.API.Enums;

namespace Patient.API.Models;

/// <summary>
/// Пациент, зарегистрированный в роддоме.
/// </summary>
public class Patient
{
    /// <summary>
    /// Уникальный идентификатор пациента.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Навигационное свойство для имени пациента.
    /// </summary>
    public PatientName Name { get; set; } = null!;

    /// <summary>
    /// Пол пациента.
    /// </summary>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Дата рождения пациента.
    /// </summary>
    [Required]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Статус активности пациента.
    /// </summary>
    [Required]
    public bool Active { get; set; }
}