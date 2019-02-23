using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using Microsoft.Win32;
using System.Windows.Forms;

namespace MasterSchedule.Helpers
{
    public class ExcelHelper
    {
        public static void ExportExcel(string fileName, string sheetName, DataTable dt)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {
                worksheet = workbook.ActiveSheet;
                worksheet.Name = sheetName;

                //worksheet.Cells.AutoFit();
                worksheet.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Cells.Font.Name = "Calibri";
                worksheet.Cells.Font.Size = 11;
                worksheet.Cells.Rows[1].Font.Size = 14;
                worksheet.Cells.Rows[1].Font.FontStyle = "Bold";

                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    var dataRow = dt.Rows[i] as DataRow;
                    for (int j = 0; j < dataRow.ItemArray.Count(); j++)
                    {
                        worksheet.Cells[i, j] = dataRow.ItemArray[j].ToString();
                    }
                }

                var sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xlsx";
                sfd.FileName = fileName;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(sfd.FileName);
                    MessageBox.Show("Successful !", "Master Schedule Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Master Schedule Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }
    }
}
