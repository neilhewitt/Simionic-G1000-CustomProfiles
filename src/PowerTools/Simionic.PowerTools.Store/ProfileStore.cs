using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simionic.PowerTools.Store
{
    public static class ProfileStore
    {
        private const string DATABASE_FILE_NAME = "ProfileStore.db";
        private static LiteDatabase _store;


        
        static ProfileStore()
        {
            _store = new LiteDatabase(DATABASE_FILE_NAME);
        }
    }
}
