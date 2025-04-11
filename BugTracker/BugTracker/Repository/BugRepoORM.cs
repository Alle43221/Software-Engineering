using BugTracker.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BugTracker.Repository
{
    public class BugRepoORM : IBugRepository
    {
        private static readonly ILog _log = LogManager.GetLogger("BugRepoORM");
        private readonly BugTrackerORMContext context;

        // Constructor accepts props dictionary to get the connection string
        public BugRepoORM(IDictionary<string, string> props)
        {
            _log.Info("Creating BugRepoORM");

            // Get the connection string from props
            string connectionString = props.ContainsKey("ConnectionString") ? props["ConnectionString"] : string.Empty;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string is missing in the provided properties.");
            }

            // Initialize context with connection string
            context = new BugTrackerORMContext(connectionString);
        }

        public Bug? FindOne(int id)
        {
            _log.Info("Finding a bug...");
            // Use LINQ to fetch a bug by ID
            var bug = context.Bugs.FirstOrDefault(b => b.Id == id);
            if (bug != null)
            {
                _log.Info("Bug found");
                // Convert the Status string to BugStatus enum
            }
            return bug;
        }

        public IEnumerable<Bug> FindAll()
        {
            _log.Info("Fetching all bugs...");
            // Use LINQ to get all bugs from the database
            var bugs = context.Bugs.ToList();
            foreach (var bug in bugs)
            {
                // Convert the Status string to BugStatus enum for each bug
            }
            return bugs;
        }

        public Bug? Save(Bug entity)
        {
            _log.Info("Saving a new bug...");

            // Check if the bug already exists
            if (FindOne(entity.Id) != null)
            {
                _log.Info("Bug already exists, skipping save.");
                return entity;
            }

            // Add the new bug entity to the Bugs DbSet
            context.Bugs.Add(entity);
            context.SaveChanges();
            _log.Info("Bug saved successfully.");

            return entity; // Return the entity with populated ID
        }

        public Bug? Delete(int id)
        {
            _log.Info("Deleting a bug...");
            var bug = FindOne(id);
            if (bug == null)
            {
                _log.Warn("Bug not found, skipping delete.");
                return null;
            }

            context.Bugs.Remove(bug); // Remove the bug from DbSet
            context.SaveChanges(); // Save changes to the database
            _log.Info("Bug deleted successfully.");
            return bug;
        }

        public Bug? Update(Bug entity)
        {
            _log.Info("Updating a bug...");
            var existingBug = FindOne(entity.Id);
            if (existingBug == null)
            {
                _log.Warn("Bug not found, skipping update.");
                return entity; // If the bug doesn't exist, return the original entity
            }

            // Update properties of the existing bug
            existingBug.Title = entity.Title;
            existingBug.Description = entity.Description;
            existingBug.CreatedAt = entity.CreatedAt;
            existingBug.Status = entity.Status;

            context.SaveChanges(); // Save changes to the database
            _log.Info("Bug updated successfully.");
            return existingBug;
        }
    }
}
