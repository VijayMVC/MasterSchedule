using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Helpers
{
    class TimeHelper
    {
        public static DateTime Convert(string input)
        {
            DateTime output = new DateTime(2000, 1, 1);
            try
            {
                string[] inputSplit = input.Split('/');
                int day = int.Parse(inputSplit[1]);
                int month = int.Parse(inputSplit[0]);
                int year = DateTime.Now.Year;
                if (inputSplit.Count() == 3)
                {
                    year = int.Parse(inputSplit[2]);
                }
                output = new DateTime(year, month, day);
            }
            catch
            {
                if (String.IsNullOrEmpty(input) == false)
                {
                    output = new DateTime(1999, 12, 31);
                }
            }
            return output;
        }

        public static string ConvertDateToView(string input)
        {
            string result = "";
            if (String.IsNullOrEmpty(input) == true)
            {
                return result;
            }
            else
            {
                return result = String.Format("{0:M/d}", TimeHelper.Convert(input));
            }
        }
    }
}
