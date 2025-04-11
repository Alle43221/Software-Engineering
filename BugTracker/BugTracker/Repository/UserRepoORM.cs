using BugTracker.Domain;
using BugTracker.Repository;
using log4net;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

public class UserRepoORM : IUserRepository
{
    private readonly BugTrackerORMContext context;

    public UserRepoORM(IDictionary<string, string> _props)
    {

        // Get the connection string from props
        string connectionString = _props.ContainsKey("ConnectionString") ? _props["ConnectionString"] : string.Empty;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string is missing in the provided properties.");
        }

        // Initialize context with connection string
        context = new BugTrackerORMContext(connectionString);
    }

    public User? FindByUsername(string username)
    {
        var user = context.Users.FirstOrDefault(u => u.Username == username);
        return user;
    }

    public User? FindOne(int id)
    {
        var user = context.Users.Find(id);
        return user;
    }

    public IEnumerable<User> FindAll()
    {
        var users = context.Users.ToList();
        return users;
    }

    public User? Save(User entity)
    {
        context.Users.Add(entity);
        context.SaveChanges();
        return entity;
    }

    public User? Update(User entity)
    {
        context.Users.Attach(entity);
        context.Entry(entity).State = EntityState.Modified; 

        context.SaveChanges();
        return entity;
    }

    public User? Delete(int id)
    {
        var user = context.Users.Find(id);
        if (user != null)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }
        return user;
    }
}
