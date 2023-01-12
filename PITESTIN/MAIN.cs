using System;
using System.Diagnostics;
using Bigdecimal;
using System.Threading;
using System.Threading.Tasks;

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
        public static BigDecimal atanone(int n, int precision)
        {
            BigDecimal results = new BigDecimal(1, 0, precision);

            for (int i = 1; i <= n; i++)
            {
                results += i % 2 == 0 ? (BigDecimal)1 / (2 * i + 1) : (BigDecimal)(-1) / (2 * i + 1);
            }
            return 4*results;
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

            BigDecimal Power3 = new BigDecimal(1, 0, sqrt12.precision);
            for (int i = 1; i <= n; i++)
            {
                Power3 *= new BigDecimal(3, 0, sqrt12.precision);
                result += i % 2 == 0 ? sqrt12 / (new BigDecimal(2 * i + 1, 0, sqrt12.precision) * Power3) : -sqrt12 / (new BigDecimal(2 * i + 1, 0, sqrt12.precision) * Power3);
                results[i - 1] = comparetotext(result.toString(), PI);
            }
            return results;
        }
        public static int[] BaileyMethod(int n)
        {
            const int precision = 1000;
            BigDecimal result = new BigDecimal(0, 0, precision);
            int[] results = new int[n];

            BigDecimal sixteeninverted = new BigDecimal(1,0,precision)/16;
            for (int i = 0; i < n; i++)
            {
                result += sixteeninverted * (new BigDecimal(4, 0, precision) / (8 * i + 1)) - (new BigDecimal(2, 0, precision) / (8 * i + 4)) - (new BigDecimal(1, 0, precision) / (8 * i + 5)) - (new BigDecimal(1, 0, precision) / (8 * i + 6));
                results[i - 1] = comparetotext(result.toString(), PI);
                sixteeninverted /= 16;
                
            }
            return results;



        }


        public static int[] runtest(Func<int,int, BigDecimal> fun, int tests)
        {

            int[] results = new int[tests];
            int precision = 15;
            for (int i = 1; i <= tests; i++)
            {
                BigDecimal result = fun(i,precision);
                int decimals = comparetotext(result.toString(), PI);
                if (decimals >= precision)
                {
                    precision *= 2;
                    i--;
                    continue;
                }
                results[i - 1] = decimals;
            }
            return results;
        }
        public static int[] runtest(Func<int,int, BigDecimal> fun, int tests, int precision)
        {
            int[] results = new int[tests];
            Parallel.For(1, tests + 1, i =>
            {
                results[i - 1] = comparetotext(fun(i,precision).toString(), PI);
            });
            return results;
        }
        // results vid itteration 1 är index 0
        /*static void writeresults(int[] results, string address)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Data");
                for (int i = 0; i < results.Length; i++)
                {
                    worksheet.Cell("A" + (i + 1)).Value = i + 1;
                    worksheet.Cell("B" + (i + 1)).Value = results[i];
                    workbook.SaveAs(address);
                }
            }
        }*/

        static string PI;
        public static BigDecimal sqrt12;
        
        public static void Init()
        {
            PI = System.IO.File.ReadAllText("./PI");
            sqrt12 = BigDecimal.Parse(System.IO.File.ReadAllText("./SQRT12"));
        }
    }
}
