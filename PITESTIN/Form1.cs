using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using piapprox;
using Bigdecimal;

namespace PITESTIN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PITOOLS.Init();
            chart1.Titles.Add("DATA");
            const int Iterations = 10;
            int[] LeibnizResult = PITOOLS.LeibnizMethod(Iterations);
            chart1.Series["Leibniz"].Points.AddXY(1, LeibnizResult[0]);
            for (int i = 1; i < LeibnizResult.Length; i++)
            {
                if ((i + 1) % 1 != 0)
                {
                    continue;
                }
                chart1.Series["Leibniz"].Points.AddXY(i + 1, LeibnizResult[i]);
            }

            int[] MadhavaResult = PITOOLS.MadhavaMethod(Iterations);
            chart1.Series["Madhava"].Points.AddXY(1, MadhavaResult[0]);
            for (int i = 1; i < MadhavaResult.Length; i++)
            {
                if ((i + 1) % 5 != 0)
                {
                    continue;
                }
                chart1.Series["Madhava"].Points.AddXY(i + 1, MadhavaResult[i]);
            }
            int[] BaileyResult = PITOOLS.BaileyMethod(Iterations);
            chart1.Series["Bailey"].Points.AddXY(1, BaileyResult[0]);
            for (int i = 1; i < BaileyResult.Length; i++)
            {
                if ((i + 1) % 5 != 0)
                {
                    continue;
                }
                chart1.Series["Bailey"].Points.AddXY(i + 1, BaileyResult[i]);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
