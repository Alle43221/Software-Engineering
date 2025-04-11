using BugTracker.Domain;
using System;
using System.Data.Entity;

namespace BugTracker.Repository
{
    // Inherit from DbContext for EF6
    internal class BugTrackerORMContext : DbContext
    {
        // Define DbSets for each entity type
        public DbSet<User> Users { get; set; }
        public DbSet<Bug> Bugs { get; set; }

        // Constructor to pass the connection string to the DbContext
        public BugTrackerORMContext(string connectionString)
            : base(connectionString)
        {
        }

        public override int SaveChanges()
        {
            
            return base.SaveChanges();
        }

        // Use the OnModelCreating method to configure the model
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
