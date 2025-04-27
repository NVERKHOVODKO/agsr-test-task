using System.ComponentModel.DataAnnotations;

namespace Patient.API.DTOs;

// <summary>
/// DTO для имени пациента
/// </summary>
public class PatientNameDto
{
    /// <summary>
    /// Тип использования имени (например, официальное имя)
    /// </summary>
    [MaxLength(50)]
    public string? Use { get; set; }

    /// <summary>
    /// Фамилия пациента
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Family { get; set; }

    /// <summary>
    /// Список собственных имен пациента
    /// </summary>
    [MinLength(1, ErrorMessage = "At least one given name is required")]
    public List<string> Given { get; set; } = new();
}