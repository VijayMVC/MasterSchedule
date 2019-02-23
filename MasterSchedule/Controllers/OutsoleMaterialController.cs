using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class OutsoleMaterialController
    {
        public static List<OutsoleMaterialModel> Select()
        {            
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterial").ToList();
        }

        public static List<OutsoleMaterialModel> SelectReject()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialReject").ToList();
        }

        public static List<OutsoleMaterialModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
           
            return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByProductNo @ProductNo", @ProductNo).ToList();
        }

        public static List<OutsoleMaterialModel> SelectByOutsoleRawMaterial()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByOutsoleRawMaterial").ToList();
        }

        public static List<OutsoleMaterialModel> SelectByOutsoleReleaseMaterial(string reportId)
        {
            var @ReportId = new SqlParameter("@ReportId", reportId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            return db.ExecuteStoreQuery<OutsoleMaterialModel>("EXEC spm_SelectOutsoleMaterialByOutsoleReleaseMaterialByReportId @ReportId", @ReportId).ToList();
        }

        public static bool Insert(OutsoleMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);
            var @RejectAssembly = new SqlParameter("@RejectAssembly", model.RejectAssembly);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterial_3 @ProductNo,@OutsoleSupplierId,@SizeNo,@Quantity,@QuantityReject,@RejectAssembly", @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @QuantityReject, @RejectAssembly) >= 1)
            {
                return true;
            }
            return false;
        }


        public static bool UpdateRejectFromOutsoleMaterialDetail(OutsoleMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleMaterialFromOutsoleMaterialDetail @ProductNo,@OutsoleSupplierId,@SizeNo,@QuantityReject", @ProductNo, @OutsoleSupplierId, @SizeNo, @QuantityReject) >= 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Upadate Outsole Material
        /// </summary>
        /// <param name="model">Material Update</param>
        /// <param name="updateReject">Update Reject?</param>
        /// <param name="updateQuantity">Update Quantity?</param>
        /// <param name="updateRejectAssembly">Update Reject Assembly?</param>
        /// <returns></returns>
        public static bool Update(OutsoleMaterialModel model, bool updateReject, bool updateQuantity, bool updateRejectAssembly)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);
            var @RejectAssembly = new SqlParameter("@RejectAssembly", model.RejectAssembly);

            var @UpdateQuantity = new SqlParameter("@UpdateQuantity", updateQuantity);
            var @UpdateReject = new SqlParameter("@UpdateReject", updateReject);
            var @UpdateRejectAssembly = new SqlParameter("@UpdateRejectAssembly", updateRejectAssembly);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleMaterial_1 @ProductNo,@OutsoleSupplierId,@SizeNo,@Quantity,@QuantityReject,@RejectAssembly,@UpdateReject,@UpdateQuantity,@UpdateRejectAssembly", @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @QuantityReject, @RejectAssembly, @UpdateReject, @UpdateQuantity, @UpdateRejectAssembly) >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
