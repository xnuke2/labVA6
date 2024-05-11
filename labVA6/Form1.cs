using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labVA6
{
    public partial class Form1 : Form
    {
        int numOfPoints = 10;
        double min = 0;
        double max = 2;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd= new Random();
            for (int i = 1;i< dataGridView1.Columns.Count;i++)
            {
                dataGridView1.Rows[0].Cells[i].Value= Convert.ToString(rnd.Next(0,2)+ rnd.NextDouble());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(1);
            dataGridView1.Rows[0].Cells[0].Value = "X";
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = 0;
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            int length = dataGridView1.Columns.Count;
            double[] X= new double[length - 1];
            double[] Y = new double[length - 1];
            for (int i = 1; i < length; i++)
            {
                X[i - 1] =Convert.ToDouble( dataGridView1.Rows[0].Cells[i].Value);
                
            }
            Array.Sort(X);
            for (int i = 1; i < length; i++)
            {
                Y[i - 1] = find(X[i - 1]);
            }
            CubeSpline cubeSpline =new CubeSpline(X, Y);
            chart1.Series[1].Points.AddXY(min, cubeSpline.Interpolate(min));
            chart1.Series[0].Points.AddXY(min, find(min));
            double lenBetweenPoints = (max-min)/(numOfPoints-1);
            for (int i = 0; i < numOfPoints-2; i++) 
            {
                chart1.Series[1].Points.AddXY(min+lenBetweenPoints*(i+1), cubeSpline.Interpolate(min+lenBetweenPoints * (i + 1)));
                chart1.Series[0].Points.AddXY(min + lenBetweenPoints * (i + 1), find(min + lenBetweenPoints * (i + 1)));
            }
            chart1.Series[1].Points.AddXY(max, cubeSpline.Interpolate(max));
            chart1.Series[0].Points.AddXY(max, find(max));


        }
        private double find(double x)
        {
            return Math.Pow(Math.E, Math.Sin(x));
        }
    }
}
