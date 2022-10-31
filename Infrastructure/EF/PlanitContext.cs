using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class PlanitContext : DbContext
{
    private readonly IConnectionStringProvider _connectionStringProvider;

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Function> Functions { get; set; }

    public PlanitContext(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionStringProvider.GetConnectionString("Db"));
        }
    }

    // Fluent Api
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Account table
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Accounts"); // Accounts tables
            entity.HasKey(arg => arg.Id); // Primary key
            entity.Property(arg => arg.Id).HasColumnName("idAccount"); 
            entity.Property(arg => arg.Email).HasColumnName("email"); 
            entity.Property(arg => arg.PasswordHash).HasColumnName("passwordHash"); 
            entity.Property(arg => arg.FirstName).HasColumnName("lastName");
            entity.Property(arg => arg.LastName).HasColumnName("firstName");
            entity.Property(arg => arg.Street).HasColumnName("street");
            entity.Property(arg => arg.Number).HasColumnName("number");
            entity.Property(arg => arg.PostCode).HasColumnName("postCode");
            entity.Property(arg => arg.City).HasColumnName("city");
            entity.Property(arg => arg.IsChief).HasColumnName("isChief");
            entity.Property(arg => arg.PictureURL).HasColumnName("pictureURL");
            entity.Property(arg => arg.idFunction).HasColumnName("idFunction");
        });
        // Function table
        modelBuilder.Entity<Function>(entity =>
        {
            entity.ToTable("Functions"); // Accounts tables
            entity.HasKey(arg => arg.Id); // Primary key
            entity.Property(arg => arg.Id).HasColumnName("idFunction");
            entity.Property(arg => arg.Title).HasColumnName("title");
        });
    }
}