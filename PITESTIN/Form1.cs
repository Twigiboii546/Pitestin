using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using piapprox;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using Bigdecimal;

namespace PITESTIN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PITOOLS.Init();
            const int Iterations = 100;
            (string, Func<int, int[]>)[] MethodFunctions = new (string, Func<int, int[]>)[]
            {
                ("Leibniz", PITOOLS.LeibnizMethod),
                ("Madhava", PITOOLS.MadhavaMethod),
                ("Bailey", PITOOLS.BaileyMethod),
                ("Bellard", PITOOLS.BellardMethod),
                ("Chudnovsky", PITOOLS.ChudnovskyMethod)
            };

            int Methods = MethodFunctions.Length;
            (int[], float)[] MethodsResults = new (int[], float)[Methods];

            Parallel.For(0, Methods, i =>
            {
                Stopwatch st = Stopwatch.StartNew();
                int[] res = MethodFunctions[i].Item2(Iterations);
                st.Stop();
                MethodsResults[i] = (res, st.ElapsedMilliseconds);
            });
            for (int i = 0; i < Methods; i++)
            {
                Series series = new Series(MethodFunctions[i].Item1);
                series.ChartType = SeriesChartType.Line;
                int[] results = MethodsResults[i].Item1;
                series.LegendText = $"{MethodFunctions[i].Item1}: {results[results.Length - 1]} correct in {MethodsResults[i].Item2} ms";
                for (int j = 0; j < results.Length; j++)
                {
                    series.Points.AddXY(j + 1, results[j]);
                }
                chart1.Series.Add(series);
            }
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
