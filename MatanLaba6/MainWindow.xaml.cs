using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Colors = ScottPlot.Colors;

namespace MatanLaba6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var x0 = 2;
            var y0 = 10;
            var xn = 10;
            var h = 0.01;
            
            double[] resultEuler = Euler(func, x0, y0, xn, h);
            double[] x0result = new double[10000];
            
            double[] resultRungeKuttathOrder = RungeKuttathOrder(func, x0, y0, xn, h);
            double[] x1result = new double[10000];
            
            double[] resultRungeKutta4thOrder = RungeKutta4thOrder(func, x0, y0, xn, h);
            double[] x2result = new double[10000];
            
            double[] resultAdamsBashForth3 = AdamsThirdOrder(func, x0, y0, xn, h);
            double[] x3result = new double[10000];
            
            for (int i = 0; i < resultEuler.Length; i++)
            {
                x0result[i] = x0 + i * h;
                //Console.WriteLine($"x = {x0 + i * h}, y = {resultEuler[i]}");
            }
            for (int i = 0; i < resultRungeKuttathOrder.Length; i++)
            {
                x1result[i] = x0 + i * h;
                //Console.WriteLine($"x = {x0 + i * h}, y = {resultRungeKuttathOrder[i]}");
            }
            for (int i = 0; i < resultRungeKutta4thOrder.Length; i++)
            {
                x2result[i] = x0 + i * h;
                //Console.WriteLine($"x = {x0 + i * h}, y = {resultRungeKutta4thOrder[i]}");
            }
            for (int i = 0; i < resultAdamsBashForth3.Length; i++)
            {
                x3result[i] = x0 + i * h;
                //Console.WriteLine($"x = {x0 + i * h}, y = {resultAdamsBashForth3[i]}");
            }

            //WpfPlot1.Plot.Add.Scatter(x0result, resultEuler);
            //WpfPlot1.Plot.Add.Scatter(x1result, resultRungeKuttathOrder, Colors.Green);
            //WpfPlot1.Plot.Add.Scatter(x2result, resultRungeKutta4thOrder, Colors.Orange);
            WpfPlot1.Plot.Add.Scatter(x3result, resultAdamsBashForth3, Colors.Red);
        }

        static double func(double x, double y)
        {
            return Math.Sin(x) - y;
        }
        static double[] Euler(Func<double, double, double> f, double x0, double y0, double xn, double h)
        {
            int n = (int)((xn - x0) / h);
            double[] tValues = new double[n + 1];
            double[] yValues = new double[n + 1];
            tValues[0] = x0;
            yValues[0] = y0;

            for (int i = 0; i < n; i++)
            {
                yValues[i + 1] = yValues[i] + h * f(tValues[i], yValues[i]);
                tValues[i + 1] = tValues[i] + h;
            }

            return yValues;
        }
        static double[] RungeKuttathOrder(Func<double, double, double> f, double x0, double y0, double xn, double h)//метод Рунге–Кутты–Мерсона
        {
            int n = (int)((xn - x0) / h);
            double[] tValues = new double[n + 1];
            double[] yValues = new double[n + 1];
            tValues[0] = x0;
            yValues[0] = y0;

            for (int i = 0; i < n; i++)
            {
                double k1 = h * f(tValues[i], yValues[i]);
                double k2 = h * f(tValues[i] + h / 3, yValues[i] + k1 / 3);
                double k3 = h * f(tValues[i] + h / 3, yValues[i] + (k1 + k2) / 6);
                double k4 = h * f(tValues[i] + h / 2, yValues[i] + (k1 + 3 * k3) / 8);
                double k5 = h * f(tValues[i] + h, yValues[i] + (k1 - 3 * k3 + 4 * k4) / 2);
                yValues[i + 1] = yValues[i] + (k1 + 4 * k4 + k5) / 6;
                tValues[i + 1] = tValues[i] + h;
            }

            return yValues;
        }
        static double[] RungeKutta4thOrder(Func<double, double, double> f, double x0, double y0, double xn, double h)
        {
            int n = (int)((xn - x0) / h);
            double[] tValues = new double[n+1];
            double[] yValues = new double[n+1];

            tValues[0] = x0;
            yValues[0] = y0;

            for (int i = 0; i < n; i++)
            {
                double t = tValues[i];
                double y = yValues[i];

                double k1 = h * f(t, y);
                double k2 = h * f(t + 0.5 * h, y + 0.5 * k1);
                double k3 = h * f(t + 0.5 * h, y + 0.5 * k2);
                double k4 = h * f(t + h, y + k3);

                double yNew = y + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                yValues[i+1] = yNew;
                tValues[i + 1] = t + h;
            }

            return yValues;
        }
        public static double[] AdamsThirdOrder(Func<double, double, double> f, double x0, double y0, double xn, double h)
        {
            int n = (int)((xn - x0) / h);
            double[] x = new double[n + 1];
            double[] y = new double[n + 1];

            x[0] = x0;
            y[0] = y0;

            // Используем метод Рунге-Кутты четвертого порядка для вычисления первых трех значений
            for (int i = 0; i < 3; i++)
            {
                double k1 = h * f(x[i], y[i]);
                double k2 = h * f(x[i] + h / 2, y[i] + k1 / 2);
                double k3 = h * f(x[i] + h / 2, y[i] + k2 / 2);
                double k4 = h * f(x[i] + h, y[i] + k3);

                y[i + 1] = y[i] + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                x[i + 1] = x[i] + h;
            }

            // Применяем метод Адамса третьего порядка для остальных значений
            for (int i = 3; i <= n; i++)
            {
                y[i] = y[i - 1] + h * (23 * f(x[i - 1], y[i - 1]) - 16 * f(x[i - 2], y[i - 2]) + 5 * f(x[i - 3], y[i - 3])) / 12;
                x[i] = x[i - 1] + h;
            }

            return y;
        }

        
    }
    
}