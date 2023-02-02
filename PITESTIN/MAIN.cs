using Bigdecimal;
using System;
using System.Security.Policy;

namespace piapprox
{
    class PITOOLS
    {
        public static int comparetotext(string a, string b)
        {
            int correctdigits = 0;
            while (true)
            {
                if(correctdigits >= a.Length || correctdigits >= b.Length) return correctdigits;
                if (a[correctdigits] != b[correctdigits]) return correctdigits;
                correctdigits++;
            }
        }
        public static BigDecimal BD(int x, int precision)
        {
            return new BigDecimal(x, 0, precision);
        }
        public static int[] LeibnizMethod(int n)
        {
            BigDecimal results = new BigDecimal(4, 0, 15);
            int[] decimalResults = new int[n];
            for (int i = 1; i <= n; i++)
            {
                BigDecimal AddTerm = i % 2 == 0 ? (BigDecimal)4 / (2 * i + 1) : (BigDecimal)(-4) / (2 * i + 1);
                results += AddTerm;
                int decRes = comparetotext(results.toString(), PI);
                if(decRes == results.precision)
                {
                    results -= AddTerm;
                    results.ChangePrecision(results.precision * 2);
                    i--;
                    continue;
                }
                decimalResults[i - 1] = decRes;
            }
            return decimalResults;
        }
        public static int[] MadhavaMethod(int n)
        {
            BigDecimal result = sqrt12;
            int[] results = new int[n];

            BigDecimal Power3 = BD(1, sqrt12.precision);
            for (int i = 1; i <= n; i++)
            {
                Power3 *= BD(3, sqrt12.precision);
                result += i % 2 == 0 ? sqrt12 / (BD(2 * i + 1, sqrt12.precision) * Power3) : -sqrt12 / (BD(2 * i + 1, sqrt12.precision) * Power3);
                results[i - 1] = comparetotext(result.toString(), PI);
            }
            return results;
        }
        public static int[] BaileyMethod(int n)
        {
            const int precision = 1500;
            BigDecimal result = new BigDecimal(33,-1,precision) - BD(1, precision)/ 6;
            int[] results = new int[n];
            results[0] = comparetotext(result.toString(), PI);
            BigDecimal sixteen = BD(1, precision) / 16;
            for (int i = 1; i < n; i++)
            {
                result += sixteen * (BD(4, precision) / (8 * i + 1) - BD(2, precision) / (8 * i + 4) - BD(1, precision) / (8 * i + 5) - BD(1,precision) / (8 * i + 6));
                sixteen /= 16;
                results[i] = comparetotext(result.toString(), PI);
            }
            return results;
        }
        public static int[] BellardMethod(int n)
        {
            const int precision = 3300;
            BigDecimal onedivsixfour = new BigDecimal(15625,-6,precision);
            BigDecimal result = BD(0, precision);
            BigDecimal onedivtusentjugofyra = BD(1, precision);
            int[] results = new int[n];
            
            for (int i = 0; i < n; i++)
            {
                result += (i % 2 == 0 ? 1 : -1) * onedivtusentjugofyra * (-BD(32, precision) / (4 * i + 1) - BD(1, precision) / (4*i + 3) + BD(256, precision)/(10*i+1)-BD(64,precision)/(10*i+3)-BD(4,precision)/(10*i+5)-BD(4,precision)/(10*i+7)+BD(1,precision)/(10*i+9));
                onedivtusentjugofyra /= 1024;
                results[i] = comparetotext((result * onedivsixfour).toString(), PI);
            }
            return results;
        }
        public static int[] ChudnovskyMethod(int n)
        {
            int precision = chudconst.precision;
            BigDecimal sixkfact = BD(1, precision); // (6k)!
            BigDecimal AddTerm = BD(13591409, precision); // (13591409 + 545140134k)
            BigDecimal threekfact = BD(1, precision); // (3k)!
            BigDecimal kfactcubed = BD(1, precision); // (k!)^3
            BigDecimal cubeterm = BD(1, precision); // -262537412640768000^k
            BigDecimal cubemultiplier = -BigDecimal.Parse("262537412640768000");
            BigDecimal result = AddTerm;
            int[] results = new int[n];
            results[0] = comparetotext((1 / result * chudconst).toString(), PI);
            for (int k = 1; k < n; k++)
            {
                // Getting new value for 6kfact
                BigDecimal sixk = BD(6 * k, precision);
                for (int i = 0; i <= 5; i++)
                {
                    sixkfact *= sixk - i;
                }
                // Getting new value for AddTerm
                AddTerm.precision = precision;
                AddTerm += 545140134;
                // Getting new value for threekfact
                BigDecimal threek = BD(3 * k, precision);
                for (int i = 0; i <= 2; i++)
                {
                    threekfact *= threek - i;
                }
                //Getting new value for kfactcubed
                kfactcubed *= BD(k, precision) * BD(k, precision) * BD(k, precision);
                //Getting new value for cubeterm
                cubeterm *= cubemultiplier;
                //Getting new value for result 
                result += sixkfact * AddTerm / (threekfact * kfactcubed * cubeterm);
                // Appending value to array
                results[k] = comparetotext((1 / result * chudconst).toString(), PI);
            }
            return results;
        }

        static string PI;
        public static BigDecimal sqrt12;
        public static BigDecimal chudconst;
        
        public static void Init()
        {
            PI = System.IO.File.ReadAllText("./PI");
            sqrt12 = BigDecimal.Parse(System.IO.File.ReadAllText("./SQRT12"));
            chudconst = BigDecimal.Parse(System.IO.File.ReadAllText("./ChudOuterConstant"));
        }
    }
}
