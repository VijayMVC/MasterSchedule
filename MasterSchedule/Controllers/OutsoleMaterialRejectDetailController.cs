using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class OutsoleMaterialRejectDetailController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

        public static List<OutsoleMaterialRejectDetailModel> SelectPerPOPerSupplierPerIndexNoPerRound(string productNo, int supplierId, string indexNo, int round)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", supplierId);
            var @IndexNo = new SqlParameter("@IndexNo", indexNo);
            var @Round = new SqlParameter("@Round", round);
            return db.ExecuteStoreQuery<OutsoleMaterialRejectDetailModel>("EXEC spm_SelectOutsoleMaterialRejectDetailPerPO @ProductNo, @OutsoleSupplierId, @IndexNo,@Round", @ProductNo, @OutsoleSupplierId, @IndexNo, @Round).ToList();
        }

        public static bool Insert(OutsoleMaterialRejectDetailModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @OutsoleMaterialRejectIssuesId = new SqlParameter("@OutsoleMaterialRejectIssuesId", model.OutsoleMaterialRejectIssuesId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);
            var @CreatedBy = new SqlParameter("@CreatedBy", model.CreatedBy);
            var @IndexNo = new SqlParameter("@IndexNo", model.IndexNo);
            var @Round = new SqlParameter("@Round", model.Round);

            if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterialRejectDetail @ProductNo, @OutsoleSupplierId, @OutsoleMaterialRejectIssuesId, @SizeNo, @QuantityReject, @CreatedBy, @IndexNo, @Round", @ProductNo, @OutsoleSupplierId, @OutsoleMaterialRejectIssuesId, @SizeNo, @QuantityReject, @CreatedBy, @IndexNo, @Round) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool UpdateRejectToOutsoleMaterialDetail(OutsoleMaterialDetailModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Reject = new SqlParameter("@Reject", model.Reject);
            var @CreatedBy = new SqlParameter("@CreatedBy", model.CreatedBy);
            var @IndexNo = new SqlParameter("@IndexNo", model.IndexNo);
            var @Round = new SqlParameter("@Round", model.Round);

            if (db.ExecuteStoreCommand("EXEC spm_UpdateOutsoleMaterialDetailFromOutsoleMaterialRejectDetail @ProductNo, @OutsoleSupplierId, @SizeNo, @Reject, @CreatedBy, @IndexNo, @Round", @ProductNo, @OutsoleSupplierId, @SizeNo, @Reject, @CreatedBy, @IndexNo, @Round) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
