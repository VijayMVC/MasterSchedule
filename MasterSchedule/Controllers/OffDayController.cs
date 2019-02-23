using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;

using MasterSchedule.Entities;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class OffDayController
    {
        public static List<OffDayModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OffDayModel>("EXEC spm_SelectOffDay").ToList();
        }

        public static List<OffDayModel> SelectDate()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OffDayModel>("EXEC spm_SelectDate").ToList();
        }

        public static bool Insert(OffDayModel model)
        {
            var @Date = new SqlParameter("@Date", model.Date);
            var @Remarks = new SqlParameter("@Remarks", model.Remarks);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertOffDay @Date, @Remarks", @Date, @Remarks) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Delete(DateTime date)
        {
            var @Date = new SqlParameter("@Date", date);            
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_DeleteOffDay @Date", @Date) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
