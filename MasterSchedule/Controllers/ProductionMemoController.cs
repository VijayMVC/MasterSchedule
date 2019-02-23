using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
using System.Data.SqlClient;
using System.Data;
namespace MasterSchedule.Controllers
{
    class ProductionMemoController
    {
        static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
        public static string Insert(ProductionMemoModel model)
        {
            var @SectionId = new SqlParameter("@SectionId", model.SectionId);
            var @ProductionNumbers = new SqlParameter("@ProductionNumbers", model.ProductionNumbers);

            var @Picture = new SqlParameter("@Picture", SqlDbType.Image);
            @Picture.Value = DBNull.Value;
            if (model.Picture != null)
            {
                @Picture.Value = model.Picture;
            }

            var @Picture1 = new SqlParameter("@Picture1", SqlDbType.Image);
            @Picture1.Value = DBNull.Value;
            if (model.Picture1 != null)
            {
                @Picture1.Value = model.Picture1;
            }

            var @Picture2 = new SqlParameter("@Picture2", SqlDbType.Image);
            @Picture2.Value = DBNull.Value;
            if (model.Picture2 != null)
            {
                @Picture2.Value = model.Picture2;
            }

            var @Picture3 = new SqlParameter("@Picture3", SqlDbType.Image);
            @Picture3.Value = DBNull.Value;
            if (model.Picture3 != null)
            {
                @Picture3.Value = model.Picture3;
            }

            var @Picture4 = new SqlParameter("@Picture4", SqlDbType.Image);
            @Picture4.Value = DBNull.Value;
            if (model.Picture4 != null)
            {
                @Picture4.Value = model.Picture4;
            }

            string memoId = db.ExecuteStoreQuery<string>("spm_InsertProductionMemo_1 @SectionId, @ProductionNumbers, @Picture, @Picture1, @Picture2, @Picture3, @Picture4", @SectionId, @ProductionNumbers, @Picture, @Picture1, @Picture2, @Picture3, @Picture4).FirstOrDefault();
            return memoId;
        }

        public static ProductionMemoModel First(string memoId)
        {
            var @MemoId = new SqlParameter("@MemoId", memoId);
            return db.ExecuteStoreQuery<ProductionMemoModel>("spm_SelectProductionMemoByMemoId @MemoId", @MemoId).FirstOrDefault();
        }

        public static bool Update(ProductionMemoModel model)
        {
            var @MemoId = new SqlParameter("@MemoId", model.MemoId);            
            var @ProductionNumbers = new SqlParameter("@ProductionNumbers", model.ProductionNumbers);

            var @Picture = new SqlParameter("@Picture", SqlDbType.Image);
            @Picture.Value = DBNull.Value;
            if (model.Picture != null)
            {
                @Picture.Value = model.Picture;
            }

            var @Picture1 = new SqlParameter("@Picture1", SqlDbType.Image);
            @Picture1.Value = DBNull.Value;
            if (model.Picture1 != null)
            {
                @Picture1.Value = model.Picture1;
            }

            var @Picture2 = new SqlParameter("@Picture2", SqlDbType.Image);
            @Picture2.Value = DBNull.Value;
            if (model.Picture2 != null)
            {
                @Picture2.Value = model.Picture2;
            }

            var @Picture3 = new SqlParameter("@Picture3", SqlDbType.Image);
            @Picture3.Value = DBNull.Value;
            if (model.Picture3 != null)
            {
                @Picture3.Value = model.Picture3;
            }

            var @Picture4 = new SqlParameter("@Picture4", SqlDbType.Image);
            @Picture4.Value = DBNull.Value;
            if (model.Picture4 != null)
            {
                @Picture4.Value = model.Picture4;
            }

            return db.ExecuteStoreCommand("spm_UpdateProductionMemo_1 @MemoId, @ProductionNumbers, @Picture, @Picture1, @Picture2, @Picture3, @Picture4", @MemoId, @ProductionNumbers, @Picture, @Picture1, @Picture2, @Picture3, @Picture4) > 0;
        }

        public static bool Delete(string memoId)
        {
            var @MemoId = new SqlParameter("@MemoId", memoId);
            return db.ExecuteStoreCommand("spm_DeleteProductionMemo @MemoId", @MemoId) > 0;
        }

        public static List<ProductionMemoModel> Select(string sectionId)
        {
            var @SectionId = new SqlParameter("@SectionId", sectionId);
            return db.ExecuteStoreQuery<ProductionMemoModel>("spm_SelectProductionMemoBySectionId @SectionId", @SectionId).ToList();
        }

        public static List<ProductionMemoModel> Select()
        {
            return db.ExecuteStoreQuery<ProductionMemoModel>("spm_SelectProductionMemo").ToList();
        }

        public static List<ProductionMemoModel> Select(string sectionId, string productionNumber)
        {
            var @ProductionNumbers = new SqlParameter("@ProductionNumbers", productionNumber);
            return db.ExecuteStoreQuery<ProductionMemoModel>("spm_SelectProductionMemoByProductionNumber @ProductionNumbers", @ProductionNumbers).ToList();
        }
    }
}
