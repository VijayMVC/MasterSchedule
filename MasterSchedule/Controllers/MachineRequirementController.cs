using System.Collections.Generic;
using System.Linq;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class MachineRequirementController
    {
        #region WrongCode
        public static List<MachineRequirementCuttingViewModel> SelectCutting()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MachineRequirementCuttingViewModel>("EXEC spm_SelectMachineRequirementCutting").ToList();
        }
        public static List<MachineRequirementPrepViewModel> SelectPrep()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MachineRequirementPrepViewModel>("EXEC spm_SelectMachineRequirementPrep").ToList();
        } 
        public static List<MachineRequirementSewingViewModel> SelectSewing()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MachineRequirementSewingViewModel>("EXEC spm_SelectMachineRequirementSewing").ToList();
        }
        public static List<MachineRequirementStockfitViewModel> SelectStockfit()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MachineRequirementStockfitViewModel>("EXEC spm_SelectMachineRequirementStockfit").ToList();
        }
        public static List<MachineRequirementAssemblyViewModel> SelectAssembly()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MachineRequirementAssemblyViewModel>("EXEC spm_SelectMachineRequirementAssembly").ToList();
        }
        #endregion
        public static List<MachineRequirementModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MachineRequirementModel>("EXEC spm_SelectMachineRequirement").ToList();
        }

        public static MachineRequirementModel SelectTop1(string articleNo)
        {
            var @ArticleNo = new SqlParameter("@ArticleNo", articleNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MachineRequirementModel>("EXEC spm_SelectMachineRequirementTop1 @ArticleNo", @ArticleNo).FirstOrDefault();
        }

        public static bool Insert(MachineRequirementModel model)
        {

            var @ArticleNo = new SqlParameter("@ArticleNo", model.ArticleNo);
            var @ShoeName = new SqlParameter("@ShoeName", model.ShoeName);
            var @PM = new SqlParameter("@PM", model.PM);
            // Cutting
            var @CuttingQuantity = new SqlParameter("@CuttingQuantity", model.CuttingQuantity);
            var @CuttingWorker = new SqlParameter("@CuttingWorker", model.CuttingWorker);
            var @CuttingArmClicker = new SqlParameter("@CuttingArmClicker", model.CuttingArmClicker);
            var @CuttingBeam = new SqlParameter("@CuttingBeam", model.CuttingBeam);
            var @CuttingCutStrap = new SqlParameter("@CuttingCutStrap", model.CuttingCutStrap);
            var @CuttingLaser = new SqlParameter("@CuttingLaser",model.CuttingLaser);
            var @CuttingPuncherHole = new SqlParameter("@CuttingPuncherHole", model.CuttingPuncherHole);
            var @CuttingSkiving = new SqlParameter("@CuttingSkiving", model.CuttingSkiving);

            // Prep
            var @PrepWorker = new SqlParameter("@PrepWorker", model.PrepWorker);
            var @PrepVerticalHF = new SqlParameter("@PrepVerticalHF", model.PrepVerticalHF);
            var @PrepHorizontalHF = new SqlParameter("@PrepHorizontalHF", model.PrepHorizontalHF);
            var @PrepOnlineHeatPress = new SqlParameter("@PrepOnlineHeatPress", model.PrepOnlineHeatPress);
            var @PrepAutoHF = new SqlParameter("@PrepAutoHF", model.PrepAutoHF);
            var @PrepInye = new SqlParameter("@PrepInye", model.PrepInye);
            var @PrepHotmeltMachine = new SqlParameter("@PrepHotmeltMachine", model.PrepHotmeltMachine);
            // Sewing
            var @SewingQuantity = new SqlParameter("@SewingQuantity", model.SewingQuantity);
            var @SewingWorker = new SqlParameter("@SewingWorker", model.SewingWorker);
            var @SewingSmallComputer = new SqlParameter("@SewingSmallComputer", model.SewingSmallComputer);
            var @SewingBigComputer = new SqlParameter("@SewingBigComputer", model.SewingBigComputer);
            var @SewingUltrasonic = new SqlParameter("@SewingUltrasonic", model.SewingUltrasonic);
            var @Sewing4NeedleFlat = new SqlParameter("@Sewing4NeedleFlat", model.Sewing4NeedleFlat);
            var @Sewing4NeedlePost = new SqlParameter("@Sewing4NeedlePost", model.Sewing4NeedlePost);
            var @SewingLongTable = new SqlParameter("@SewingLongTable", model.SewingLongTable);
            var @SewingEyeleting = new SqlParameter("@SewingEyeleting", model.SewingEyeleting);
            var @SewingZZBinding = new SqlParameter("@SewingZZBinding", model.SewingZZBinding);
            var @SewingHotmeltMachine = new SqlParameter("@SewingHotmeltMachine", model.SewingHotmeltMachine);
            var @SewingHandHeldHotmelt = new SqlParameter("@SewingHandHeldHotmelt", model.SewingHandHeldHotmelt);
            var @SewingStationaryHHHotmelt = new SqlParameter("@SewingStationaryHHHotmelt", model.SewingStationaryHHHotmelt);
            // Stockfit
            var @StockfitQuantity = new SqlParameter("@StockfitQuantity", model.StockfitQuantity);
            var @StockfitWorker = new SqlParameter("@StockfitWorker", model.StockfitWorker);
            var @StockfitVerticalBuffing = new SqlParameter("@StockfitVerticalBuffing", model.StockfitVerticalBuffing);
            var @StockfitHorizontalBuffing = new SqlParameter("@StockfitHorizontalBuffing", model.StockfitHorizontalBuffing);
            var @StockfitSideBuffing = new SqlParameter("@StockfitSideBuffing", model.StockfitSideBuffing);
            var @StockfitOutsoleStitching = new SqlParameter("@StockfitOutsoleStitching", model.StockfitOutsoleStitching);
            var @StockfitAutoBuffing = new SqlParameter("@StockfitAutoBuffing", model.StockfitAutoBuffing);
            var @StockfitHydraulicCutting = new SqlParameter("@StockfitHydraulicCutting", model.StockfitHydraulicCutting);
            var @StockfitPadPrinting = new SqlParameter("@StockfitPadPrinting", model.StockfitPadPrinting);
            // Assembly
            var @AssemblyQuantity = new SqlParameter("@AssemblyQuantity", model.AssemblyQuantity);
            var @AssemblyWorker = new SqlParameter("@AssemblyWorker", model.AssemblyWorker);
            var @AssemblyToeLasting = new SqlParameter("@AssemblyToeLasting", model.AssemblyToeLasting);
            var @AssemblySideLasting = new SqlParameter("@AssemblySideLasting", model.AssemblySideLasting);
            var @AssemblyHeelLasting = new SqlParameter("@AssemblyHeelLasting", model.AssemblyHeelLasting);
            var @AssemblySidePress = new SqlParameter("@AssemblySidePress", model.AssemblySidePress);
            var @AssemblyTopDown = new SqlParameter("@AssemblyTopDown", model.AssemblyTopDown);
            var @AssemblyHotmeltMachine = new SqlParameter("@AssemblyHotmeltMachine", model.AssemblyHotmeltMachine);
            var @AssemblySocklinerHotmelt = new SqlParameter("@AssemblySocklinerHotmelt", model.AssemblySocklinerHotmelt);
            var @AssemblyVWrinkleRemover = new SqlParameter("@AssemblyVWrinkleRemover", model.AssemblyVWrinkleRemover);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand(@"EXEC spm_InsertMachineRequirement @ArticleNo,@ShoeName,@PM, 
                                            @CuttingQuantity,@CuttingWorker,@CuttingArmClicker,@CuttingBeam,@CuttingCutStrap,@CuttingLaser,@CuttingPuncherHole,@CuttingSkiving,
                                            @PrepWorker,@PrepVerticalHF,@PrepHorizontalHF,@PrepOnlineHeatPress,@PrepAutoHF,@PrepInye,@PrepHotmeltMachine,
                                            @SewingQuantity,@SewingWorker,@SewingSmallComputer,@SewingBigComputer,@SewingUltrasonic,@Sewing4NeedleFlat,@Sewing4NeedlePost, 
                                            @SewingLongTable,@SewingEyeleting,@SewingZZBinding,@SewingHotmeltMachine,@SewingHandHeldHotmelt,@SewingStationaryHHHotmelt,
                                            @StockfitQuantity,@StockfitWorker,@StockfitVerticalBuffing,@StockfitHorizontalBuffing,@StockfitSideBuffing,@StockfitOutsoleStitching,
                                            @StockfitAutoBuffing,@StockfitHydraulicCutting,@StockfitPadPrinting,
                                            @AssemblyQuantity,@AssemblyWorker,@AssemblyToeLasting,@AssemblySideLasting,@AssemblyHeelLasting,@AssemblySidePress,
                                            @AssemblyTopDown,@AssemblyHotmeltMachine,@AssemblySocklinerHotmelt,@AssemblyVWrinkleRemover", 
                @ArticleNo, @ShoeName, @PM, 
                @CuttingQuantity, @CuttingWorker, @CuttingArmClicker, @CuttingBeam, @CuttingCutStrap, @CuttingLaser,@CuttingPuncherHole, @CuttingSkiving,
                @PrepWorker, @PrepVerticalHF, @PrepHorizontalHF, @PrepOnlineHeatPress, @PrepAutoHF, @PrepInye, @PrepHotmeltMachine, 
                @SewingQuantity, @SewingWorker, @SewingSmallComputer, @SewingBigComputer, @SewingUltrasonic, @Sewing4NeedleFlat, @Sewing4NeedlePost, @SewingLongTable, @SewingEyeleting, @SewingZZBinding, @SewingHotmeltMachine, @SewingHandHeldHotmelt, @SewingStationaryHHHotmelt,
                @StockfitQuantity, @StockfitWorker, @StockfitVerticalBuffing, @StockfitHorizontalBuffing, @StockfitSideBuffing, @StockfitOutsoleStitching, @StockfitAutoBuffing, @StockfitHydraulicCutting, @StockfitPadPrinting,
                @AssemblyQuantity, @AssemblyWorker, @AssemblyToeLasting, @AssemblySideLasting, @AssemblyHeelLasting, @AssemblySidePress, @AssemblyTopDown, @AssemblyHotmeltMachine, @AssemblySocklinerHotmelt, @AssemblyVWrinkleRemover) > 0)
            {
                return true;
            }

            return false;
        }

        public static bool Delete(string articleNo)
        {
            var @ArticleNo = new SqlParameter("@ArticleNo", articleNo);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_DeleteMachineRequirement @ArticleNo", @ArticleNo) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
