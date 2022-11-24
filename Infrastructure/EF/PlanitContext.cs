using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class PlanitContext : DbContext
{
    private readonly IConnectionStringProvider _connectionStringProvider;

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Function> Functions { get; set; }
    
    public DbSet<Domain.Companies> Companies { get; set; }
    
    public DbSet<Domain.Has> has { get; set; }
    
    public DbSet<Domain.Events> Events { get; set; }
    
    public PlanitContext(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // optionsBuilder.UseSqlServer(_connectionStringProvider.GetConnectionString("DbWindows"));
            // optionsBuilder.UseSqlServer(_connectionStringProvider.GetConnectionString("DbLinuxMac"));
            optionsBuilder.UseSqlServer(_connectionStringProvider.GetConnectionString("DbRemote"));
        }
    }

    // Fluent Api
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Account table
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Accounts"); // Accounts tables
            entity.HasKey(arg => arg.IdAccount); // Primary key
            entity.Property(arg => arg.IdAccount).HasColumnName("idAccount"); 
            entity.Property(arg => arg.Email).HasColumnName("email"); 
            entity.Property(arg => arg.Password).HasColumnName("password"); 
            entity.Property(arg => arg.FirstName).HasColumnName("firstName");
            entity.Property(arg => arg.LastName).HasColumnName("lastName");
            entity.Property(arg => arg.IdAddress).HasColumnName("idAddress");
            entity.Property(arg => arg.IsAdmin).HasColumnName("isAdmin");
            entity.Property(arg => arg.PictureURL).HasColumnName("pictureUrl");
        });
        // Function table
        modelBuilder.Entity<Function>(entity =>
        {
            entity.ToTable("Functions"); // Accounts tables
            entity.HasKey(arg => arg.Title); // Primary key
            entity.Property(arg => arg.Title).HasColumnName("title");
        });
        modelBuilder.Entity<Domain.Companies>(entity =>
        {
            entity.ToTable("Companies"); 
            entity.HasKey(arg => arg.IdCompanies); // Primary key
            entity.Property(arg => arg.IdCompanies).HasColumnName("idCompanies");
            entity.Property(arg => arg.CompaniesName).HasColumnName("companiesName");
            entity.Property(arg => arg.DirectorEmail).HasColumnName("directorEmail");
        });
        modelBuilder.Entity<Domain.Has>(entity =>
        {
            entity.ToTable("Has");
            entity.HasKey(arg => arg.IdHas);
            entity.Property(arg => arg.IdHas).HasColumnName("idHas");
            entity.Property(arg => arg.IdAccount).HasColumnName("idAccount");
            entity.Property(arg => arg.IdCompanies).HasColumnName("idCompanies");
            entity.Property(arg => arg.IdFunctions).HasColumnName("idFunctions");
        });
        modelBuilder.Entity<Domain.Events>(entity =>
        {
            entity.ToTable("EventsEmployee");
            entity.HasKey(arg => arg.IdEventsEmployee);
            entity.Property(arg => arg.IdEventsEmployee).HasColumnName("idEventsEmployee");
            entity.Property(arg => arg.IdSchedule).HasColumnName("idSchedules");
            entity.Property(arg => arg.IdAccount).HasColumnName("idAccount");
            entity.Property(arg => arg.StartDate).HasColumnName("startDate");
            entity.Property(arg => arg.EndDate).HasColumnName("endDate");
            entity.Property(arg => arg.IdWork).HasColumnName("idWork");
            entity.Property(arg => arg.IdAbsents).HasColumnName("idAbsents");
            entity.Property(arg => arg.IdHolidays).HasColumnName("idHolidays");
        });
    }
}