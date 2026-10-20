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

        private static void print_random_abcd(int[]x1x2,
            TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            int x1 = x1x2[0];
            int x2 = x1x2[1];
            Random r = new Random();
            tb1.Text = r.Next(x1, x2).ToString();
            tb2.Text = r.Next(x1, x2).ToString();
            tb3.Text = r.Next(x1, x2).ToString();
            tb4.Text = r.Next(x1, x2).ToString();
        }
        private static int[] parse_abcd(TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            int[] abcd = new int[4];
            abcd[0] = int.Parse(tb1.Text);
            abcd[1] = int.Parse(tb2.Text);
            abcd[2] = int.Parse(tb3.Text);
            abcd[3] = int.Parse(tb4.Text);
            return abcd;
        }
        private static int[] parse_x1x2(TextBox tb1, TextBox tb2)
        {
            int[] x1x2 = new int[2];
            x1x2[0] = int.Parse(tb1.Text);
            x1x2[1] = int.Parse(tb2.Text) + 1;
            return x1x2;
        }
        private static void print_equation(int[] abcd, TextBox tb)
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

            if (a != 0 & b != 0 & b > 0)
            {
                tb.Text += $" + {b}x\u00B2";
            }
            else 
                if (b != 0)
                    tb.Text += $" {b}x\u00B2";

            if ((a != 0 | b != 0) & c != 0 & c > 0)
            {
                tb.Text += $" + {c}x";
            }
            else
                if (c != 0)
                    tb.Text += $" {c}x";

            if ((a != 0 | b != 0 | c != 0) & d != 0 & d > 0)
            {
                tb.Text += $" + {d}";
            }
            else
                if (d != 0)
                    tb.Text += $" {d}";
        }
        private static double calculate_y(int x, int[] abcd)
        {
            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

            return calculate_y((double)x, abcd);
        }
        private static double calculate_y(double x, int[] abcd)
        {
            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

            double y = a * Math.Pow(x, 3) + b * Math.Pow(x, 2) + c * x + d;
            return y;
        }
        private static double[,] find_xy(int[] abcd, int start, int end, int step)
        {
            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

            int length = (end - start) / step + 1;
            double[,] mas_xy = new double[length, 2];
            for (int x = start; x <= end; x += step)
            {
                double y = calculate_y(x, abcd);
                int i = (x - start) / step;
                mas_xy[i, 0] = x;
                mas_xy[i, 1] = y;
            }

            return mas_xy;
        }
        private static void print_xy(double[,] mas_xy, TextBox tb)
        {
            int length = mas_xy.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                tb.Text += $"x = {mas_xy[i, 0]}\t y = {mas_xy[i, 1]}";
                tb.Text += Environment.NewLine;
            }
        }
        private  static double[,] find_change_sign(double[,] mas_xy, int length)
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
                if ((mas_xy[i, 1] == 0) && (i != length - 1) &&
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
        private static void print_change_sign(double[,] change_sign, TextBox tb)
        {
            double length = change_sign[6, 0];

            for (int i = 0; i < length; i += 2)
            {
                tb.Text += "Смена знаков:";
                tb.Text += Environment.NewLine;
                tb.Text += $"x = {change_sign[i, 0]}\t y = {change_sign[i, 1]}";
                tb.Text += Environment.NewLine;
                tb.Text += $"x = {change_sign[i + 1, 0]}\t y = {change_sign[i + 1, 1]}";
                tb.Text += Environment.NewLine;
            }
        }
        private static double find_y_limit(int[] x1x2, int start, int end)
        {
            int x1 = x1x2[0];
            int x2 = x1x2[1];

            double max_abcd = Math.Max(Math.Abs(x1), Math.Abs(x2));
            double max_x = Math.Max(Math.Abs(start), Math.Abs(end));
            double y_limit = max_abcd * Math.Pow(max_x,3) + max_abcd * Math.Pow(max_x, 2) +
                max_abcd * max_x + max_abcd;
            return y_limit;
        }
        private static double method1 (double[,] change_sign, double mini_step,
            double y_limit, int[] abcd)
        {

            double bot_lim = change_sign[0, 0];
            double top_lim = change_sign[1, 0];
            double x_exemplary = 1;
            for (double x = bot_lim; x <= top_lim; x += mini_step)
            {
                double y = calculate_y(x, abcd);
                if (Math.Abs(y) < y_limit)
                {
                    x_exemplary = x;
                    y_limit = Math.Abs(y);
                }
            }
            return x_exemplary;
        }
        private double method2 (double[,] change_sign, double accuracy, int[] abcd)
        {
            double bot_lim = change_sign[0, 0];
            double top_lim = change_sign[1, 0];
            double x_exemplary = bot_lim;
            while (top_lim - bot_lim >= accuracy)
            {
                x_exemplary = (bot_lim + top_lim) / 2;
                double y_bot = calculate_y(bot_lim, abcd);
                double y_top = calculate_y(top_lim, abcd);
                double y_curr = calculate_y(x_exemplary, abcd);
                if (y_curr == 0)
                {
                    return x_exemplary;
                }
                if (y_curr * y_bot < 0)
                    top_lim = x_exemplary;
                else
                    if (y_curr * y_top < 0)
                        bot_lim = x_exemplary;
            }
            return x_exemplary;
        }
        private static void print_abcd(TextBox tb, int[] abcd)
        {
            tb.Text = "";

            int a = abcd[0];
            int b = abcd[1];
            int c = abcd[2];
            int d = abcd[3];

            tb.Text += $"A = {a}\t B = {b}\t C = {c}\t D = {d}";
            tb.Text += Environment.NewLine + Environment.NewLine;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] x1x2 = parse_x1x2(textBox14, textBox15);

            print_random_abcd(x1x2, textBox1, textBox2, textBox3, textBox4);

            int[] abсd = parse_abcd(textBox1, textBox2, textBox3, textBox4);

            print_equation(abсd, textBox5);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            int[] abcd = parse_abcd(textBox1, textBox2, textBox3, textBox4);

            print_equation(abcd, textBox5);

            print_abcd(textBox9, abcd);

            int start = int.Parse(textBox6.Text);
            int end = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (end - start) / step + 1;

            double[,] mas_xy = find_xy(abcd, start, end, step);

            print_xy(mas_xy, textBox9);

            double[,] change_sign = find_change_sign(mas_xy, length);

            print_change_sign(change_sign, textBox9);

        }
        private void button3_Click(object sender, EventArgs e)
        {
            int[] abcd = parse_abcd(textBox1, textBox2, textBox3, textBox4);
            int[] x1x2 = parse_x1x2(textBox14, textBox15);

            int start = int.Parse(textBox6.Text);
            int end = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (end - start) / step + 1;

            double[,] mas_xy = find_xy(abcd, start, end, step);
            double[,] change_sign = find_change_sign(mas_xy, length);
            double mini_step = double.Parse(textBox12.Text);
            double y_limit = find_y_limit(x1x2, start, end);

            double x_exemplary = method1(change_sign, mini_step, y_limit, abcd);

            textBox10.Text = "";
            textBox10.Text += $"{x_exemplary:F9}";
            textBox10.Text += Environment.NewLine;
            double y = calculate_y(x_exemplary, abcd);
            textBox10.Text += $"{y:F9}";
        }
        private void button4_Click(object sender, EventArgs e)
        {

            int start = int.Parse(textBox6.Text);
            int end = int.Parse(textBox7.Text);
            int step = int.Parse(textBox8.Text);
            int length = (end - start) / step + 1;

            int[] abcd = parse_abcd(textBox1, textBox2, textBox3, textBox4);

            double[,] mas_xy = find_xy(abcd, start, end, step);

            double[,] change_sign = find_change_sign(mas_xy, length);

            double accuracy = double.Parse(textBox13.Text);

            double x_exemplary = method2(change_sign, accuracy, abcd);

            textBox11.Text = "";
            textBox11.Text += $"{x_exemplary:F9}";
            textBox11.Text += Environment.NewLine;
            double y = calculate_y(x_exemplary, abcd);
            textBox11.Text += $"{y:F9}";
        }
    }
}