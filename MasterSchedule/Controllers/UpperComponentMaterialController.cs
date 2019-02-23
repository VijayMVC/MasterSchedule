using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class UpperComponentMaterialController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

        public static List<UpperComponentMaterialModel> Select()
        {
            return db.ExecuteStoreQuery<UpperComponentMaterialModel>("EXEC spm_SelectUpperComponentMaterial").ToList();
        }

        public static List<UpperComponentMaterialModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<UpperComponentMaterialModel>("EXEC spm_SelectUpperComponentMaterialByProductNo @ProductNo", @ProductNo).ToList();
        }

        public static bool Insert(UpperComponentMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @UpperComponentID = new SqlParameter("@UpperComponentID", model.UpperComponentID);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_InsertUpperComponentMaterial_2 @ProductNo,@UpperComponentID,@SizeNo,@Quantity,@QuantityReject", @ProductNo, @UpperComponentID, @SizeNo, @Quantity, @QuantityReject) >= 1)
            {
                return true;
            }
            return false;
        }

        public static bool Update(UpperComponentMaterialModel model, bool updateReject, bool updateQuantity)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @UpperComponentID = new SqlParameter("@UpperComponentID", model.UpperComponentID);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);

            var @UpdateReject = new SqlParameter("@UpdateReject", updateReject);
            var @UpdateQuantity = new SqlParameter("@UpdateQuantity", updateQuantity);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_UpdateUpperComponentMaterial @ProductNo,@UpperComponentID,@SizeNo,@Quantity,@QuantityReject,@UpdateReject,@UpdateQuantity", @ProductNo, @UpperComponentID, @SizeNo, @Quantity, @QuantityReject, @UpdateReject, @UpdateQuantity) >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
