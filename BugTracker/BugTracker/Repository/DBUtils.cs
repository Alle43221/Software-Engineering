using System;
using System.Data;
using System.Collections.Generic;

namespace BugTracker.Repository
{
	public static class DBUtils
	{
		
		private static IDbConnection instance = null;
		
		public static IDbConnection getConnection(IDictionary<string,string> props)
		{
			if (instance == null || instance.State == System.Data.ConnectionState.Closed)
			{
				instance = getNewConnection(props);
				instance.Open();
			}
			return instance;
		}

		private static IDbConnection getNewConnection(IDictionary<string,string> props)
		{
			return ConnectionFactory.getInstance().createConnection(props);
		}

        public static string GetConnectionString(IDictionary<string, string> props)
        {
            // Example: Assuming the connection string is passed in props dictionary
            if (props.ContainsKey("ConnectionString"))
            {
                return props["ConnectionString"];
            }
            throw new Exception("Connection string not found in provided properties.");
        }


    }
}
