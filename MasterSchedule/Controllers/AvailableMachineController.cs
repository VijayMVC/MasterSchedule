using System.Collections.Generic;
using System.Linq;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class AvailableMachineController
    {
        public static AvailableMachineModel Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<AvailableMachineModel>("EXEC spm_SelectAvailableMachine").FirstOrDefault();
        }

        public static List<AvailableWorker> SelectWorker(int _year, int _month, int _day)
        {
            var @Year = new SqlParameter("@Year", _year);
            var @Month = new SqlParameter("@Month", _month);
            var @Day = new SqlParameter("@Day", _day);
            
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<AvailableWorker>("EXEC spm_SelectWorkerByDate @Year, @Month, @Day", @Year, @Month, @Day).ToList();
        }

        public static AvailableMachineModel SelectTop1(string IdMachineAvailable)
        {
            var @Id = new SqlParameter("@Id", IdMachineAvailable);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<AvailableMachineModel>("EXEC spm_SelectAvailableMachineTop1 @Id", @Id).FirstOrDefault();
        }

        public static bool Insert(AvailableMachineModel model)
        {
            var @Id = new SqlParameter("@Id", model.Id);

            // Cutting
            var @CuttingArmClicker = new SqlParameter("@CuttingArmClicker", model.CuttingArmClicker);
            var @CuttingBeam = new SqlParameter("@CuttingBeam", model.CuttingBeam);
            var @CuttingCutStrap = new SqlParameter("@CuttingCutStrap", model.CuttingCutStrap);
            var @CuttingLaser = new SqlParameter("@CuttingLaser", model.CuttingLaser);
            var @CuttingPuncherHole = new SqlParameter("@CuttingPuncherHole", model.CuttingPuncherHole);
            var @CuttingSkiving = new SqlParameter("@CuttingSkiving", model.CuttingSkiving);

            // Prep
            var @PrepVerticalHF = new SqlParameter("@PrepVerticalHF", model.PrepVerticalHF);
            var @PrepHorizontalHF = new SqlParameter("@PrepHorizontalHF", model.PrepHorizontalHF);
            var @PrepOnlineHeatPress = new SqlParameter("@PrepOnlineHeatPress", model.PrepOnlineHeatPress);
            var @PrepAutoHF = new SqlParameter("@PrepAutoHF", model.PrepAutoHF);
            var @PrepInye = new SqlParameter("@PrepInye", model.PrepInye);
            var @PrepHotmeltMachine = new SqlParameter("@PrepHotmeltMachine", model.PrepHotmeltMachine);

            // Sewing
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
            var @StockfitVerticalBuffing = new SqlParameter("@StockfitVerticalBuffing", model.StockfitVerticalBuffing);
            var @StockfitHorizontalBuffing = new SqlParameter("@StockfitHorizontalBuffing", model.StockfitHorizontalBuffing);
            var @StockfitSideBuffing = new SqlParameter("@StockfitSideBuffing", model.StockfitSideBuffing);
            var @StockfitOutsoleStitching = new SqlParameter("@StockfitOutsoleStitching", model.StockfitOutsoleStitching);
            var @StockfitAutoBuffing = new SqlParameter("@StockfitAutoBuffing", model.StockfitAutoBuffing);
            var @StockfitHydraulicCutting = new SqlParameter("@StockfitHydraulicCutting", model.StockfitHydraulicCutting);
            var @StockfitPadPrinting = new SqlParameter("@StockfitPadPrinting", model.StockfitPadPrinting);

            // Assembly
            var @AssemblyToeLasting = new SqlParameter("@AssemblyToeLasting", model.AssemblyToeLasting);
            var @AssemblySideLasting = new SqlParameter("@AssemblySideLasting", model.AssemblySideLasting);
            var @AssemblyHeelLasting = new SqlParameter("@AssemblyHeelLasting", model.AssemblyHeelLasting);
            var @AssemblySidePress = new SqlParameter("@AssemblySidePress", model.AssemblySidePress);
            var @AssemblyTopDown = new SqlParameter("@AssemblyTopDown", model.AssemblyTopDown);
            var @AssemblyHotmeltMachine = new SqlParameter("@AssemblyHotmeltMachine", model.AssemblyHotmeltMachine);
            var @AssemblySocklinerHotmelt = new SqlParameter("@AssemblySocklinerHotmelt", model.AssemblySocklinerHotmelt);
            var @AssemblyVWrinkleRemover = new SqlParameter("@AssemblyVWrinkleRemover", model.AssemblyVWrinkleRemover);

            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand(@"EXEC spm_InsertAvailableMachine @Id, 
                                            @CuttingArmClicker,@CuttingBeam,@CuttingCutStrap,@CuttingLaser,@CuttingPuncherHole,@CuttingSkiving,
                                            @PrepVerticalHF,@PrepHorizontalHF,@PrepOnlineHeatPress,@PrepAutoHF,@PrepInye,@PrepHotmeltMachine,
                                            @SewingSmallComputer,@SewingBigComputer,@SewingUltrasonic,@Sewing4NeedleFlat,@Sewing4NeedlePost, 
                                            @SewingLongTable,@SewingEyeleting,@SewingZZBinding,@SewingHotmeltMachine,@SewingHandHeldHotmelt,@SewingStationaryHHHotmelt,
                                            @StockfitVerticalBuffing,@StockfitHorizontalBuffing,@StockfitSideBuffing,@StockfitOutsoleStitching,
                                            @StockfitAutoBuffing,@StockfitHydraulicCutting,@StockfitPadPrinting,
                                            @AssemblyToeLasting,@AssemblySideLasting,@AssemblyHeelLasting,@AssemblySidePress,
                                            @AssemblyTopDown,@AssemblyHotmeltMachine,@AssemblySocklinerHotmelt,@AssemblyVWrinkleRemover",
                @Id,
                @CuttingArmClicker, @CuttingBeam, @CuttingCutStrap, @CuttingLaser, @CuttingPuncherHole, @CuttingSkiving,
                @PrepVerticalHF, @PrepHorizontalHF, @PrepOnlineHeatPress, @PrepAutoHF, @PrepInye, @PrepHotmeltMachine,
                @SewingSmallComputer, @SewingBigComputer, @SewingUltrasonic, @Sewing4NeedleFlat, @Sewing4NeedlePost, @SewingLongTable, @SewingEyeleting, @SewingZZBinding, @SewingHotmeltMachine, @SewingHandHeldHotmelt, @SewingStationaryHHHotmelt,
                @StockfitVerticalBuffing, @StockfitHorizontalBuffing, @StockfitSideBuffing, @StockfitOutsoleStitching, @StockfitAutoBuffing, @StockfitHydraulicCutting, @StockfitPadPrinting,
                @AssemblyToeLasting, @AssemblySideLasting, @AssemblyHeelLasting, @AssemblySidePress, @AssemblyTopDown, @AssemblyHotmeltMachine, @AssemblySocklinerHotmelt, @AssemblyVWrinkleRemover) > 0)
            {
                return true;
            }

            return false;
        }

        public static bool Delete(string IdAvailableMachine)
        {
            var @Id = new SqlParameter("@Id", IdAvailableMachine);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            if (db.ExecuteStoreCommand("EXEC spm_DeleteAvailableMachine @Id", @Id) > 0)
            {
                return true;
            }
            return false;
        }
    }

}
