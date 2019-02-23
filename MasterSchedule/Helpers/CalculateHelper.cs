using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Helpers
{
    class CalculateHelper
    {
        public static float ConvertToFloat(string input)
        {
            float output;
            float.TryParse(input, out output);
            return output;
        }

        public static float DivideFloat(float input1, float input2)
        {
            float output = 0;
            if (input2 <= 0 || input1 <= 0)
            {
                return output;
            }
            else
            {
                output = input1 / input2;
            }
            return output;
        }
    }
}
