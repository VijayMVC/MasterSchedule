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
    class OutsoleMasterController
    {
        public static List<OutsoleMasterModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OutsoleMasterModel>("EXEC spm_SelectOutsoleMaster").ToList();
        }

        public static bool InsertSequence(OutsoleMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @OutsoleStartDate = new SqlParameter("@OutsoleStartDate", model.OutsoleStartDate);
            var @OutsoleFinishDate = new SqlParameter("@OutsoleFinishDate", model.OutsoleFinishDate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMasterSequence @ProductNo,@Sequence,@OutsoleStartDate,@OutsoleFinishDate", @ProductNo, @Sequence, @OutsoleStartDate, @OutsoleFinishDate) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool InsertOutsole(OutsoleMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleLine = new SqlParameter("@OutsoleLine", model.OutsoleLine);
            var @OutsoleStartDate = new SqlParameter("@OutsoleStartDate", model.OutsoleStartDate);
            var @OutsoleFinishDate = new SqlParameter("@OutsoleFinishDate", model.OutsoleFinishDate);
            var @OutsoleQuota = new SqlParameter("@OutsoleQuota", model.OutsoleQuota);
            var @OutsoleActualStartDate = new SqlParameter("@OutsoleActualStartDate", model.OutsoleActualStartDate);
            var @OutsoleActualFinishDate = new SqlParameter("@OutsoleActualFinishDate", model.OutsoleActualFinishDate);
            var @OutsoleBalance = new SqlParameter("@OutsoleBalance", model.OutsoleBalance);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMasterOutsole @ProductNo,@OutsoleLine,@OutsoleStartDate,@OutsoleFinishDate,@OutsoleQuota,@OutsoleActualStartDate,@OutsoleActualFinishDate,@OutsoleBalance", @ProductNo, @OutsoleLine, @OutsoleStartDate, @OutsoleFinishDate, @OutsoleQuota, @OutsoleActualStartDate, @OutsoleActualFinishDate, @OutsoleBalance) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Insert_2(OutsoleMasterModel model)
        {
            DateTime dtDefault = new DateTime(2000, 01, 01);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @OutsoleLine = new SqlParameter("@OutsoleLine", model.OutsoleLine);
            var @OutsoleStartDate = new SqlParameter("@OutsoleStartDate", model.OutsoleStartDate);
            var @OutsoleFinishDate = new SqlParameter("@OutsoleFinishDate", model.OutsoleFinishDate);
            var @OutsoleQuota = new SqlParameter("@OutsoleQuota", model.OutsoleQuota);
            var @OutsoleActualStartDate = new SqlParameter("@OutsoleActualStartDate", model.OutsoleActualStartDate);
            var @OutsoleActualFinishDate = new SqlParameter("@OutsoleActualFinishDate", model.OutsoleActualFinishDate);

            DateTime outsoleActualStartDateAutoDt = TimeHelper.Convert(model.OutsoleActualStartDateAuto);
            DateTime outsoleActualFinishDateAutoDt = TimeHelper.Convert(model.OutsoleActualFinishDateAuto);
            string outsoleActualStartDateAutoString = "";
            if (outsoleActualStartDateAutoDt != dtDefault)
            {
                outsoleActualStartDateAutoString = String.Format("{0:MM/dd/yyyy}", outsoleActualStartDateAutoDt);
            }
            string outsoleActualFinishDateAutoString = "";
            if (outsoleActualFinishDateAutoDt != dtDefault)
            {
                outsoleActualFinishDateAutoString = String.Format("{0:MM/dd/yyyy}", outsoleActualFinishDateAutoDt);
            }


            var @OutsoleActualStartDateAuto = new SqlParameter("@OutsoleActualStartDateAuto", outsoleActualStartDateAutoString);
            var @OutsoleActualFinishDateAuto = new SqlParameter("@OutsoleActualFinishDateAuto", outsoleActualFinishDateAutoString);

            var @OutsoleBalance = new SqlParameter("@OutsoleBalance", model.OutsoleBalance);

            var @IsSequenceUpdate = new SqlParameter("@IsSequenceUpdate", model.IsSequenceUpdate);
            var @IsOutsoleLineUpdate = new SqlParameter("@IsOutsoleLineUpdate", model.IsOutsoleLineUpdate);
            var @IsOutsoleStartDateUpdate = new SqlParameter("@IsOutsoleStartDateUpdate", model.IsOutsoleStartDateUpdate);
            var @IsOutsoleFinishDateUpdate = new SqlParameter("@IsOutsoleFinishDateUpdate", model.IsOutsoleFinishDateUpdate);
            var @IsOutsoleQuotaUpdate = new SqlParameter("@IsOutsoleQuotaUpdate", model.IsOutsoleQuotaUpdate);
            var @IsOutsoleActualStartDateUpdate = new SqlParameter("@IsOutsoleActualStartDateUpdate", model.IsOutsoleActualStartDateUpdate);
            var @IsOutsoleActualFinishDateUpdate = new SqlParameter("@IsOutsoleActualFinishDateUpdate", model.IsOutsoleActualFinishDateUpdate);

            var @IsOutsoleActualStartDateAutoUpdate = new SqlParameter("@IsOutsoleActualStartDateAutoUpdate", model.IsOutsoleActualStartDateAutoUpdate);
            var @IsOutsoleActualFinishDateAutoUpdate = new SqlParameter("@IsOutsoleActualFinishDateAutoUpdate", model.IsOutsoleActualFinishDateAutoUpdate);

            var @IsOutsoleBalanceUpdate = new SqlParameter("@IsOutsoleBalanceUpdate", model.IsOutsoleBalanceUpdate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand(@"EXEC spm_InsertOutsoleMaster_3 @ProductNo, @Sequence, @OutsoleLine, @OutsoleStartDate, @OutsoleFinishDate, @OutsoleQuota, @OutsoleActualStartDate, @OutsoleActualFinishDate, @OutsoleActualStartDateAuto, @OutsoleActualFinishDateAuto, @OutsoleBalance, @IsSequenceUpdate, @IsOutsoleLineUpdate, @IsOutsoleStartDateUpdate, @IsOutsoleFinishDateUpdate, @IsOutsoleQuotaUpdate, @IsOutsoleActualStartDateUpdate, @IsOutsoleActualFinishDateUpdate, @IsOutsoleActualStartDateAutoUpdate, @IsOutsoleActualFinishDateAutoUpdate, @IsOutsoleBalanceUpdate",
                                                                        @ProductNo, @Sequence, @OutsoleLine, @OutsoleStartDate, @OutsoleFinishDate, @OutsoleQuota, @OutsoleActualStartDate, @OutsoleActualFinishDate, @OutsoleActualStartDateAuto, @OutsoleActualFinishDateAuto, @OutsoleBalance, @IsSequenceUpdate, @IsOutsoleLineUpdate, @IsOutsoleStartDateUpdate, @IsOutsoleFinishDateUpdate, @IsOutsoleQuotaUpdate, @IsOutsoleActualStartDateUpdate, @IsOutsoleActualFinishDateUpdate, @IsOutsoleActualStartDateAutoUpdate, @IsOutsoleActualFinishDateAutoUpdate, @IsOutsoleBalanceUpdate) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
