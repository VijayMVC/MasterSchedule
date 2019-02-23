using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class OutsoleMaterialRejectIssuesController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
        public static List<OutsoleMaterialRejectIssuesModel> Select()
        {
            return db.ExecuteStoreQuery<OutsoleMaterialRejectIssuesModel>("EXEC spm_SelectOutsoleMaterialRejectIssues").ToList();
        }
    }
}
