using Microsoft.EntityFrameworkCore;
using Patient.API.Models;

namespace Patient.API.DataBase;

/// <summary>
/// Контекст базы данных для работы с пациентами.
/// </summary>
public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) 
        : base(options)
    {
    }

    /// <summary>
    /// Пациенты.
    /// </summary>
    public DbSet<Models.Patient> Patients { get; set; } = null!;

    /// <summary>
    /// Имена пациентов.
    /// </summary>
    public DbSet<PatientName> PatientNames { get; set; } = null!;

    /// <summary>
    /// Собственные имена пациентов.
    /// </summary>
    public DbSet<PatientGivenName> PatientGivenNames { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связи "Пациент 1-1 Имя"
        modelBuilder.Entity<Models.Patient>()
            .HasOne(p => p.Name)
            .WithOne(n => n.Patient)
            .HasForeignKey<PatientName>(n => n.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка связи "Имя 1-Н Собственные имена"
        modelBuilder.Entity<PatientName>()
            .HasMany(n => n.Given)
            .WithOne(g => g.PatientName)
            .HasForeignKey(g => g.PatientNameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}