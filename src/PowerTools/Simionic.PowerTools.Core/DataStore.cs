using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simionic.CustomProfiles.ImportExport;
using LiteDB;

namespace Simionic.PowerTools.Core
{
    public class DataStore
    {
        private string _dbPath;
        private LiteDatabase _database;

        public DataStore()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = Path.Combine(appPath, "datastore.db");

            _database = new LiteDatabase(_dbPath);
        }
    }
}
