using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamer
{
    public class NaturalSorter : IComparer<string>
    {
        private int ascii0 = (int)'0';
        private int ascii9 = (int)'9';

        public int Compare(string x, string y)
        {
            x = Path.GetFileNameWithoutExtension(x);
            y = Path.GetFileNameWithoutExtension(y);
            int len = x.Length > y.Length ? x.Length : y.Length;
            int res = x.Length > y.Length ? 1 : x.Length == y.Length ? 0 : -1;

            int xd = 0, yd = 0;

            for (int i = 0, j = 0; i < len; i++)
            {
                if (i >= x.Length || j >= y.Length)
                    return res;


                if (IsNumber(x[i]) && IsNumber(y[j]))
                {
                    while (i < x.Length && IsNumber(x[i]))
                    {
                        xd *= 10;
                        xd += GetNum(x[i]);
                        i++;
                    }
                    while (j < y.Length && IsNumber(y[j]))
                    {
                        yd *= 10;
                        yd += GetNum(y[j]);
                        j++;
                    }

                    if (xd < yd)
                        return -1;
                    else if (xd > yd)
                        return 1;

                    j++;
                    continue;
                }

                if (IsNumber(x[i]))
                    return 1;
                if (IsNumber(y[j]))
                    return -1;

                if (x[i] < y[j])
                    return 1;
                else if (x[i] > y[j])
                    return -1;

                j++;

            }

            return 0;
        }

        private bool IsNumber(char x)
        {
            if ((int)x >= ascii0 && (int)x <= ascii9)
                return true;
            return false;
        }

        private int GetNum(char x)
        {
            return (int)x - ascii0;
        }
    }
}
