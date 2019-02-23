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
    class SockliningMasterController
    {
        public static List<SockliningMasterModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SockliningMasterModel>("EXEC spm_SelectSockliningMaster").ToList();
        }

        public static bool InsertSequence(SockliningMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @SockliningStartDate = new SqlParameter("@SockliningStartDate", model.SockliningStartDate);
            var @SockliningFinishDate = new SqlParameter("@SockliningFinishDate", model.SockliningFinishDate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSockliningMasterSequence @ProductNo,@Sequence,@SockliningStartDate,@SockliningFinishDate", @ProductNo, @Sequence, @SockliningStartDate, @SockliningFinishDate) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool InsertSocklining(SockliningMasterModel model)
        {
            DateTime dtDefault = new DateTime(2000, 01, 01);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SockliningLine = new SqlParameter("@SockliningLine", model.SockliningLine);
            var @SockliningStartDate = new SqlParameter("@SockliningStartDate", model.SockliningStartDate);
            var @SockliningFinishDate = new SqlParameter("@SockliningFinishDate", model.SockliningFinishDate);
            var @SockliningQuota = new SqlParameter("@SockliningQuota", model.SockliningQuota);

            DateTime sockliningActualStartDateDt = TimeHelper.Convert(model.SockliningActualStartDate);
            DateTime sockliningActualFinishDateDt = TimeHelper.Convert(model.SockliningActualFinishDate);
            string sockliningActualStartDateString = "";
            if (sockliningActualStartDateDt != dtDefault)
            {
                sockliningActualStartDateString = String.Format("{0:MM/dd/yyyy}", sockliningActualStartDateDt);
            }
            string sockliningActualFinishDateString = "";
            if (sockliningActualFinishDateDt != dtDefault)
            {
                sockliningActualFinishDateString = String.Format("{0:MM/dd/yyyy}", sockliningActualFinishDateDt);
            }

            var @SockliningActualStartDate = new SqlParameter("@SockliningActualStartDate", sockliningActualStartDateString);
            var @SockliningActualFinishDate = new SqlParameter("@SockliningActualFinishDate", sockliningActualFinishDateString);
            var @InsoleBalance = new SqlParameter("@InsoleBalance", model.InsoleBalance);
            var @InsockBalance = new SqlParameter("@InsockBalance", model.InsockBalance);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSockliningMasterSocklining @ProductNo,@SockliningLine,@SockliningStartDate,@SockliningFinishDate,@SockliningQuota,@SockliningActualStartDate,@SockliningActualFinishDate,@InsoleBalance,@InsockBalance", @ProductNo, @SockliningLine, @SockliningStartDate, @SockliningFinishDate, @SockliningQuota, @SockliningActualStartDate, @SockliningActualFinishDate, @InsoleBalance, @InsockBalance) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Insert_2(SockliningMasterModel model)
        {
            DateTime dtDefault = new DateTime(2000, 01, 01);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @SockliningLine = new SqlParameter("@SockliningLine", model.SockliningLine);
            var @SockliningStartDate = new SqlParameter("@SockliningStartDate", model.SockliningStartDate);
            var @SockliningFinishDate = new SqlParameter("@SockliningFinishDate", model.SockliningFinishDate);
            var @SockliningQuota = new SqlParameter("@SockliningQuota", model.SockliningQuota);

            DateTime sockliningActualStartDateDt = TimeHelper.Convert(model.SockliningActualStartDate);
            DateTime sockliningActualFinishDateDt = TimeHelper.Convert(model.SockliningActualFinishDate);
            string sockliningActualStartDateString = "";
            if (sockliningActualStartDateDt != dtDefault)
            {
                sockliningActualStartDateString = String.Format("{0:MM/dd/yyyy}", sockliningActualStartDateDt);
            }
            string sockliningActualFinishDateString = "";
            if (sockliningActualFinishDateDt != dtDefault)
            {
                sockliningActualFinishDateString = String.Format("{0:MM/dd/yyyy}", sockliningActualFinishDateDt);
            }

            var @SockliningActualStartDate = new SqlParameter("@SockliningActualStartDate", sockliningActualStartDateString);
            var @SockliningActualFinishDate = new SqlParameter("@SockliningActualFinishDate", sockliningActualFinishDateString);

            var @InsoleBalance = new SqlParameter("@InsoleBalance", model.InsoleBalance);
            var @InsockBalance = new SqlParameter("@InsockBalance", model.InsockBalance);

            var @IsSequenceUpdate = new SqlParameter("@IsSequenceUpdate", model.IsSequenceUpdate);
            var @IsSockliningLineUpdate = new SqlParameter("@IsSockliningLineUpdate", model.IsSockliningLineUpdate);
            var @IsSockliningStartDateUpdate = new SqlParameter("@IsSockliningStartDateUpdate", model.IsSockliningStartDateUpdate);
            var @IsSockliningFinishDateUpdate = new SqlParameter("@IsSockliningFinishDateUpdate", model.IsSockliningFinishDateUpdate);
            var @IsSockliningQuotaUpdate = new SqlParameter("@IsSockliningQuotaUpdate", model.IsSockliningQuotaUpdate);
            var @IsSockliningActualStartDateUpdate = new SqlParameter("@IsSockliningActualStartDateUpdate", model.IsSockliningActualStartDateUpdate);
            var @IsSockliningActualFinishDateUpdate = new SqlParameter("@IsSockliningActualFinishDateUpdate", model.IsSockliningActualFinishDateUpdate);
            var @IsInsoleBalanceUpdate = new SqlParameter("@IsInsoleBalanceUpdate", model.IsInsoleBalanceUpdate);
            var @IsInsockBalanceUpdate = new SqlParameter("@IsInsockBalanceUpdate", model.IsInsockBalanceUpdate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSockliningMaster_2 @ProductNo, @Sequence, @SockliningLine, @SockliningStartDate, @SockliningFinishDate, @SockliningQuota, @SockliningActualStartDate, @SockliningActualFinishDate, @InsoleBalance, @InsockBalance, @IsSequenceUpdate, @IsSockliningLineUpdate, @IsSockliningStartDateUpdate, @IsSockliningFinishDateUpdate, @IsSockliningQuotaUpdate, @IsSockliningActualStartDateUpdate, @IsSockliningActualFinishDateUpdate, @IsInsoleBalanceUpdate, @IsInsockBalanceUpdate ",
                                                                          @ProductNo, @Sequence, @SockliningLine, @SockliningStartDate, @SockliningFinishDate, @SockliningQuota, @SockliningActualStartDate, @SockliningActualFinishDate, @InsoleBalance, @InsockBalance, @IsSequenceUpdate, @IsSockliningLineUpdate, @IsSockliningStartDateUpdate, @IsSockliningFinishDateUpdate, @IsSockliningQuotaUpdate, @IsSockliningActualStartDateUpdate, @IsSockliningActualFinishDateUpdate, @IsInsoleBalanceUpdate, @IsInsockBalanceUpdate) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
