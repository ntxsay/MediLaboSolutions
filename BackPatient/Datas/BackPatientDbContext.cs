using BackPatient.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackPatient.Datas;

public class BackPatientDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<GenreEntity> Genres { get; set; }
    public DbSet<PatientEntity> Patients { get; set; }
    
    public BackPatientDbContext(DbContextOptions<BackPatientDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Genres
        modelBuilder.Entity<GenreEntity>()
            .Property(g => g.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<GenreEntity>()
            .HasKey(g => g.Id);
            
        modelBuilder.Entity<GenreEntity>()
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(60);
            
        modelBuilder.Entity<GenreEntity>()
            .HasIndex(g => g.Name)
            .IsUnique(true);
            
        modelBuilder.Entity<GenreEntity>()
            .Property(g => g.Description)
            .HasMaxLength(255);
        
        //Patients
        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<PatientEntity>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.GenreId)
            .IsRequired();
        
        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(60);
        
        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(60);
        
        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.BirthDate)
            .IsRequired();
        
        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.PostalAddress)
            .HasMaxLength(1000);
        
        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.NoTelephone)
            .HasMaxLength(100);
        
        modelBuilder.Entity<PatientEntity>()
            .Property(p => p.GenreId)
            .IsRequired();
        
        modelBuilder.Entity<PatientEntity>()
            .HasOne(p => p.Genre)
            .WithMany(g => g.Patients)
            .HasForeignKey(p => p.GenreId);
    }
}