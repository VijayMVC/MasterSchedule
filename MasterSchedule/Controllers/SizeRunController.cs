using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;
namespace MasterSchedule.Controllers
{
    class SizeRunController
    {
        public static List<SizeRunModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunByProductNo @ProductNo", @ProductNo).ToList();
        }

        //spm_SelectSizeRunIsnable
        public static List<SizeRunModel> SelectIsEnable()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunIsnable").ToList();
        }
        public static List<SizeRunModel> SelectPerArticle(string articleNo)
        {
            var @ArticleNo = new SqlParameter("@ArticleNo", articleNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunByArticle @ArticleNo", @ArticleNo).ToList();
        }

        public static List<SizeRunModel> SelectPerOutsoleCode(string outsoleCode)
        {
            var @OutsoleCode = new SqlParameter("@OutsoleCode", outsoleCode);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunByOutsoleCode @OutsoleCode", @OutsoleCode).ToList();
        }

        public static List<SizeRunModel> SelectByOutsoleRawMaterial()
        {            
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunByOutsoleRawMaterial").ToList();
        }

        public static List<SizeRunModel> SelectByInsockRawMaterial()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunByInsockRawMaterial").ToList();
        }

        public static List<SizeRunModel> SelectByOutsoleReleaseMaterial(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunByOutsoleReleaseMaterialByReportId @ReportId", @ReportId).ToList();
        }

        public static List<SizeRunModel> SelectByAssemblyRelease(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<SizeRunModel>("EXEC spm_SelectSizeRunByAssemblyReleaseByReportId @ReportId", @ReportId).ToList();
        }

        public static bool Insert(SizeRunModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSizeRun @ProductNo,@SizeNo,@Quantity", @ProductNo, @SizeNo, @Quantity) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Update(SizeRunModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_UpdateSizeRun @ProductNo,@SizeNo,@Quantity", @ProductNo, @SizeNo, @Quantity) > 0)
            {
                return true;
            }
            return false;
        }


        public static bool UpdateSizeMap(SizeRunModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @OutsoleSize = new SqlParameter("@OutsoleSize", model.OutsoleSize);
            var @MidsoleSize = new SqlParameter("@MidsoleSize", model.MidsoleSize);
            var @UpdateOutsoleSizeByArticle = new SqlParameter("@UpdateOutsoleSizeByArticle", model.UpdateOutsoleSizeByArticle);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_UpdateSizeRunMap_1 @ProductNo,@SizeNo,@OutsoleSize,@MidsoleSize,@UpdateOutsoleSizeByArticle", @ProductNo, @SizeNo, @OutsoleSize, @MidsoleSize, @UpdateOutsoleSizeByArticle) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
