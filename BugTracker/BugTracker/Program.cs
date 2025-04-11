using BugTracker.Repository;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using BugTracker.Service;

namespace BugTracker
{
    internal static class Program
    {

        static string GetConnectionStringByName(string name)
        {
            // Presupunem ca nu exista.
            string returnValue = null;
            // Cauta numele in sectiunea connectionStrings. 
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            // Daca este gasit, returneaza valoarea asociata la connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;
            return returnValue;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        [STAThread]
        static void Main()
        {

            IDictionary<string, string> props = new SortedList<string, string>();
            props.Add("ConnectionString", GetConnectionStringByName("MSSql"));

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Util.LogLog.InternalDebugging = true;
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Info("Application started");

            IUserRepository uR = new UserDbRepository(props);
            IBugRepository bR = new BugDbRepository(props);

            Service.MyService service = new MyService(bR, uR);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogIn(service));
        }
    }
}
