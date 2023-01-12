using System;
using System.Diagnostics;
using Bigdecimal;
using ClosedXML.Excel;
using System.Threading;
using System.Threading.Tasks;

namespace piapprox
{
    class Program
    {
        static int comparetotext(string a, string b)
        {
            int correctdigits = 0;
            
            while (true)
            {
                if(correctdigits >= a.Length || correctdigits >= b.Length) return correctdigits;
                if (a[correctdigits] != b[correctdigits]) return correctdigits;
                correctdigits++;
            }

        }
        static BigDecimal atanone(int n, int precision)
        {
            BigDecimal results = new BigDecimal(1, 0, precision);

            for (int i = 1; i <= n; i++)
            {
                results += i % 2 == 0 ? (BigDecimal)1 / (2 * i + 1) : (BigDecimal)(-1) / (2 * i + 1);
            }
            return 4*results;
        }
        static int[] LeibnizMethod(int n)
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
        static int[] runtest(Func<int,int, BigDecimal> fun, int tests)
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
        static int[] runtest(Func<int,int, BigDecimal> fun, int tests, int precision)
        {
            int[] results = new int[tests];
            Parallel.For(1, tests + 1, i =>
            {
                results[i - 1] = comparetotext(fun(i,precision).toString(), PI);
            });
            return results;
        }
        // results vid itteration 1 är index 0
        static void writeresults(int[] results, string address)
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


        }

        static string PI;
        
        static void Main(string[] args)
        {
            PI = System.IO.File.ReadAllText("./PI");
            Stopwatch st = Stopwatch.StartNew();
            int[] result = LeibnizMethod(100000000);
            st.Stop();
            Console.WriteLine($"Finished in {st.ElapsedMilliseconds} ms");
            Console.WriteLine($"Last term has {result[^1]} digits correct");
            writeresults(result, "LeibnizMethodData.xlsx");
            Console.WriteLine("Done Writing!");
            Console.ReadLine();
        }
    }
}
