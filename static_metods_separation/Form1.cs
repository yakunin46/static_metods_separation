using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace StaticMethodsSeparation
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private static int[] ParseLimsOfCoeffs(TextBox tb1, TextBox tb2)
        {
            int[] limsOfCoeffs = new int[2];
            limsOfCoeffs[0] = int.Parse(tb1.Text);
            limsOfCoeffs[1] = int.Parse(tb2.Text) + 1;
            return limsOfCoeffs;
        }
        private static void FillRandomCoeffs(int[] limsOfCoeffs,
            TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            int limOfCoeffs1 = limsOfCoeffs[0];
            int limOfСoeffs2 = limsOfCoeffs[1];
            Random r = new Random();
            tb1.Text = r.Next(limOfCoeffs1, limOfСoeffs2).ToString();
            tb2.Text = r.Next(limOfCoeffs1, limOfСoeffs2).ToString();
            tb3.Text = r.Next(limOfCoeffs1, limOfСoeffs2).ToString();
            tb4.Text = r.Next(limOfCoeffs1, limOfСoeffs2).ToString();
        }
        private static int[] ParseCoeffs(TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            int[] coeffs = new int[4];
            coeffs[0] = int.Parse(tb1.Text);
            coeffs[1] = int.Parse(tb2.Text);
            coeffs[2] = int.Parse(tb3.Text);
            coeffs[3] = int.Parse(tb4.Text);
            return coeffs;
        }
        private static void ShowCoeffs(int[] coeffs, TextBox tb)
        {
            tb.Text = "";

            int a = coeffs[0];
            int b = coeffs[1];
            int c = coeffs[2];
            int d = coeffs[3];

            tb.Text += $"A = {a}\t B = {b}\t C = {c}\t D = {d}";
            tb.Text += Environment.NewLine + Environment.NewLine;
        }
        private static void ShowEquation(int[] coeffs, TextBox tb)
        {
            int a = coeffs[0];
            int b = coeffs[1];
            int c = coeffs[2];
            int d = coeffs[3];

            tb.Text = "";

            if (a != 0)
            {
                tb.Text += $"{a}x\u00B3";
            }

            if (a != 0 & b > 0)
            {
                tb.Text += $" + {b}x\u00B2";
            }
            else 
                if (b != 0)
                    tb.Text += $" {b}x\u00B2";

            if ((a != 0 | b != 0) & c > 0)
            {
                tb.Text += $" + {c}x";
            }
            else
                if (c != 0)
                    tb.Text += $" {c}x";

            if ((a != 0 | b != 0 | c != 0) & d > 0)
            {
                tb.Text += $" + {d}";
            }
            else
                if (d != 0)
                    tb.Text += $" {d}";
        }
        private static double CalculateY(int x, int[] coeffs)
        {
            return CalculateY((double)x, coeffs);
        }
        private static double CalculateY(double x, int[] coeffs)
        {
            int a = coeffs[0];
            int b = coeffs[1];
            int c = coeffs[2];
            int d = coeffs[3];

            double y = a * Math.Pow(x, 3) + b * Math.Pow(x, 2) + c * x + d;
            return y;
        }
        private static double[,] FindXY(int[] coeffs, int xLim1, int xLim2, int step)
        {
            int a = coeffs[0];
            int b = coeffs[1];
            int c = coeffs[2];
            int d = coeffs[3];

            int length = (xLim2 - xLim1) / step + 1;
            double[,] masXY = new double[length, 2];
            for (int x = xLim1; x <= xLim2; x += step)
            {
                double y = CalculateY(x, coeffs);
                int i = (x - xLim1) / step;
                masXY[i, 0] = x;
                masXY[i, 1] = y;
            }

            return masXY;
        }
        private static void ShowXY(double[,] masXY, TextBox tb)
        {
            int length = masXY.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                tb.Text += $"x = {masXY[i, 0]}\t y = {masXY[i, 1]}";
                tb.Text += Environment.NewLine;
            }
        }
        private  static double[,] FindChangeSign(double[,] masXY, int length)
        {
            double[,] changeSign = new double[7, 2];
            int count = 0;
            int countZero = 0;
            for (int i = 1; i < length; i++)
            {
                int index = i - 1 - countZero;
                if (masXY[index, 1] * masXY[i, 1] < 0)
                {
                    changeSign[count, 0] = masXY[index, 0];
                    changeSign[count, 1] = masXY[index, 1];
                    changeSign[count + 1, 0] = masXY[i, 0];
                    changeSign[count + 1, 1] = masXY[i, 1];
                    count += 2; countZero = 0;
                }
                if (masXY[i, 1] == 0)
                {
                    countZero++;
                }
            }

            changeSign[6, 0] = count;

            return changeSign;
        }
        private static void ShowChangeSign(double[,] changeSign, TextBox tb)
        {
            double length = changeSign[6, 0];

            for (int i = 0; i < length; i += 2)
            {
                tb.Text += "Смена знаков:" + Environment.NewLine;
                tb.Text += $"x = {changeSign[i, 0]}\t y = {changeSign[i, 1]}";
                tb.Text += Environment.NewLine;
                tb.Text += $"x = {changeSign[i + 1, 0]}\t y = {changeSign[i + 1, 1]}";
                tb.Text += Environment.NewLine;
            }
        }
        private static double CalculateYLimit(int[] limsOfCoeffs, int xLim1, int xLim2)
        {
            int limOfCoeffs1 = limsOfCoeffs[0];
            int limOfCoeffs2 = limsOfCoeffs[1];

            double maxCoeff = Math.Max(Math.Abs(limOfCoeffs1), Math.Abs(limOfCoeffs2));
            double maxX = Math.Max(Math.Abs(xLim1), Math.Abs(xLim2));
            double yLimit = maxCoeff * Math.Pow(maxX, 3) + maxCoeff * Math.Pow(maxX, 2) +
                maxCoeff * maxX + maxCoeff;

            return yLimit;
        }
        private static double Method1 (double[,] changeSign, int[] coeffs,
            double smallStep, double yLimit)
        {
            double botLim = changeSign[0, 0];
            double topLim = changeSign[1, 0];
            double xExemplary = botLim;
            for (double x = botLim; x <= topLim; x += smallStep)
            {
                double y = Math.Abs(CalculateY(x, coeffs));
                if (y < yLimit)
                {
                    xExemplary = x;
                    yLimit = y;
                }
            }
            return xExemplary;
        }
        private static double Method2 (double[,] changeSign, int[] coeffs, double accuracy)
        {
            double botLim = changeSign[0, 0];
            double topLim = changeSign[1, 0];
            double xExemplary = botLim;
            while (topLim - botLim >= accuracy)
            {
                xExemplary = (botLim + topLim) / 2;
                double yBot = CalculateY(botLim, coeffs);
                double yTop = CalculateY(topLim, coeffs);
                double yCurr = CalculateY(xExemplary, coeffs);
                if (yCurr == 0)
                {
                    return xExemplary;
                }
                if (yCurr * yBot < 0)
                {
                    topLim = xExemplary;
                }
                else
                {
                    if (yCurr * yTop < 0)
                    {
                        botLim = xExemplary;
                    }
                }
            }
            return xExemplary;
        }
        private static void ShowExemplaryXY(int[] coeffs, double xExemplary, TextBox tb)
        {
            tb.Text = "";
            tb.Text += $"{xExemplary:F9}" + Environment.NewLine;
            double yExemplary = CalculateY(xExemplary, coeffs);
            tb.Text += $"{yExemplary:F9}";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] limsOfCoeffs = ParseLimsOfCoeffs(textBox14, textBox15);

            FillRandomCoeffs(limsOfCoeffs, textBox1, textBox2, textBox3, textBox4);

            int[] coeffs = ParseCoeffs(textBox1, textBox2, textBox3, textBox4);

            ShowEquation(coeffs, textBox5);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            int[] coeffs = ParseCoeffs(textBox1, textBox2, textBox3, textBox4);

            ShowEquation(coeffs, textBox5);

            ShowCoeffs(coeffs, textBox9);

            int xLim1 = int.Parse(textBox6.Text);
            int xLim2 = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (xLim2 - xLim1) / step + 1;

            double[,] masXY = FindXY(coeffs, xLim1, xLim2, step);

            ShowXY(masXY, textBox9);

            double[,] changeSign = FindChangeSign(masXY, length);

            ShowChangeSign(changeSign, textBox9);

        }
        private void button3_Click(object sender, EventArgs e)
        {
            int[] limsOfCoeffs = ParseLimsOfCoeffs(textBox14, textBox15);
            int[] coeffs = ParseCoeffs(textBox1, textBox2, textBox3, textBox4);

            int xLim1 = int.Parse(textBox6.Text);
            int xLim2 = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (xLim2 - xLim1) / step + 1;

            double smallStep = double.Parse(textBox12.Text);

            double[,] masXY = FindXY(coeffs, xLim1, xLim2, step);
            double[,] changeSign = FindChangeSign(masXY, length);
            double yLimit = CalculateYLimit(limsOfCoeffs, xLim1, xLim2);

            double xExemplary = Method1(changeSign, coeffs, smallStep, yLimit);

            ShowExemplaryXY(coeffs, xExemplary, textBox10);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            int[] coeffs = ParseCoeffs(textBox1, textBox2, textBox3, textBox4);

            int start = int.Parse(textBox6.Text);
            int end = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (end - start) / step + 1;

            double accuracy = double.Parse(textBox13.Text);

            double[,] masXY = FindXY(coeffs, start, end, step);
            double[,] changeSign = FindChangeSign(masXY, length);

            double xExemplary = Method2(changeSign, coeffs, accuracy);

            ShowExemplaryXY(coeffs, xExemplary, textBox11);
        }
    }
}