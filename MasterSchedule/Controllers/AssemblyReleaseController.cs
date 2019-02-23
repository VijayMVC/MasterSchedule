using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class AssemblyReleaseController
    {
        public static List<AssemblyReleaseModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<AssemblyReleaseModel>("EXEC spm_SelectAssemblyReleaseByProductNo @ProductNo", @ProductNo).ToList();
        }

        public static bool Delete(string reportId, string productNo)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            var @ProductNo = new SqlParameter("@ProductNo", productNo);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_DeleteAssemblyReleaseByReportIdProductNo @ReportId, @ProductNo", @ReportId, @ProductNo) >= 1)
            {
                return true;
            }
            return false;
        }

        public static bool Insert(AssemblyReleaseModel model)
        {
            var @ReportId = new SqlParameter("@ReportId", model.ReportId);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Cycle = new SqlParameter("@Cycle", model.Cycle);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_InsertAssemblyRelease @ReportId,@ProductNo,@Cycle,@SizeNo,@Quantity", @ReportId, @ProductNo, @Cycle, @SizeNo, @Quantity) >= 1)
            {
                return true;
            }
            return false;
        }

        public static List<AssemblyReleaseModel> Select(string reportId, string productNo)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<AssemblyReleaseModel>("EXEC spm_SelectAssemblyReleaseByReportId @ReportId", @ReportId).ToList();
        }

        public static List<AssemblyReleaseModel> SelectByAssemblyMaster()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<AssemblyReleaseModel>("EXEC spm_SelectAssemblyReleaseByAssemblyMaster").ToList();
        }

        public static List<AssemblyReleaseModel> SelectReportId()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<AssemblyReleaseModel>("EXEC spm_SelectAssemblyReleaseReportId_2").ToList();
        }
    }
}
