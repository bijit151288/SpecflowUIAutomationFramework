using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowNunitTestAutomation.Utils
{
    public class RandomItemGenerator
    {
        public static string RandomTextGeneration(int length)
        {
            // creating a StringBuilder object()
            StringBuilder str_build = new();
            Random random = new();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        public static string RandomNumberGeneration(Int64 min, Int64 max)
        {
            Random rnd = new();
            Int64 myRandomNo = rnd.NextInt64(min, max);

            return myRandomNo.ToString();
        }
    }

}