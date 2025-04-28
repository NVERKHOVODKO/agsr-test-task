using System.ComponentModel.DataAnnotations;

namespace Patient.API.DTOs;

/// <summary>
/// DTO для обновления данных пациента
/// </summary>
public class UpdatePatientDto : CreatePatientDto
{
    /// <summary>
    /// Идентификатор пациента
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}