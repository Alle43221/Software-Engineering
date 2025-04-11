using System;

namespace BugTracker.Domain
{
    public enum BugStatus
    {
        Open,
        Closed
    }

    public class Bug : Entity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public BugStatus Status { get; set; }

        // Parameterless constructor sets default values.
        public Bug()
        {
            CreatedAt = DateTime.Now;
            Status = BugStatus.Open;
        }

        // Constructor with parameters.
        public Bug(int id, string title, string description, DateTime createdAt, BugStatus status)
        {
            Id = id;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Status = status;
        }

        public Bug( string title, string description, DateTime createdAt, BugStatus status)
        {
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            Status = status;
        }
    }
}
