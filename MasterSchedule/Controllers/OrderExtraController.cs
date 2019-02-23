using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Entities;
using MasterSchedule.Models;
using System.Data.SqlClient;
namespace MasterSchedule.Controllers
{
    class OrderExtraController
    {
        static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

        public static bool Insert(OrderExtraModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @LoadingDate = new SqlParameter("@LoadingDate", model.LoadingDate);

            if (db.ExecuteStoreCommand("EXEC spm_InsertOrderExtra @ProductNo,@LoadingDate", @ProductNo, @LoadingDate) > 0)
            {
                return true;
            }
            return false;
        }

        public static List<OrderExtraModel> Select()
        {
            return db.ExecuteStoreQuery<OrderExtraModel>("EXEC spm_SelectOrderExtra").ToList();
        }
    }
}
