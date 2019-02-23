using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    public class OutsoleMaterialDetailController
    {
       private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

       public static List<OutsoleMaterialDetailModel> SelectAll()
       {
           return db.ExecuteStoreQuery<OutsoleMaterialDetailModel>("EXEC spm_SelectOutsoleMaterialDetailAll").ToList();
       }
       public static List<OutsoleMaterialDetailModel> Select(string productNo)
       {
           var @ProductNo = new SqlParameter("@ProductNo", productNo);
           return db.ExecuteStoreQuery<OutsoleMaterialDetailModel>("EXEC spm_SelectOutsoleMaterialDetail @ProductNo", @ProductNo).ToList();
       }

       public static bool Insert(OutsoleMaterialDetailModel model, AccountModel account)
       {
           var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
           var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
           var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
           var @Quantity = new SqlParameter("@Quantity", model.Quantity);
           var @Reject = new SqlParameter("@Reject", model.Reject);
           var @QuantityExcess = new SqlParameter("@QuantityExcess", model.QuantityExcess);
           var @CreatedBy = new SqlParameter("@CreatedBy", account.UserName);
           var @IndexNo = new SqlParameter("@IndexNo", model.IndexNo);
           var @Round = new SqlParameter("@Round", model.Round);
           if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterialDetail @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @Reject, @QuantityExcess,@CreatedBy,@IndexNo,@Round", @ProductNo, @OutsoleSupplierId, @SizeNo, @Quantity, @Reject, @QuantityExcess, @CreatedBy, @IndexNo, @Round) >= 1)
           {
               return true;
           }
           return false;
       }

       public static bool Delete(string productNo, int outsoleSupplierId, DateTime createdDate)
       {
           var @ProductNo = new SqlParameter("@ProductNo", productNo);
           var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", outsoleSupplierId);
           var @CreatedDate = new SqlParameter("@CreatedDate", createdDate);
           if (db.ExecuteStoreCommand("EXEC spm_DeleteOutsoleMaterialDetail @ProductNo, @OutsoleSupplierId, @CreatedDate", @ProductNo, @OutsoleSupplierId, @CreatedDate) >= 1)
           {
               return true;
           }
           return false;
       }
    }


}
