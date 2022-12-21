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
    
    public DbSet<Domain.EventTypes> EventTypes { get; set; }
    
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
            entity.Property(arg => arg.Password).HasColumnName("password");
        });
        modelBuilder.Entity<Domain.Has>(entity =>
        {
            entity.ToTable("Has");
            entity.HasKey(arg => arg.IdHas);
            entity.Property(arg => arg.IdHas).HasColumnName("idHas");
            entity.Property(arg => arg.IdAccount).HasColumnName("idAccount");
            entity.Property(arg => arg.IdCompanies).HasColumnName("idCompanies");
            entity.Property(arg => arg.IdFunctions).HasColumnName("idFunctions");
            entity.HasOne(arg => arg.Account)
                .WithOne()
                .HasForeignKey<Domain.Has>(arg => arg.IdAccount)
                .HasPrincipalKey<Domain.Account>(arg => arg.IdAccount);
            entity.HasOne(arg => arg.Function)
                .WithOne()
                .HasForeignKey<Domain.Has>(arg => arg.IdFunctions)
                .HasPrincipalKey<Domain.Function>(arg => arg.IdFunctions);
        });
        modelBuilder.Entity<Domain.EventTypes>(entity =>
        {
            entity.ToTable("EventTypes");
            entity.HasKey(arg => arg.Types);
            entity.Property(arg => arg.Types).HasColumnName("types");
            entity.Property(arg => arg.BarColor).HasColumnName("barColor");
        });
        modelBuilder.Entity<Domain.Events>(entity =>
        {
            entity.ToTable("EventsEmployee");
            entity.HasKey(arg => arg.IdEventsEmployee);
            entity.Property(arg => arg.IdEventsEmployee).HasColumnName("idEventsEmployee");
            entity.Property(arg => arg.IdCompanies).HasColumnName("idCompanies");
            entity.Property(arg => arg.IdAccount).HasColumnName("idAccount");
            entity.Property(arg => arg.StartDate).HasColumnName("startDate");
            entity.Property(arg => arg.EndDate).HasColumnName("endDate");
            entity.Property(arg => arg.Types).HasColumnName("types");
            entity.Property(arg => arg.IsValid).HasColumnName("isValid");
            entity.Property(arg => arg.Comments).HasColumnName("comments");
            entity.HasOne(arg => arg.EventTypes)
                .WithOne()
                .HasForeignKey<Events>(arg => arg.Types)
                .HasPrincipalKey<Domain.EventTypes>(arg => arg.Types);
        });
    }
}