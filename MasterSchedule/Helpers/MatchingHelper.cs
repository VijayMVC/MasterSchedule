using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Helpers
{
    public class MatchingHelper
    {
        public static int Calculate(int upper, int outsole, string sizeNo)
        {
            string[] sizeNoListException = {"0C", "1C", "2C", "3C", "4C"};
            int matching = 0;
            if (sizeNoListException.Contains(sizeNo) == true)
            {
                matching = upper;
            }
            else
            {
                matching = Math.Min(upper, outsole);
            }
            return matching;
        }
    }
}
