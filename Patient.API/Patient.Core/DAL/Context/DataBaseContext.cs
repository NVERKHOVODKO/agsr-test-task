using Microsoft.EntityFrameworkCore;
using Patient.Core.DAL.Models;

namespace Patient.Core.DAL.Context;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) 
        : base(options)
    {
    }

    public DbSet<DAL.Models.Patient> Patients { get; set; }

    public DbSet<PatientName> PatientNames { get; set; }

    public DbSet<PatientGivenName> PatientGivenNames { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DAL.Models.Patient>()
            .HasOne(p => p.Name)
            .WithOne(n => n.Patient)
            .HasForeignKey<PatientName>(n => n.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PatientName>()
            .HasMany(n => n.Given)
            .WithOne(g => g.PatientName)
            .HasForeignKey(g => g.PatientNameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}