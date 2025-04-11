using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BugTracker.Domain;

namespace BugTracker.Repository
{
    public class UserDbRepository : IUserRepository
    {
        private readonly IDictionary<string, string> _props;

        public UserDbRepository(IDictionary<string, string> props)
        {
            _props = props;
        }

        public User? FindByUsername(string username)
        {
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, name, username, password, role FROM [User] WHERE username = @username";
                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = username;
                comm.Parameters.Add(paramUsername);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Username = reader.GetString(2),
                            Password = reader.GetString(3),
                            Role = (Role)Enum.Parse(typeof(Role), reader.GetString(4))
                        };
                    }
                }
            }
            return null;
        }

        public User? FindOne(int id)
        {
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, name, username, password, role FROM [User] WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Username = reader.GetString(2),
                            Password = reader.GetString(3),
                            Role = (Role)Enum.Parse(typeof(Role), reader.GetString(4))
                        };
                    }
                }
            }
            return null;
        }

        public IEnumerable<User> FindAll()
        {
            var users = new List<User>();
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, name, username, password, role FROM [User]";
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Username = reader.GetString(2),
                            Password = reader.GetString(3),
                            Role = (Role)Enum.Parse(typeof(Role), reader.GetString(4))
                        });
                    }
                }
            }
            return users;
        }

        public User? Save(User entity)
        {
            if (FindOne(entity.Id) != null)
            {
                return entity;
            }
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO [User] (name, username, password, role) VALUES (@name, @username, @password, @role); SELECT CAST(scope_identity() AS int);";

                var paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.Name;
                comm.Parameters.Add(paramName);

                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = entity.Username;
                comm.Parameters.Add(paramUsername);

                var paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = entity.Password;
                comm.Parameters.Add(paramPassword);

                var paramRole = comm.CreateParameter();
                paramRole.ParameterName = "@role";
                paramRole.Value = entity.Role.ToString();
                comm.Parameters.Add(paramRole);

                entity.Id = (int)comm.ExecuteScalar();
                return null;
            }
        }

        public User? Delete(int id)
        {
            User? user = FindOne(id);
            if (user == null) return null;

            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM [User] WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                comm.ExecuteNonQuery();
            }
            return user;
        }

        public User? Update(User entity)
        {
            if (FindOne(entity.Id) == null)
            {
                return entity;
            }
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "UPDATE [User] SET name = @name, username = @username, password = @password, role = @role WHERE id = @id";

                var paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.Name;
                comm.Parameters.Add(paramName);

                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = entity.Username;
                comm.Parameters.Add(paramUsername);

                var paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = entity.Password;
                comm.Parameters.Add(paramPassword);

                var paramRole = comm.CreateParameter();
                paramRole.ParameterName = "@role";
                paramRole.Value = entity.Role.ToString();
                comm.Parameters.Add(paramRole);

                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

                comm.ExecuteNonQuery();
            }
            return null;
        }
    }
}
