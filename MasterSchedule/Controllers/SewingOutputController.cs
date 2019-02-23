using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class SewingOutputController
    {
        public static List<SewingOutputModel> SelectByAssemblyMaster()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<SewingOutputModel>("EXEC spm_SelectSewingOutputByAssemblyMaster").ToList();
        }

        public static List<SewingOutputModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<SewingOutputModel>("EXEC spm_SelectSewingOutputByProductNo @ProductNo", @ProductNo).ToList();
        }  

        public static bool Insert(SewingOutputModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);            
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_InsertSewingOutput @ProductNo,@SizeNo,@Quantity", @ProductNo, @SizeNo, @Quantity) >= 1)
            {
                return true;
            }
            return false;
        }

        public static List<SewingOutputModel> SelectByAssemblyRelease(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<SewingOutputModel>("EXEC spm_SelectSewingOutputByAssemblyReleaseByReportId @ReportId", @ReportId).ToList();
        }
    }
}
