using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Entities;
namespace MasterSchedule.Helpers
{
    class DatabaseHelper
    {
        public static bool Exist()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.DatabaseExists() == true)
            {
                return true;
            }
            return false;
        }
    }
}
