using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simionic.CustomProfiles.ImportExport;
using LiteDB;

namespace Simionic.PowerTools.DesktopApp
{
    public class DataStore
    {
        private string _dbPath;
        private LiteDatabase _database;

        public static LiteDatabase Profiles { get; private set; }
        public static LiteDatabase FlightPlans { get; private set; }


        public DataStore()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = Path.Combine(appPath, "datastore.db");

            _database = new LiteDatabase(_dbPath);
        }
    }
}
