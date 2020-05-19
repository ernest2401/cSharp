using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.DAL.Entities;

namespace SmartLibrary.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class
        /// </summary>
        /// <param name="options">The options to be used by <see cref="DbContext"/></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<AvailableBook> AvailableBooks { get; set; }
        public DbSet<ReservedBook> ReservedBooks { get; set; }

        /// <summary>
        /// Customize the ASP.NET Identity model and override the defaults if needed
        /// </summary>
        /// <param name="builder">Instance <see cref="ModelBuilder"/> class</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(x => x.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.LastName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.PhoneNumber)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Birthday)
                     .IsRequired()
                     .HasColumnType("datetime");

                entity.Property(x => x.Sex)
                     .IsRequired();

                entity.Property(x => x.Status)
                     .IsRequired();
            });

            builder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(x => x.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Description)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(x => x.Author)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Genre)
                      .IsRequired();

                entity.Property(x => x.Pages)
                      .IsRequired();

                entity.Property(x => x.ImageUrl)
                      .HasMaxLength(1000);

                entity.Property(x => x.Condition)
                       .IsRequired();

                entity.Property(x => x.CreationDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.ModifyDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            builder.Entity<AvailableBook>(entity =>
            {
                entity.ToTable("AvailableBook");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(x => x.Count)
                      .IsRequired();

                entity.Property(x => x.MaxTermDays)
                      .IsRequired();

                entity.HasOne(x => x.Book)
                   .WithMany(c => c.AvailableBooks)
                   .HasForeignKey(e => e.BookId);
            });

            builder.Entity<ReservedBook>(entity =>
            {
                entity.ToTable("ReservedBook");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.HasOne(x => x.User)
                      .WithMany(c => c.ReservedBooks)
                      .HasForeignKey(e => e.UserId);
                
                entity.HasOne(x => x.AvailableBook)
                      .WithMany(c => c.ReservedBooks)
                      .HasForeignKey(e => e.AvailableBookId);

                entity.Property(x => x.ReservedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(x => x.ReturnedAt)
                      .HasColumnType("datetime");
            });
        }
    }
}
