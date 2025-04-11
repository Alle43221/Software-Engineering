using BugTracker.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BugTracker.Repository
{
    public class BugDbRepository : IBugRepository
    {
        private static readonly ILog _log = LogManager.GetLogger("BookingDBRepository");
        private readonly IDictionary<string, string> _props;

        public BugDbRepository(IDictionary<string, string> props)
        {
            _log.Info("Creating BugDBRepository");
            _props = props;
        }

        public Bug? FindOne(int id)
        {
            _log.Info("Finding a bug...");
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, title, description, createdAt, status FROM Bug WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _log.Info("Bug found");
                        return new Bug
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3),
                            Status = (BugStatus)Enum.Parse(typeof(BugStatus), reader.GetString(4))
                        };
                    }
                }
            }
            return null;
        }

        public IEnumerable<Bug> FindAll()
        {
            var bugs = new List<Bug>();
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, title, description, createdAt, status FROM Bug";
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bugs.Add(new Bug
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3),
                            Status = (BugStatus)Enum.Parse(typeof(BugStatus), reader.GetString(4))
                        });
                    }
                }
            }
            return bugs;
        }

        public Bug? Save(Bug entity)
        {
            if (FindOne(entity.Id) != null)
            {
                return entity;
            }
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO Bug (title, description, createdAt, status) VALUES (@title, @description, @createdAt, @status); SELECT CAST(scope_identity() AS int);";

                var paramTitle = comm.CreateParameter();
                paramTitle.ParameterName = "@title";
                paramTitle.Value = entity.Title;
                comm.Parameters.Add(paramTitle);

                var paramDesc = comm.CreateParameter();
                paramDesc.ParameterName = "@description";
                paramDesc.Value = entity.Description;
                comm.Parameters.Add(paramDesc);

                var paramCreatedAt = comm.CreateParameter();
                paramCreatedAt.ParameterName = "@createdAt";
                paramCreatedAt.Value = entity.CreatedAt;
                comm.Parameters.Add(paramCreatedAt);

                var paramStatus = comm.CreateParameter();
                paramStatus.ParameterName = "@status";
                paramStatus.Value = entity.Status.ToString();
                comm.Parameters.Add(paramStatus);

                entity.Id = (int)comm.ExecuteScalar();
                return null;
            }
        }

        public Bug? Delete(int id)
        {
            Bug? bug = FindOne(id);
            if (bug == null) return null;

            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM Bug WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                comm.ExecuteNonQuery();
            }
            return bug;
        }

        public Bug? Update(Bug entity)
        {
            if (FindOne(entity.Id) == null)
            {
                return entity;
            }
            using (var con = DBUtils.getConnection(_props))
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "UPDATE Bug SET title = @title, description = @description, createdAt = @createdAt, status = @status WHERE id = @id";

                var paramTitle = comm.CreateParameter();
                paramTitle.ParameterName = "@title";
                paramTitle.Value = entity.Title;
                comm.Parameters.Add(paramTitle);

                var paramDesc = comm.CreateParameter();
                paramDesc.ParameterName = "@description";
                paramDesc.Value = entity.Description;
                comm.Parameters.Add(paramDesc);

                var paramCreatedAt = comm.CreateParameter();
                paramCreatedAt.ParameterName = "@createdAt";
                paramCreatedAt.Value = entity.CreatedAt;
                comm.Parameters.Add(paramCreatedAt);

                var paramStatus = comm.CreateParameter();
                paramStatus.ParameterName = "@status";
                paramStatus.Value = entity.Status.ToString();
                comm.Parameters.Add(paramStatus);

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
