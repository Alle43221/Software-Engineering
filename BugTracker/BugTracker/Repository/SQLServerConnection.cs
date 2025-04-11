using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BugTracker.Repository
{
	public class SQLServerConnection : ConnectionFactory
	{
		
		public override IDbConnection createConnection(IDictionary<string,string> props)
		{
			//Mono Sqlite Connection

			// String connectionString = GetConnectionStringByName(props["name"]);
			String connectionString = props["ConnectionString"];
			Console.WriteLine("SQL Server ---Se deschide o conexiune la  ... {0}", connectionString);
			return new SqlConnection(connectionString);

			// Windows SQLite Connection, fisierul .db ar trebuie sa fie in directorul bin/debug
			//String connectionString = "Data Source=tasks.db;Version=3";
			//return new SQLiteConnection(connectionString);
		}
	}
}
