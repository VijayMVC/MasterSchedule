using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class RawMaterialController
    {
        public static bool Insert(RawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @MaterialTypeId = new SqlParameter("@MaterialTypeId", model.MaterialTypeId);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);
            var @Remarks = new SqlParameter("@Remarks", model.Remarks);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertRawMaterial @ProductNo,@MaterialTypeId,@ETD,@ActualDate,@Remarks", @ProductNo, @MaterialTypeId, @ETD, @ActualDate, @Remarks) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Insert_2(RawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @MaterialTypeId = new SqlParameter("@MaterialTypeId", model.MaterialTypeId);
            var @ETD = new SqlParameter("@ETD", model.ETD);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);
            var @Remarks = new SqlParameter("@Remarks", model.Remarks);

            var @IsETDUpdate = new SqlParameter("@IsETDUpdate", model.IsETDUpdate);
            var @IsActualDateUpdate = new SqlParameter("@IsActualDateUpdate", model.IsActualDateUpdate);
            var @IsRemarksUpdate = new SqlParameter("@IsRemarksUpdate", model.IsRemarksUpdate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertRawMaterial_2 @ProductNo,@MaterialTypeId,@ETD,@ActualDate,@Remarks,@IsETDUpdate,@IsActualDateUpdate,@IsRemarksUpdate", @ProductNo, @MaterialTypeId, @ETD, @ActualDate, @Remarks, @IsETDUpdate, @IsActualDateUpdate, @IsRemarksUpdate) > 0)
            {
                return true;
            }
            return false;
        }

        public static List<RawMaterialModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<RawMaterialModel>("EXEC spm_SelectRawMaterial").ToList();
        }
    }
}
