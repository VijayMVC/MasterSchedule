using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    public class InsockMaterialController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
        public static List<InsockMaterialModel> Select()
        {
            return db.ExecuteStoreQuery<InsockMaterialModel>("EXEC spm_SelectInsockMaterial").ToList();
        }

        public static List<InsockMaterialModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<InsockMaterialModel>("EXEC spm_SelectInsockMaterialByProductNo @ProductNo", @ProductNo).ToList();
        }

        public static bool Insert(InsockMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @InsockSupplierId = new SqlParameter("@InsockSupplierId", model.InsockSupplierId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_InsertInsockMaterial @ProductNo, @InsockSupplierId, @SizeNo, @Quantity, @QuantityReject", @ProductNo, @InsockSupplierId, @SizeNo, @Quantity, @QuantityReject) >= 1)
            {
                return true;
            }
            return false;
        }

        public static bool Update(InsockMaterialModel model, bool updateReject, bool updateQuantity)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @InsockSupplierId = new SqlParameter("@InsockSupplierId", model.InsockSupplierId);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @QuantityReject = new SqlParameter("@QuantityReject", model.QuantityReject);

            var @UpdateReject = new SqlParameter("@UpdateReject", updateReject);
            var @UpdateQuantity = new SqlParameter("@UpdateQuantity", updateQuantity);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

            if (db.ExecuteStoreCommand("EXEC spm_UpdateInsockMaterial @ProductNo, @InsockSupplierId, @SizeNo, @Quantity, @QuantityReject, @UpdateReject, @UpdateQuantity", @ProductNo, @InsockSupplierId, @SizeNo, @Quantity, @QuantityReject, @UpdateReject, @UpdateQuantity) >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
