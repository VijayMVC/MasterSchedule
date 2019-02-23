using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using MasterSchedule.Helpers;
using System.Diagnostics;
using System.Windows;
using System.IO;
namespace MasterSchedule.Customs
{
    public class CustomDataGrid : DataGrid
    {
        static CustomDataGrid()
        {
            CommandManager.RegisterClassCommandBinding(
                typeof(CustomDataGrid),
                new CommandBinding(ApplicationCommands.Paste,
                    new ExecutedRoutedEventHandler(OnExecutedPaste),
                    new CanExecuteRoutedEventHandler(OnCanExecutePaste)));
        }

        private static void OnCanExecutePaste(object target, CanExecuteRoutedEventArgs args)
        {
            ((CustomDataGrid)target).OnCanExecutePaste(args);
        }

        /// <summary>
        /// This virtual method is called when ApplicationCommands. Paste command query its state.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCanExecutePaste(CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = CurrentCell != null;
            args.Handled = true;
        }

        private static void OnExecutedPaste(object target, ExecutedRoutedEventArgs args)
        {
            ((CustomDataGrid)target).OnExecutedPaste(args);
        }

        /// <summary>
        /// This virtual method is called when ApplicationCommands. Paste command is executed.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnExecutedPaste(ExecutedRoutedEventArgs args)
        {
            Debug.WriteLine("The Events '<OnExecutedPaste>' Begin...");
            // Parse the clipboard data.            
            List<string[]> rowData = ClipboardHelper.ParseClipboardData();
            if (rowData == null)
            {
                return;
            }
            // Call OnPastingCellClipboardContent for each cell.
            int minRowIndex = Items.IndexOf(CurrentItem);
            int maxRowIndex = Items.Count - 1;
            int minColumnDisplayIndex = (SelectionUnit != DataGridSelectionUnit.FullRow) ? Columns.IndexOf(CurrentColumn) : 0;
            int maxColumnDisplayIndex = Columns.Count - 1;
            int rowDataIndex = 0;            
            for (int i = minRowIndex; i <= maxRowIndex && rowDataIndex < rowData.Count; i++, rowDataIndex++)
            {
                int columnDataIndex = 0;
                for (int j = minColumnDisplayIndex; j <= maxColumnDisplayIndex && columnDataIndex < rowData[rowDataIndex].Length; j++, columnDataIndex++)
                {
                    DataGridColumn column = ColumnFromDisplayIndex(j);
                    column.OnPastingCellClipboardContent(Items[i], rowData[rowDataIndex][columnDataIndex]);
                }
            }
        }
    }

    public static class ClipboardHelper
    {
        public delegate string[] ParseFormat(string value);

        public static List<string[]> ParseClipboardData()
        {
            List<string[]> clipboardData = null;
            object clipboardRawData = null;
            ParseFormat parseFormat = null;
            // Get the data and set the parsing method based on the format
            // currently works with CSV and Text DataFormats.           
            IDataObject dataObj = Clipboard.GetDataObject();
            if ((clipboardRawData = dataObj.GetData(DataFormats.CommaSeparatedValue)) != null)
            {
                parseFormat = ParseCsvFormat;
            }
            else if ((clipboardRawData = dataObj.GetData(DataFormats.Text)) != null)
            {
                parseFormat = ParseTextFormat;
            }

            if (parseFormat != null)
            {
                string rawDataStr = clipboardRawData as string;
                if (rawDataStr == null && clipboardRawData is MemoryStream)
                {
                    // Cannot convert to a string so try a MemoryStream.
                    MemoryStream ms = clipboardRawData as MemoryStream;
                    StreamReader sr = new StreamReader(ms);
                    rawDataStr = sr.ReadToEnd();
                }
                Debug.Assert(rawDataStr != null, string.Format("ClipboardRawData: {0}, Could Not Be Converted To A String Or Memorystream.", clipboardRawData));
                string[] rows = rawDataStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (rows != null && rows.Length > 0)
                {
                    clipboardData = new List<string[]>();
                    foreach (string row in rows)
                    {
                        clipboardData.Add(parseFormat(row));
                    }
                }
                else
                {
                    Debug.WriteLine("Unable To Parse Row Data.  Possibly Null Or Contains Zero Rows.");
                }
            }
            return clipboardData;
        }

        public static string[] ParseCsvFormat(string value)
        {
            return ParseCsvOrTextFormat(value, true);
        }

        public static string[] ParseTextFormat(string value)
        {
            return ParseCsvOrTextFormat(value, false);
        }

        private static string[] ParseCsvOrTextFormat(string value, bool isCSV)
        {
            List<string> outputList = new List<string>();
            char separator = isCSV ? ',' : '\t';
            int startIndex = 0;
            int endIndex = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (ch == separator)
                {
                    outputList.Add(value.Substring(startIndex, endIndex - startIndex));
                    startIndex = endIndex + 1;
                    endIndex = startIndex;
                }
                else if (ch == '\"' && isCSV)
                {
                    // Skip until the ending quotes.
                    i++;
                    if (i >= value.Length)
                    {
                        throw new FormatException(string.Format("Value: {0}, Had A Format Exception.", value));
                    }
                    char tempCh = value[i];
                    while (tempCh != '\"' && i < value.Length)
                        i++;
                    endIndex = i;
                }
                else if (i + 1 == value.Length)
                {
                    // Add the last value.
                    outputList.Add(value.Substring(startIndex));
                    break;
                }
                else
                {
                    endIndex++;
                }
            }
            return outputList.ToArray();
        }
    }
}
