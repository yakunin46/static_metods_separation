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

namespace static_metods_separation
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private static int[] Parse_Coeffs(TextBox tb1, TextBox tb2)
        {
            int[] coeffs = new int[2];
            coeffs[0] = int.Parse(tb1.Text);
            coeffs[1] = int.Parse(tb2.Text) + 1;
            return coeffs;
        }
        private static void Print_Random_ABCD(int[]coeffs,
            TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            int coeff1 = coeffs[0];
            int coeff2 = coeffs[1];
            Random r = new Random();
            tb1.Text = r.Next(coeff1, coeff2).ToString();
            tb2.Text = r.Next(coeff1, coeff2).ToString();
            tb3.Text = r.Next(coeff1, coeff2).ToString();
            tb4.Text = r.Next(coeff1, coeff2).ToString();
        }
        private static int[] Parse_ABCD(TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            int[] abcd = new int[4];
            abcd[0] = int.Parse(tb1.Text);
            abcd[1] = int.Parse(tb2.Text);
            abcd[2] = int.Parse(tb3.Text);
            abcd[3] = int.Parse(tb4.Text);
            return abcd;
        }
        private static void Print_ABCD(int[] abcd, TextBox tb)
        {
            tb.Text = "";

            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

            tb.Text += $"A = {a}\t B = {b}\t C = {c}\t D = {d}";
            tb.Text += Environment.NewLine + Environment.NewLine;
        }
        private static void Print_Equation(int[] abcd, TextBox tb)
        {
            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

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
        private static double Calculate_Y(int x, int[] abcd)
        {
            return Calculate_Y((double)x, abcd);
        }
        private static double Calculate_Y(double x, int[] abcd)
        {
            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

            double y = a * Math.Pow(x, 3) + b * Math.Pow(x, 2) + c * x + d;
            return y;
        }
        private static double[,] Find_XY(int[] abcd, int start, int end, int step)
        {
            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

            int length = (end - start) / step + 1;
            double[,] mas_xy = new double[length, 2];
            for (int x = start; x <= end; x += step)
            {
                double y = Calculate_Y(x, abcd);
                int i = (x - start) / step;
                mas_xy[i, 0] = x;
                mas_xy[i, 1] = y;
            }

            return mas_xy;
        }
        private static void Print_XY(double[,] mas_xy, TextBox tb)
        {
            int length = mas_xy.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                tb.Text += $"x = {mas_xy[i, 0]}\t y = {mas_xy[i, 1]}";
                tb.Text += Environment.NewLine;
            }
        }
        private  static double[,] Find_Change_Sign(double[,] mas_xy, int length)
        {
            double[,] change_sign = new double[7, 2];
            int count = 0;
            for (int i = 1; i < length; i++)
            {
                if (mas_xy[i - 1, 1] * mas_xy[i, 1] < 0)
                {
                    change_sign[count, 0] = mas_xy[i - 1, 0];
                    change_sign[count, 1] = mas_xy[i - 1, 1];
                    change_sign[count + 1, 0] = mas_xy[i, 0];
                    change_sign[count + 1, 1] = mas_xy[i, 1];
                    count += 2;
                }
                if ((i != length - 1) && (mas_xy[i, 1] == 0) &&
                    (mas_xy[i - 1, 1] * mas_xy[i + 1, 1] < 0))
                {
                    change_sign[count, 0] = mas_xy[i - 1, 0];
                    change_sign[count, 1] = mas_xy[i - 1, 1];
                    change_sign[count + 1, 0] = mas_xy[i + 1, 0];
                    change_sign[count + 1, 1] = mas_xy[i + 1, 1];
                    count += 2;
                }
            }

            change_sign[6, 0] = count;

            return change_sign;
        }
        private static void Print_Change_Sign(double[,] change_sign, TextBox tb)
        {
            double length = change_sign[6, 0];

            for (int i = 0; i < length; i += 2)
            {
                tb.Text += "Смена знаков:" + Environment.NewLine;
                tb.Text += $"x = {change_sign[i, 0]}\t y = {change_sign[i, 1]}";
                tb.Text += Environment.NewLine;
                tb.Text += $"x = {change_sign[i + 1, 0]}\t y = {change_sign[i + 1, 1]}";
                tb.Text += Environment.NewLine;
            }
        }
        private static double Find_Y_Limit(int[] coeffs, int start, int end)
        {
            int coeff1 = coeffs[0];
            int coeff2 = coeffs[1];

            double max_coeff = Math.Max(Math.Abs(coeff1), Math.Abs(coeff2));
            double max_x = Math.Max(Math.Abs(start), Math.Abs(end));
            double y_limit = max_coeff * Math.Pow(max_x, 3) + max_coeff * Math.Pow(max_x, 2) +
                max_coeff * max_x + max_coeff;

            return y_limit;
        }
        private static double Method1 (double[,] change_sign, double mini_step,
            double y_limit, int[] abcd)
        {
            double bot_lim = change_sign[0, 0];
            double top_lim = change_sign[1, 0];
            double x_exemplary = bot_lim;
            for (double x = bot_lim; x <= top_lim; x += mini_step)
            {
                double y = Math.Abs(Calculate_Y(x, abcd));
                if (y < y_limit)
                {
                    x_exemplary = x;
                    y_limit = y;
                }
            }
            return x_exemplary;
        }
        private static double Method2 (double[,] change_sign, int[] abcd, double accuracy)
        {
            double bot_lim = change_sign[0, 0];
            double top_lim = change_sign[1, 0];
            double x_exemplary = bot_lim;
            while (top_lim - bot_lim >= accuracy)
            {
                x_exemplary = (bot_lim + top_lim) / 2;
                double y_bot = Calculate_Y(bot_lim, abcd);
                double y_top = Calculate_Y(top_lim, abcd);
                double y_curr = Calculate_Y(x_exemplary, abcd);
                if (y_curr == 0)
                {
                    return x_exemplary;
                }
                if (y_curr * y_bot < 0)
                {
                    top_lim = x_exemplary;
                }
                else
                {
                    if (y_curr * y_top < 0)
                    {
                        bot_lim = x_exemplary;
                    }
                }
            }
            return x_exemplary;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] coeffs = Parse_Coeffs(textBox14, textBox15);

            Print_Random_ABCD(coeffs, textBox1, textBox2, textBox3, textBox4);

            int[] abсd = Parse_ABCD(textBox1, textBox2, textBox3, textBox4);

            Print_Equation(abсd, textBox5);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            int[] abcd = Parse_ABCD(textBox1, textBox2, textBox3, textBox4);

            Print_Equation(abcd, textBox5);

            Print_ABCD(abcd, textBox9);

            int start = int.Parse(textBox6.Text);
            int end = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (end - start) / step + 1;

            double[,] mas_xy = Find_XY(abcd, start, end, step);

            Print_XY(mas_xy, textBox9);

            double[,] change_sign = Find_Change_Sign(mas_xy, length);

            Print_Change_Sign(change_sign, textBox9);

        }
        private void button3_Click(object sender, EventArgs e)
        {
            int[] abcd = Parse_ABCD(textBox1, textBox2, textBox3, textBox4);
            int[] x1x2 = Parse_Coeffs(textBox14, textBox15);

            int start = int.Parse(textBox6.Text);
            int end = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (end - start) / step + 1;

            double[,] mas_xy = Find_XY(abcd, start, end, step);
            double[,] change_sign = Find_Change_Sign(mas_xy, length);
            double mini_step = double.Parse(textBox12.Text);
            double y_limit = Find_Y_Limit(x1x2, start, end);

            double x_exemplary = Method1(change_sign, mini_step, y_limit, abcd);

            textBox10.Text = "";
            textBox10.Text += $"{x_exemplary:F9}";
            textBox10.Text += Environment.NewLine;
            double y = Calculate_Y(x_exemplary, abcd);
            textBox10.Text += $"{y:F9}";
        }
        private void button4_Click(object sender, EventArgs e)
        {

            int start = int.Parse(textBox6.Text);
            int end = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (end - start) / step + 1;

            int[] abcd = Parse_ABCD(textBox1, textBox2, textBox3, textBox4);

            double[,] mas_xy = Find_XY(abcd, start, end, step);

            double[,] change_sign = Find_Change_Sign(mas_xy, length);

            double accuracy = double.Parse(textBox13.Text);

            double x_exemplary = Method2(change_sign, abcd, accuracy);

            textBox11.Text = "";
            textBox11.Text += $"{x_exemplary:F9}";
            textBox11.Text += Environment.NewLine;
            double y = Calculate_Y(x_exemplary, abcd);
            textBox11.Text += $"{y:F9}";
        }
    }
}