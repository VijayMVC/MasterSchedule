using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
using MasterSchedule.Helpers;
namespace MasterSchedule.Controllers
{
    class SewingMasterController
    {
        public static List<SewingMasterModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SewingMasterModel>("EXEC spm_SelectSewingMaster").ToList();
        }

        public static List<SewingMasterModel> SelectAnTuongList()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SewingMasterModel>("EXEC spm_SelectAnTuongList").ToList();
        }

        public static List<SewingMasterModel> SelectByProductNo()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SewingMasterModel>("EXEC spm_SelectSewingLineByProductNo").ToList();
        }

        public static List<SewingMasterModel> SelectCutAStartDate()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<SewingMasterModel>("EXEC spm_SelectSewingMasterCutAStartDate").ToList();
        }

        public static bool Insert(SewingMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @SewingLine = new SqlParameter("@SewingLine", model.SewingLine);
            var @SewingQuota = new SqlParameter("@SewingQuota", model.SewingQuota);
            var @SewingActualStartDate = new SqlParameter("@SewingActualStartDate", model.SewingActualStartDate);
            var @SewingActualFinishDate = new SqlParameter("@SewingActualFinishDate", model.SewingActualFinishDate);
            var @SewingBalance = new SqlParameter("@SewingBalance", model.SewingBalance);
            var @CutAQuota = new SqlParameter("@CutAQuota", model.CutAQuota);
            var @CutAActualStartDate = new SqlParameter("@CutAActualStartDate", model.CutAActualStartDate);
            var @CutAActualFinishDate = new SqlParameter("@CutAActualFinishDate", model.CutAActualFinishDate);
            var @CutABalance = new SqlParameter("@CutABalance", model.CutABalance);
            var @PrintingBalance = new SqlParameter("@PrintingBalance", model.PrintingBalance);
            var @H_FBalance = new SqlParameter("@H_FBalance", model.H_FBalance);
            var @EmbroideryBalance = new SqlParameter("@EmbroideryBalance", model.EmbroideryBalance);
            var @CutBBalance = new SqlParameter("@CutBBalance", model.CutBBalance);
            var @AutoCut = new SqlParameter("@AutoCut", model.AutoCut);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSewingMaster @ProductNo,@Sequence,@SewingLine,@SewingQuota,@SewingActualStartDate,@SewingActualFinishDate,@SewingBalance,@CutAQuota,@CutAActualStartDate,@CutAActualFinishDate,@CutABalance,@PrintingBalance,@H_FBalance,@EmbroideryBalance,@CutBBalance,@AutoCut", @ProductNo, @Sequence, @SewingLine, @SewingQuota, @SewingActualStartDate, @SewingActualFinishDate, @SewingBalance, @CutAQuota, @CutAActualStartDate, @CutAActualFinishDate, @CutABalance, @PrintingBalance, @H_FBalance, @EmbroideryBalance, @CutBBalance, @AutoCut) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Insert_2(SewingMasterModel model)
        {
            DateTime dtDefault = new DateTime(2000, 01, 01);
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @SewingLine = new SqlParameter("@SewingLine", model.SewingLine);
            var @SewingStartDate = new SqlParameter("@SewingStartDate", model.SewingStartDate);
            var @SewingFinishDate = new SqlParameter("@SewingFinishDate", model.SewingFinishDate);
            var @SewingQuota = new SqlParameter("@SewingQuota", model.SewingQuota);

            var @SewingPrep = new SqlParameter("@SewingPrep", model.SewingPrep);

            var @SewingActualStartDate = new SqlParameter("@SewingActualStartDate", model.SewingActualStartDate);
            var @SewingActualFinishDate = new SqlParameter("@SewingActualFinishDate", model.SewingActualFinishDate);

            DateTime sewingActualStartDateAutoDt = TimeHelper.Convert(model.SewingActualStartDateAuto);
            DateTime sewingActualFinishDateAutoDt = TimeHelper.Convert(model.SewingActualFinishDateAuto);
            string sewingActualStartDateAutoString = "";
            if (sewingActualStartDateAutoDt != dtDefault)
            { 
                sewingActualStartDateAutoString = String.Format("{0:MM/dd/yyyy}", sewingActualStartDateAutoDt); 
            }
            string sewingActualFinishDateAutoString = "";
            if (sewingActualFinishDateAutoDt != dtDefault)
            {
                sewingActualFinishDateAutoString = String.Format("{0:MM/dd/yyyy}", sewingActualFinishDateAutoDt);
            }

            var @SewingActualStartDateAuto = new SqlParameter("@SewingActualStartDateAuto", sewingActualStartDateAutoString);
            var @SewingActualFinishDateAuto = new SqlParameter("@SewingActualFinishDateAuto", sewingActualFinishDateAutoString);

            var @SewingBalance = new SqlParameter("@SewingBalance", model.SewingBalance);

            var @CutAStartDate = new SqlParameter("@CutAStartDate", model.CutAStartDate);
            var @CutAFinishDate = new SqlParameter("@CutAFinishDate", model.CutAFinishDate);
            var @CutAQuota = new SqlParameter("@CutAQuota", model.CutAQuota);

            DateTime cutAActualStartDateDt = TimeHelper.Convert(model.CutAActualStartDate);
            DateTime cutAActualFinishDateDt = TimeHelper.Convert(model.CutAActualFinishDate);
            string cutAActualStartDateString = "";
            if (cutAActualStartDateDt != dtDefault)
            {
                cutAActualStartDateString = String.Format("{0:MM/dd/yyyy}", cutAActualStartDateDt);
            }
            string cutAActualFinishDateString = "";
            if (cutAActualFinishDateDt != dtDefault)
            {
                cutAActualFinishDateString = String.Format("{0:MM/dd/yyyy}", cutAActualFinishDateDt);
            }
            var @CutAActualStartDate = new SqlParameter("@CutAActualStartDate", cutAActualStartDateString);
            var @CutAActualFinishDate = new SqlParameter("@CutAActualFinishDate", cutAActualFinishDateString);

            var @CutABalance = new SqlParameter("@CutABalance", model.CutABalance);
            var @PrintingBalance = new SqlParameter("@PrintingBalance", model.PrintingBalance);
            var @H_FBalance = new SqlParameter("@H_FBalance", model.H_FBalance);
            var @EmbroideryBalance = new SqlParameter("@EmbroideryBalance", model.EmbroideryBalance);
            DateTime cutBActualStartDateDt = TimeHelper.Convert(model.CutBActualStartDate);
            string cutBActualStartDateString = "";
            if (cutBActualStartDateDt != dtDefault)
            {
                cutBActualStartDateString = String.Format("{0:MM/dd/yyyy}", cutBActualStartDateDt);
            }
            var @CutBActualStartDate = new SqlParameter("@CutBActualStartDate", cutBActualStartDateString);

            var @CutBBalance = new SqlParameter("@CutBBalance", model.CutBBalance);
            var @AutoCut = new SqlParameter("@AutoCut", model.AutoCut);
            var @LaserCut = new SqlParameter("@LaserCut", model.LaserCut);
            var @HuasenCut = new SqlParameter("@HuasenCut", model.HuasenCut);

            var @IsSequenceUpdate = new SqlParameter("@IsSequenceUpdate", model.IsSequenceUpdate);
            var @IsSewingLineUpdate = new SqlParameter("@IsSewingLineUpdate", model.IsSewingLineUpdate);
            var @IsSewingStartDateUpdate = new SqlParameter("@IsSewingStartDateUpdate", model.IsSewingStartDateUpdate);
            var @IsSewingFinishDateUpdate = new SqlParameter("@IsSewingFinishDateUpdate", model.IsSewingFinishDateUpdate);
            var @IsSewingQuotaUpdate = new SqlParameter("@IsSewingQuotaUpdate", model.IsSewingQuotaUpdate);

            var @IsSewingPrepUpdate = new SqlParameter("@IsSewingPrepUpdate", model.IsSewingPrepUpdate);

            var @IsSewingActualStartDateUpdate = new SqlParameter("@IsSewingActualStartDateUpdate", model.IsSewingActualStartDateUpdate);
            var @IsSewingActualFinishDateUpdate = new SqlParameter("@IsSewingActualFinishDateUpdate", model.IsSewingActualFinishDateUpdate);

            var @IsSewingActualStartDateAutoUpdate = new SqlParameter("@IsSewingActualStartDateAutoUpdate", model.IsSewingActualStartDateAutoUpdate);
            var @IsSewingActualFinishDateAutoUpdate = new SqlParameter("@IsSewingActualFinishDateAutoUpdate", model.IsSewingActualFinishDateAutoUpdate);

            var @IsSewingBalanceUpdate = new SqlParameter("@IsSewingBalanceUpdate", model.IsSewingBalanceUpdate);
            var @IsCutAStartDateUpdate = new SqlParameter("@IsCutAStartDateUpdate", model.IsCutAStartDateUpdate);
            var @IsCutAFinishDateUpdate = new SqlParameter("@IsCutAFinishDateUpdate", model.IsCutAFinishDateUpdate);
            var @IsCutAQuotaUpdate = new SqlParameter("@IsCutAQuotaUpdate", model.IsCutAQuotaUpdate);
            var @IsCutAActualStartDateUpdate = new SqlParameter("@IsCutAActualStartDateUpdate", model.IsCutAActualStartDateUpdate);
            var @IsCutAActualFinishDateUpdate = new SqlParameter("@IsCutAActualFinishDateUpdate", model.IsCutAActualFinishDateUpdate);
            var @IsCutABalanceUpdate = new SqlParameter("@IsCutABalanceUpdate", model.IsCutABalanceUpdate);
            var @IsPrintingBalanceUpdate = new SqlParameter("@IsPrintingBalanceUpdate", model.IsPrintingBalanceUpdate);
            var @IsH_FBalanceUpdate = new SqlParameter("@IsH_FBalanceUpdate", model.IsH_FBalanceUpdate);
            var @IsEmbroideryBalanceUpdate = new SqlParameter("@IsEmbroideryBalanceUpdate", model.IsEmbroideryBalanceUpdate);

            var @IsCutBActualStartDateUpdate = new SqlParameter("@IsCutBActualStartDateUpdate", model.IsCutBActualStartDateUpdate);
            var @IsCutBBalanceUpdate = new SqlParameter("@IsCutBBalanceUpdate", model.IsCutBBalanceUpdate);
            var @IsAutoCutUpdate = new SqlParameter("@IsAutoCutUpdate", model.IsAutoCutUpdate);
            var @IsLaserCutUpdate = new SqlParameter("@IsLaserCutUpdate", model.IsLaserCutUpdate);
            var @IsHuasenCutUpdate = new SqlParameter("@IsHuasenCutUpdate", model.IsHuasenCutUpdate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSewingMaster_5  @ProductNo, @Sequence, @SewingLine, @SewingStartDate, @SewingFinishDate, @SewingQuota, @SewingPrep, @SewingActualStartDate, @SewingActualFinishDate, @SewingActualStartDateAuto, @SewingActualFinishDateAuto, @SewingBalance, @CutAStartDate, @CutAFinishDate, @CutAQuota, @CutAActualStartDate, @CutAActualFinishDate, @CutABalance, @PrintingBalance, @H_FBalance, @EmbroideryBalance, @CutBActualStartDate, @CutBBalance, @AutoCut, @LaserCut, @HuasenCut, @IsSequenceUpdate, @IsSewingLineUpdate, @IsSewingStartDateUpdate, @IsSewingFinishDateUpdate, @IsSewingQuotaUpdate, @IsSewingPrepUpdate, @IsSewingActualStartDateUpdate, @IsSewingActualFinishDateUpdate, @IsSewingActualStartDateAutoUpdate, @IsSewingActualFinishDateAutoUpdate, @IsSewingBalanceUpdate, @IsCutAStartDateUpdate, @IsCutAFinishDateUpdate, @IsCutAQuotaUpdate, @IsCutAActualStartDateUpdate, @IsCutAActualFinishDateUpdate, @IsCutABalanceUpdate, @IsPrintingBalanceUpdate, @IsH_FBalanceUpdate, @IsEmbroideryBalanceUpdate, @IsCutBActualStartDateUpdate, @IsCutBBalanceUpdate, @IsAutoCutUpdate, @IsLaserCutUpdate, @IsHuasenCutUpdate",
                                                                       @ProductNo, @Sequence, @SewingLine, @SewingStartDate, @SewingFinishDate, @SewingQuota, @SewingPrep, @SewingActualStartDate, @SewingActualFinishDate, @SewingActualStartDateAuto, @SewingActualFinishDateAuto, @SewingBalance, @CutAStartDate, @CutAFinishDate, @CutAQuota, @CutAActualStartDate, @CutAActualFinishDate, @CutABalance, @PrintingBalance, @H_FBalance, @EmbroideryBalance, @CutBActualStartDate, @CutBBalance, @AutoCut, @LaserCut, @HuasenCut, @IsSequenceUpdate, @IsSewingLineUpdate, @IsSewingStartDateUpdate, @IsSewingFinishDateUpdate, @IsSewingQuotaUpdate, @IsSewingPrepUpdate, @IsSewingActualStartDateUpdate, @IsSewingActualFinishDateUpdate, @IsSewingActualStartDateAutoUpdate, @IsSewingActualFinishDateAutoUpdate, @IsSewingBalanceUpdate, @IsCutAStartDateUpdate, @IsCutAFinishDateUpdate, @IsCutAQuotaUpdate, @IsCutAActualStartDateUpdate, @IsCutAActualFinishDateUpdate, @IsCutABalanceUpdate, @IsPrintingBalanceUpdate, @IsH_FBalanceUpdate, @IsEmbroideryBalanceUpdate, @IsCutBActualStartDateUpdate, @IsCutBBalanceUpdate, @IsAutoCutUpdate, @IsLaserCutUpdate, @IsHuasenCutUpdate) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool InsertSequence(SewingMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Sequence = new SqlParameter("@Sequence", model.Sequence);
            var @SewingStartDate = new SqlParameter("@SewingStartDate", model.SewingStartDate);
            var @SewingFinishDate = new SqlParameter("@SewingFinishDate", model.SewingFinishDate);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSewingMasterSequence @ProductNo,@Sequence,@SewingStartDate,@SewingFinishDate", @ProductNo, @Sequence, @SewingStartDate, @SewingFinishDate) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool InsertSewing(SewingMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @SewingLine = new SqlParameter("@SewingLine", model.SewingLine);
            var @SewingStartDate = new SqlParameter("@SewingStartDate", model.SewingStartDate);
            var @SewingFinishDate = new SqlParameter("@SewingFinishDate", model.SewingFinishDate);
            var @SewingQuota = new SqlParameter("@SewingQuota", model.SewingQuota);
            var @SewingActualStartDate = new SqlParameter("@SewingActualStartDate", model.SewingActualStartDate);
            var @SewingActualFinishDate = new SqlParameter("@SewingActualFinishDate", model.SewingActualFinishDate);
            var @SewingBalance = new SqlParameter("@SewingBalance", model.SewingBalance);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSewingMasterSewing @ProductNo,@SewingLine,@SewingStartDate,@SewingFinishDate,@SewingQuota,@SewingActualStartDate,@SewingActualFinishDate,@SewingBalance", @ProductNo, @SewingLine, @SewingStartDate, @SewingFinishDate, @SewingQuota, @SewingActualStartDate, @SewingActualFinishDate, @SewingBalance) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool InsertCutPrep(SewingMasterModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @CutAStartDate = new SqlParameter("@CutAStartDate", model.CutAStartDate);
            var @CutAFinishDate = new SqlParameter("@CutAFinishDate", model.CutAFinishDate);
            var @CutAQuota = new SqlParameter("@CutAQuota", model.CutAQuota);
            var @CutAActualStartDate = new SqlParameter("@CutAActualStartDate", model.CutAActualStartDate);
            var @CutAActualFinishDate = new SqlParameter("@CutAActualFinishDate", model.CutAActualFinishDate);
            var @CutABalance = new SqlParameter("@CutABalance", model.CutABalance);
            var @PrintingBalance = new SqlParameter("@PrintingBalance", model.PrintingBalance);
            var @H_FBalance = new SqlParameter("@H_FBalance", model.H_FBalance);
            var @EmbroideryBalance = new SqlParameter("@EmbroideryBalance", model.EmbroideryBalance);
            var @CutBBalance = new SqlParameter("@CutBBalance", model.CutBBalance);
            var @AutoCut = new SqlParameter("@AutoCut", model.AutoCut);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertSewingMasterCutPrep @ProductNo, @CutAStartDate, @CutAFinishDate, @CutAQuota, @CutAActualStartDate, @CutAActualFinishDate, @CutABalance, @PrintingBalance, @H_FBalance, @EmbroideryBalance, @CutBBalance, @AutoCut", @ProductNo, @CutAStartDate, @CutAFinishDate, @CutAQuota, @CutAActualStartDate, @CutAActualFinishDate, @CutABalance, @PrintingBalance, @H_FBalance, @EmbroideryBalance, @CutBBalance, @AutoCut) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
