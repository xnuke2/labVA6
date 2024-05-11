using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labVA6
{
    class CubeSpline
    {
        SplineTuple[] splines; // Сплайн

        // Структура, описывающая сплайн на каждом сегменте сетки
        private struct SplineTuple
        {
            public double a, b, c, d, x;
        }

        // Построение сплайна
        // x - узлы сетки, должны быть упорядочены по возрастанию, кратные узлы запрещены
        // y - значения функции в узлах сетки
        public CubeSpline(double[] x, double[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException("Длины массивов не совпадают");
            int n =x.Length;
            // Инициализация массива сплайнов
            splines = new SplineTuple[n];
            for (int i = 0; i < n; i++)
            {
                splines[i].x = x[i];
                splines[i].a = y[i];
            }
            splines[0].c = splines[n - 1].c = 0.0;

            
            // прямой ход метода прогонки
            double[] alpha = new double[n - 1];
            double[] beta = new double[n - 1];
            alpha[0] = beta[0] = 0.0;
            for (int i = 1; i < n - 1; ++i)
            {
                double hi = x[i] - x[i - 1];
                double hiplus1 = x[i + 1] - x[i];
                double A = hi;
                double C = 2.0 * (hi + hiplus1);
                double B = hiplus1;
                double F = 3.0 * ((y[i + 1] - y[i]) / hiplus1 - (y[i] - y[i - 1]) / hi);
                double z = (A * alpha[i - 1] + C);
                alpha[i] = -B / z;
                beta[i] = (F - A * beta[i - 1]) / z;
            }
            /*
                C B 0 0 ... F
                A1 C1 B1 0 ... F1
                0 A2 B2 C2 ... F2
             

             */
            // обратный ход метода прогонки
            for (int i = n - 2; i > 0; --i)
            {
                splines[i].c =( alpha[i] * splines[i + 1].c + beta[i]) ;
            }

            
            for (int i = n - 1; i > 0; --i)
            {
                double hi = x[i] - x[i - 1];
                splines[i].d = ((splines[i].c - splines[i - 1].c) / hi) / 6.0;
                splines[i].b =( hi * (2.0 * splines[i].c + splines[i - 1].c) / 6.0 + (y[i] - y[i - 1]) / hi ) ;
            }
            for(int i = 0; i < splines.Length; i++)
            {
                splines[i].c /= 2.0;
            }
        }

        // Вычисление значения интерполированной функции в произвольной точке
        public double Interpolate(double x)
        {
            int n = splines.Length;
            SplineTuple s;

            if (x <= splines[0].x) 
            {
                s = splines[0];
            }
            else if (x >= splines[n - 1].x) 
            {
                s = splines[n - 1];
            }
            else 
            {
                int i = 0;
                int j = n - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (x <= splines[k].x)
                    {
                        j = k;
                    }
                    else
                    {
                        i = k;
                    }
                }
                s = splines[j];
            }

            double dx = x - s.x;
            // Вычисляем значение сплайна 
            return s.a + (s.b + (s.c  + s.d * dx) * dx) * dx;
        }
    }
}
