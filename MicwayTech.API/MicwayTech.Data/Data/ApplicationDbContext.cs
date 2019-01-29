using Microsoft.EntityFrameworkCore;
using MicwayTech.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicwayTech.Data
{
    /// <summary>
    /// Initializing DbContext for application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Constructor
        /// <summary>
        /// empty contructor
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        #endregion

        #region DbSets
        /// <summary>
        /// Drivers Db set
        /// </summary>
        public DbSet<Driver> Drivers { get; set; }
        #endregion

        #region Configuration Overrides 
        /// <summary>
        /// Extra configuration for DBcontext not in use as declaring options via startup class
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MicWayDatabase;Trusted_Connection=True;");
        }

        /// <summary>
        /// Fluid API to configure the DB table paramters
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Naming Table
            modelBuilder.Entity<Driver>()
            .ToTable("DriverDetails");

            //Assigning Primary Key
            modelBuilder.Entity<Driver>()
                .HasKey(d => d.Id);

            //Removing Deleted rows from selection
            modelBuilder.Entity<Driver>()
                .HasQueryFilter(d => !d.IsDeleted);

            //Configuring column Id
            modelBuilder.Entity<Driver>().Property(d => d.Id)
            .HasColumnName("id");

            //Configuring column First Name
            modelBuilder.Entity<Driver>().Property(d => d.FirstName)
           .HasColumnName("firstName")
           .HasColumnType("varchar(50)")
           .IsRequired();

            //Configuring column Last Name
            modelBuilder.Entity<Driver>().Property(d => d.LastName)
           .HasColumnName("lastName")
           .HasColumnType("varchar(50)")
           .IsRequired();

            //Configuring column Date of Birth
            modelBuilder.Entity<Driver>().Property(d => d.DOB)
           .HasColumnName("dob")
           .IsRequired();

            //Configuring column Email
            modelBuilder.Entity<Driver>().Property(d => d.Email)
           .HasColumnName("email")
           .HasColumnType("varchar(100)")
           .IsRequired();

            //Configuring column Deleted tag
            modelBuilder.Entity<Driver>().Property(d => d.IsDeleted)
            .HasColumnName("isDeleted")
            .HasDefaultValueSql<bool>("0");


            //Initial Data sample data
            modelBuilder.Entity<Driver>()
                .HasData(new Driver { Id = Guid.NewGuid().ToString(), FirstName = "John", LastName = "Mack", DOB = new DateTime(1900, 06, 11), Email = "John.Mack@micway.com", IsDeleted = false },
                new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Hubert", LastName = "DAF", DOB = new DateTime(1928, 01, 01), Email = "Hubert.DAF@micway.com", IsDeleted = false },
                new Driver { Id = Guid.NewGuid().ToString(), FirstName = "George", LastName = "Kenworth", DOB = new DateTime(1900, 06, 11), Email = "George.Kenworth@micway.com", IsDeleted = false },
                new Driver { Id = Guid.NewGuid().ToString(), FirstName = "Benjamin", LastName = "Caterpillar", DOB = new DateTime(1900, 06, 11), Email = "Benjamin.Caterpillar@micway.com", IsDeleted = false });


        }

        //overriding EF SaveChanges method, adding soft delete
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        //overriding EF SaveChangesAsync method, adding soft delete
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        //adding soft delete method
        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries<Driver>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
        #endregion
    }
}
