using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    class OutsoleWHFGController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

        public static List<OutsoleWHFGModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<OutsoleWHFGModel>("EXEC spm_SelectOutsoleWHFG @ProductNo", @ProductNo).ToList();
        }

        public static bool Insert(OutsoleWHFGModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @Quantity = new SqlParameter("@Quantity", model.Quantity);
            var @CreatedDate = new SqlParameter("@CreatedDate", model.CreatedDate);

            if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleWHFG @ProductNo, @SizeNo, @Quantity, @CreatedDate", @ProductNo, @SizeNo, @Quantity, @CreatedDate) >= 1)
            {
                return true;
            }
            return false;
        }

        public static bool Delete(string productNo, DateTime createdDate)
       {
           var @ProductNo = new SqlParameter("@ProductNo", productNo);
           var @CreatedDate = new SqlParameter("@CreatedDate", createdDate);
           if (db.ExecuteStoreCommand("EXEC spm_DeleteOutsoleWHFG @ProductNo, @CreatedDate", @ProductNo, @CreatedDate) >= 1)
           {
               return true;
           }
           return false;
       }
    }
}
