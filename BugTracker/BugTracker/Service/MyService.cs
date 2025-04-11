using System;
using System.Collections.Generic;
using BugTracker.Domain;
using BugTracker.Repository;

namespace BugTracker.Service
{
        public class MyService
        {
            private readonly IBugRepository bugRepository;
            private readonly IUserRepository userRepository;

            public MyService(IBugRepository bugRepository, IUserRepository userRepository)
            {
                this.bugRepository = bugRepository;
                this.userRepository = userRepository;
            }

            // Fetch all bugs
            public IEnumerable<Bug> GetAllBugs()
            {
                return bugRepository.FindAll();
            }

            // Authenticate a user and set the session.
            public User AuthenticateUser(string username, string password)
            {
                foreach (var user in userRepository.FindAll())
                {
                    if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase) &&
                        user.Password == password)
                    {
                        // Store the user id and username in the session.
                        UserSession.Login(user.Id.ToString(), user.Username);
                        return user;
                    }
                }
                return null;
        }

            // Create a new user.
            public User CreateNewUser(string name, string username, string password, Role role)
            {
                var user = new User
                {
                    Name = name,
                    Username = username,
                    Password = password, // In production, store a hashed password instead.
                    Role = role
                };

                var result = userRepository.Save(user);
                if (result != null)
                {
                    throw new InvalidOperationException("User creation failed or user already exists.");
                }

                return user;
            }

            // Create a new bug.
            public Bug CreateNewBug(string title, string description)
            {
                // Ensure the user is logged in before creating a bug.
                if (string.IsNullOrEmpty(UserSession.Username))
                    throw new UnauthorizedAccessException("User must be logged in to create a bug.");

                var bug = new Bug
                {
                    Title = title,
                    Description = description,
                    // CreatedAt and default Status (Open) are set in the Bug constructor.
                };

                var result = bugRepository.Save(bug);
                if (result != null)
                {
                    throw new InvalidOperationException("Bug creation failed or bug already exists.");
                }

                return bug;
            }

            // Set a bug as closed.
            public Bug CloseBug(int bugId)
            {
                // Ensure the user is logged in before closing a bug.
                if (string.IsNullOrEmpty(UserSession.Username))
                    throw new UnauthorizedAccessException("User must be logged in to close a bug.");

                var bug = bugRepository.FindOne(bugId);
                if (bug == null)
                {
                    throw new ArgumentException("Bug not found.");
                }

                bug.Status = BugStatus.Closed;
                var updateResult = bugRepository.Update(bug);
                if (updateResult != null)
                {
                    throw new InvalidOperationException("Failed to update the bug.");
                }

                return bug;
            }

            public User? GetUserByUsername(string username)
            {
                return userRepository.FindByUsername(username);
            }

        public void LogOut()
        {
            // Clear user session (assuming UserSession has a Logout method to clear the session)
            UserSession.Logout();
        }
    }
    }

