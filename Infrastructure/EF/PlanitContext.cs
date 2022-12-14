using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class PlanitContext : DbContext
{
    private readonly IConnectionStringProvider _connectionStringProvider;

    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Address> Address { get; set; }

    public DbSet<Function> Functions { get; set; }
    
    public DbSet<Domain.Companies> Companies { get; set; }
    
    public DbSet<Domain.Has> Has { get; set; }
    
    public DbSet<Domain.Events> Events { get; set; }
    
    public DbSet<EventTypes> EventTypes { get; set; }
    
    public DbSet<Announcements> Announcements { get; set; }
    
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
            entity.Property(arg => arg.Phone).HasColumnName("phone");
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
                .HasPrincipalKey<Account>(arg => arg.IdAccount);
            entity.HasOne(arg => arg.Function)
                .WithOne()
                .HasForeignKey<Domain.Has>(arg => arg.IdFunctions)
                .HasPrincipalKey<Function>(arg => arg.IdFunctions);
        });
        modelBuilder.Entity<EventTypes>(entity =>
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
                .HasForeignKey<Domain.Events>(arg => arg.Types)
                .HasPrincipalKey<EventTypes>(arg => arg.Types);
        });
        modelBuilder.Entity<Domain.Address>(entity =>
        {
            entity.ToTable("Address");
            entity.HasKey(arg => arg.IdAddress);
            entity.Property(arg => arg.IdAddress).HasColumnName("idAddress");
            entity.Property(arg => arg.Street).HasColumnName("street");
            entity.Property(arg => arg.Number).HasColumnName("number");
            entity.Property(arg => arg.PostCode).HasColumnName("postCode");
            entity.Property(arg => arg.City).HasColumnName("city");
        });
        modelBuilder.Entity<Announcements>(entity =>
        {
            entity.ToTable("Announcements");
            entity.HasKey(arg => arg.IdAnnouncements);
            entity.Property(arg => arg.IdAnnouncements).HasColumnName("idAnnouncements");
            entity.Property(arg => arg.IdCompanies).HasColumnName("idCompanies");
            entity.Property(arg => arg.IdFunctions).HasColumnName("idFunctions");
            entity.Property(arg => arg.Content).HasColumnName("content");
        });
    }
}