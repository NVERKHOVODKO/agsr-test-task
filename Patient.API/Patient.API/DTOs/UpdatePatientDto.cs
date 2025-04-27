using System.ComponentModel.DataAnnotations;
using Patient.API.Enums;

namespace Patient.API.DTOs;

/// <summary>
/// DTO для обновления данных пациента
/// </summary>
public class UpdatePatientDto
{
    /// <summary>
    /// Идентификатор пациента
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пациента
    /// </summary>
    [Required]
    public PatientNameDto Name { get; set; }

    /// <summary>
    /// Пол пациента
    /// </summary>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Дата рождения пациента
    /// </summary>
    [Required]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Статус активности пациента
    /// </summary>
    public bool Active { get; set; } = true;
}