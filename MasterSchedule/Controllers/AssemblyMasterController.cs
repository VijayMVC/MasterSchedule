using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;
using MasterSchedule.Entities;
using MasterSchedule.Helpers;
using System.Data.SqlClient;

namespace MasterSchedule.Controllers
{
    class AssemblyMasterController
    {
        public static List<AssemblyMasterModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<AssemblyMasterModel>("EXEC spm_SelectAssemblyMaster").ToList();
        }

        public static List<AssemblyMasterModel> SelectAssemblyByProductNo()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<AssemblyMasterModel>("EXEC spm_SelectAssemblyMasterByProductNo").ToList();
        }

        public static bool InsertSequence(AssemblyMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @AssemblyStartDate = new SqlParameter("@AssemblyStartDate", model.AssemblyStartDate);
            var @AssemblyFinishDate = new SqlParameter("@AssemblyFinishDate", model.AssemblyFinishDate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertAssemblyMasterSequence @ProductNo,@Sequence,@AssemblyStartDate,@AssemblyFinishDate", @ProductNo, @Sequence, @AssemblyStartDate, @AssemblyFinishDate) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool InsertAssembly(AssemblyMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @AssemblyLine = new SqlParameter("@AssemblyLine", model.AssemblyLine);
            var @AssemblyStartDate = new SqlParameter("@AssemblyStartDate", model.AssemblyStartDate);
            var @AssemblyFinishDate = new SqlParameter("@AssemblyFinishDate", model.AssemblyFinishDate);
            var @AssemblyQuota = new SqlParameter("@AssemblyQuota", model.AssemblyQuota);
            var @AssemblyActualStartDate = new SqlParameter("@AssemblyActualStartDate", model.AssemblyActualStartDate);
            var @AssemblyActualFinishDate = new SqlParameter("@AssemblyActualFinishDate", model.AssemblyActualFinishDate);
            var @AssemblyBalance = new SqlParameter("@AssemblyBalance", model.AssemblyBalance);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertAssemblyMasterAssembly @ProductNo,@AssemblyLine,@AssemblyStartDate,@AssemblyFinishDate,@AssemblyQuota,@AssemblyActualStartDate,@AssemblyActualFinishDate,@AssemblyBalance", @ProductNo, @AssemblyLine, @AssemblyStartDate, @AssemblyFinishDate, @AssemblyQuota, @AssemblyActualStartDate, @AssemblyActualFinishDate, @AssemblyBalance) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Insert_2(AssemblyMasterModel model)
        {
            DateTime dtDefault = new DateTime(2000, 01, 01);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @AssemblyLine = new SqlParameter("@AssemblyLine", model.AssemblyLine);
            var @AssemblyStartDate = new SqlParameter("@AssemblyStartDate", model.AssemblyStartDate);
            var @AssemblyFinishDate = new SqlParameter("@AssemblyFinishDate", model.AssemblyFinishDate);
            var @AssemblyQuota = new SqlParameter("@AssemblyQuota", model.AssemblyQuota);

            DateTime assemblyActualStartDateDt = TimeHelper.Convert(model.AssemblyActualStartDate);
            DateTime assemblyActualFinishDateDt = TimeHelper.Convert(model.AssemblyActualFinishDate);
            string assemblyActualStartDateString = "";
            if (assemblyActualStartDateDt != dtDefault)
            {
                assemblyActualStartDateString = String.Format("{0:MM/dd/yyyy}", assemblyActualStartDateDt);
            }
            string assemblyActualFinishDateString = "";
            if (assemblyActualFinishDateDt != dtDefault)
            {
                assemblyActualFinishDateString = String.Format("{0:MM/dd/yyyy}", assemblyActualFinishDateDt);
            }

            var @AssemblyActualStartDate = new SqlParameter("@AssemblyActualStartDate", assemblyActualStartDateString);
            var @AssemblyActualFinishDate = new SqlParameter("@AssemblyActualFinishDate", assemblyActualFinishDateString);

            var @AssemblyBalance = new SqlParameter("@AssemblyBalance", model.AssemblyBalance);

            var @IsSequenceUpdate = new SqlParameter("@IsSequenceUpdate", model.IsSequenceUpdate);
            var @IsAssemblyLineUpdate = new SqlParameter("@IsAssemblyLineUpdate", model.IsAssemblyLineUpdate);
            var @IsAssemblyStartDateUpdate = new SqlParameter("@IsAssemblyStartDateUpdate", model.IsAssemblyStartDateUpdate);
            var @IsAssemblyFinishDateUpdate = new SqlParameter("@IsAssemblyFinishDateUpdate", model.IsAssemblyFinishDateUpdate);
            var @IsAssemblyQuotaUpdate = new SqlParameter("@IsAssemblyQuotaUpdate", model.IsAssemblyQuotaUpdate);
            var @IsAssemblyActualStartDateUpdate = new SqlParameter("@IsAssemblyActualStartDateUpdate", model.IsAssemblyActualStartDateUpdate);
            var @IsAssemblyActualFinishDateUpdate = new SqlParameter("@IsAssemblyActualFinishDateUpdate", model.IsAssemblyActualFinishDateUpdate);
            var @IsAssemblyBalanceUpdate = new SqlParameter("@IsAssemblyBalanceUpdate", model.IsAssemblyBalanceUpdate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertAssemblyMaster_2 @ProductNo, @Sequence, @AssemblyLine, @AssemblyStartDate, @AssemblyFinishDate, @AssemblyQuota, @AssemblyActualStartDate, @AssemblyActualFinishDate, @AssemblyBalance, @IsSequenceUpdate, @IsAssemblyLineUpdate, @IsAssemblyStartDateUpdate, @IsAssemblyFinishDateUpdate, @IsAssemblyQuotaUpdate, @IsAssemblyActualStartDateUpdate, @IsAssemblyActualFinishDateUpdate, @IsAssemblyBalanceUpdate",
                                                                        @ProductNo, @Sequence, @AssemblyLine, @AssemblyStartDate, @AssemblyFinishDate, @AssemblyQuota, @AssemblyActualStartDate, @AssemblyActualFinishDate, @AssemblyBalance, @IsSequenceUpdate, @IsAssemblyLineUpdate, @IsAssemblyStartDateUpdate, @IsAssemblyFinishDateUpdate, @IsAssemblyQuotaUpdate, @IsAssemblyActualStartDateUpdate, @IsAssemblyActualFinishDateUpdate, @IsAssemblyBalanceUpdate) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
