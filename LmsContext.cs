using Microsoft.EntityFrameworkCore;

namespace LMS;

public partial class LmsContext : DbContext
{
    public LmsContext()
    {
    }

    public LmsContext(DbContextOptions<LmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<IssuedBook> IssuedBooks { get; set; }

    public virtual DbSet<RequestBook> RequestBooks { get; set; }

    public virtual DbSet<User> Users { get; set; }
    // public virtual DbSet<Audit> Audit { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Iban).HasName("PK__tmp_ms_x__8235CCBD1D95768C");

            entity.ToTable("Book");

            entity.Property(e => e.Iban)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IBAN");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<IssuedBook>(entity =>
        {
            entity.HasKey(e => new { e.Iban, e.UserId }).HasName("pk_issuedBooks");

            entity.Property(e => e.Iban)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IBAN");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userID");
            entity.Property(e => e.IssuedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("issuedDate");
        });

        modelBuilder.Entity<RequestBook>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tmp_ms_x__CB9A1CDFE3387305");

            entity.ToTable("RequestBook");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userID");
            entity.Property(e => e.Iban)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IBAN");

            entity.HasOne(d => d.User).WithOne(p => p.RequestBook)
                .HasForeignKey<RequestBook>(d => d.UserId)
                .HasConstraintName("fk_RequestBooks");
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E19D8C7B");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Admin)
                .HasDefaultValueSql("((0))")
                .HasColumnName("admin");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public override int SaveChanges()
    {
        var changeTracker = this.ChangeTracker;

        foreach (var entry in changeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                var createdAtProperty = entry.Property("CreatedAt");
                var createdByProperty = entry.Property("CreatedBy");
                var modifiedAtProperty = entry.Property("ModifiedAt");
                var modifiedByProperty = entry.Property("ModifiedBy");
         
                if (createdAtProperty == null && createdByProperty == null)
                {
                    createdAtProperty.CurrentValue = DateTime.Now;
                    createdByProperty.CurrentValue = "admin";
                    modifiedAtProperty.CurrentValue = DateTime.Now;
                    modifiedByProperty.CurrentValue = "admin";
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                var modifiedAtProperty = entry.Property("ModifiedAt");
                var modifiedByProperty = entry.Property("ModifiedBy");

                if (modifiedAtProperty == null && modifiedByProperty == null)
                {
                    modifiedAtProperty.CurrentValue = DateTime.Now;
                    modifiedByProperty.CurrentValue = "admin";
                }

            }
        }
        return base.SaveChanges();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
