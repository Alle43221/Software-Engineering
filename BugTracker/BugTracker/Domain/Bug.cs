using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Domain
{
    public enum BugStatus
    {
        Open,
        Closed
    }

    [Table("Bug")]
    public class Bug : Entity<int>

    {
        [Key]
        public override int Id { get; set; }

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
