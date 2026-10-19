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

        private int a, b, c, d;
        private int start, end, step, length;
        private int x1, x2;
        double[,] mas_xy, change_sign;
        private Random r = new Random();

        private void random_abcd()
        {
            x1 = -10; x2 = 10;
            textBox1.Text = r.Next(x1, x2).ToString();
            textBox2.Text = r.Next(x1, x2).ToString();
            textBox3.Text = r.Next(x1, x2).ToString();
            textBox4.Text = r.Next(x1, x2).ToString();
        }
        private void parse_abcd()
        {
            a = int.Parse(textBox1.Text);
            b = int.Parse(textBox2.Text);
            c = int.Parse(textBox3.Text);
            d = int.Parse(textBox4.Text);
        }
        private void print_equation()
        {

            textBox5.Text = "";

            if (a != 0)
            {
                textBox5.Text += $"{a}x\u00B3";
            }

            if (a != 0 & b != 0 & b > 0)
            {
                textBox5.Text += $" + {b}x\u00B2";
            }
            else 
                if (b != 0)
                    textBox5.Text += $" {b}x\u00B2";

            if ((a != 0 | b != 0) & c != 0 & c > 0)
            {
                textBox5.Text += $" + {c}x";
            }
            else
                if (c != 0)
                    textBox5.Text += $" {c}x";

            if ((a != 0 | b != 0 | c != 0) & d != 0 & d > 0)
            {
                textBox5.Text += $" + {d}";
            }
            else
                if (d != 0)
                    textBox5.Text += $" {d}";
        }
        private double calculate_y(int x)
        {
            return calculate_y((double)x);
        }
        private double calculate_y(double x)
        {
            double y = a * Math.Pow(x, 3) + b * Math.Pow(x, 2) + c * x + d;
            return y;
        }
        private void print_xy()
        {
            start = int.Parse(textBox6.Text);
            end = int.Parse(textBox7.Text);
            step = int.Parse(textBox8.Text);

            length = (end - start) / step + 1;
            mas_xy = new double[length, 2];
            for (int x = start; x <= end; x += step)
            {
                double y = calculate_y(x);
                int i = (x - start) / step;
                mas_xy[i, 0] = x;
                mas_xy[i, 1] = y;
            }
            for (int i = 0; i < length; i++)
            {
                textBox9.Text += $"x = {mas_xy[i, 0]}\t y = {mas_xy[i, 1]}";
                textBox9.Text += Environment.NewLine;
            }
        }
        private void find_change_sign()
        {
            change_sign = new double[6, 2];
            int count = 0;
            for (int i = 1; i < length; i++)
            {
                if (mas_xy[i-1, 1] * mas_xy[i, 1] < 0)
                {
                    textBox9.Text += "Смена знаков:";
                    textBox9.Text += Environment.NewLine;
                    textBox9.Text += $"x = {mas_xy[i - 1, 0]}\t y = {mas_xy[i - 1, 1]}";
                    textBox9.Text += Environment.NewLine;
                    textBox9.Text += $"x = {mas_xy[i, 0]}\t y = {mas_xy[i, 1]}";
                    textBox9.Text += Environment.NewLine;
                    change_sign[count, 0] = mas_xy[i - 1, 0];
                    change_sign[count, 1] = mas_xy[i - 1, 1];
                    change_sign[count + 1, 0] = mas_xy[i, 0];
                    change_sign[count + 1, 1] = mas_xy[i, 1];
                    count += 2;
                }
                if ((mas_xy[i, 1] == 0) && (i != length - 1) &&
                    (mas_xy[i - 1, 1] * mas_xy[i + 1, 1] < 0))
                {
                    textBox9.Text += "Смена знаков:";
                    textBox9.Text += Environment.NewLine;
                    textBox9.Text += $"x = {mas_xy[i - 1, 0]}\t y = {mas_xy[i - 1, 1]}";
                    textBox9.Text += Environment.NewLine;
                    textBox9.Text += $"x = {mas_xy[i + 1, 0]}\t y = {mas_xy[i + 1, 1]}";
                    textBox9.Text += Environment.NewLine;
                    change_sign[count, 0] = mas_xy[i - 1, 0];
                    change_sign[count, 1] = mas_xy[i - 1, 1];
                    change_sign[count + 1, 0] = mas_xy[i + 1, 0];
                    change_sign[count + 1, 1] = mas_xy[i + 1, 1];
                    count += 2;
                }
            }
        }
        private double find_y_limit()
        {
            double max_abcd = Math.Max(Math.Abs(x1), Math.Abs(x2));
            double max_x = Math.Max(Math.Abs(start), Math.Abs(end));
            double y_limit = max_abcd * Math.Pow(max_x,3) + max_abcd * Math.Pow(max_x, 2) +
                max_abcd * max_x + max_abcd;
            return y_limit;
        }
        private double method1 ()
        {
            double start = change_sign[0, 0];
            double end = change_sign[1, 0];
            double mini_step = double.Parse(textBox12.Text);
            double x_exemplary = 1;
            double y_limit = find_y_limit();
            for (double x = start; x <= end; x += mini_step)
            {
                double y = calculate_y(x);
                if (Math.Abs(y) < y_limit)
                {
                    x_exemplary = x;
                    y_limit = Math.Abs(y);
                }
            }
            return x_exemplary;
        }
        private double method2 ()
        {
            double start = change_sign[0, 0];
            double end = change_sign[1, 0];
            double accuracy = double.Parse(textBox13.Text);
            double x_exemplary = start;
            while (end - start >= accuracy)
            {
                x_exemplary = (end + start) / 2;
                double y_start = calculate_y(start);
                double y_end = calculate_y(end);
                double y_curr = calculate_y(x_exemplary);
                if (y_curr == 0)
                {
                    return x_exemplary;
                }
                if (y_curr * y_start < 0)
                    end = x_exemplary;
                else
                    if (y_curr * y_end < 0)
                        start = x_exemplary;
            }
            return x_exemplary;
        }
        private void print_abcd()
        {
            textBox9.Text += $"A = {a}\t B = {b}\t C = {c}\t D = {d}";
            textBox9.Text += Environment.NewLine + Environment.NewLine;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            random_abcd();

            parse_abcd();

            print_equation();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox9.Text = "";

            parse_abcd();

            print_equation();

            print_abcd();

            print_xy();

            find_change_sign();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            double x_exemplary = method1();
            textBox10.Text = "";
            textBox10.Text += $"{x_exemplary:F9}";
            textBox10.Text += Environment.NewLine;
            double y = calculate_y(x_exemplary);
            textBox10.Text += $"{y:F9}";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            double x_exemplary = method2();
            textBox11.Text = "";
            textBox11.Text += $"{x_exemplary:F9}";
            textBox11.Text += Environment.NewLine;
            double y = calculate_y(x_exemplary);
            textBox11.Text += $"{y:F9}";
        }
    }
}