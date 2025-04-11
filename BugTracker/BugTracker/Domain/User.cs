
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Domain
{
    public enum Role
    {
        Programmer,
        QualityAssuranceEngineer
    }
    [Table("User")]
    public class User : Entity<int>
    {
        [Key]
        public override int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }

        // Parameterless constructor
        public User() { }


        // Constructor with parameters
        public User(int id, string name, string username, string password, Role role)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            Role = role; 
        }

        public User( string name, string username, string password, Role role)
        {
            Name = name;
            Username = username;
            Password = password;
            Role = role;
        }

    }
}
