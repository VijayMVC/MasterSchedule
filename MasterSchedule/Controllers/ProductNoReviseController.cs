using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class ProductNoReviseController
    {
        public static bool Insert(ProductNoReviseModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @ReviseDate = new SqlParameter("@ReviseDate", model.ReviseDate);
            var @SectionId = new SqlParameter("@SectionId", model.SectionId);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertProductNoRevise @ProductNo, @ReviseDate, @SectionId", @ProductNo, @ReviseDate, @SectionId) > 0)
            {
                return true;
            }
            return false;
        }

        public static List<ProductNoReviseModel> SelectProductNoReviseToday()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<ProductNoReviseModel>("EXEC spm_SelectProductNoReviseToDay").ToList();
        }
    }
}
