using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mapaa
{
    public partial class Form1 : Form
    {
        System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        System.Drawing.Graphics formGraphics;// = this.CreateGraphics();
        Thread thread = null;
        Starting s = null;
        private int samochody = 2;
        private int pojemnosc = 2;
        public String dataa = "-";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PerformCalculations("Próbny tekst", "nagłowek");
        }
        public void DrawVertices(int color, String label, int x, int y)
        {

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            if (color == 0)
                myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            if (color == 2)
                myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Orange);
            if (color == 1)
                myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
            formGraphics = this.CreateGraphics();
            formGraphics.FillEllipse(myBrush, new Rectangle(x, y, 25, 25));

            string drawString = label;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            x = x + 2;
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)//rozpoczyna program
        {
            if (s == null)
            {
                thread = new Thread(new ThreadStart(newThread));
                thread.Start();
            }
        }
        public void newThread()
        {
            s = new Starting(this);
            s.iloscSamochodow = samochody;
            ustawLiczbe();
            s.pojemnoscSamochodu = pojemnosc;
            s.Start(@"E:\mapaa\mapaa\przesylkii.txt");
        }

        private void button3_Click(object sender, EventArgs e)//przerywa program
        {
            try
            {
                thread.Abort();
                Application.Exit();
            }
            catch (NullReferenceException)
            {
                Application.Exit();
            }

        }
        public void DrawConnection(int x, int a, int b, int c, int d)//rysuje połączenia na grafie
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            if (x == 1)
                myPen = new System.Drawing.Pen(System.Drawing.Color.Yellow);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.DrawLine(myPen, a, b, c, d);
            myPen.Dispose();
            formGraphics.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)//Przycisk "X", zamyka okienko, wymusza przerwanie programu
        {
            try
            {
                thread.Abort();
                Application.Exit();
            }
            catch (NullReferenceException)
            {
                Application.Exit();
            }
        }
        public void kom(String text)
        {
            MessageBox.Show(text, "My Application", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (thread == null)
            {
                label1.Text = "Ilość Samochodów:" + trackBar1.Value.ToString();
                samochody = trackBar1.Value;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (thread == null)
            {
                label2.Text = "Pojemność samochodu:" + trackBar2.Value.ToString();
                pojemnosc = trackBar2.Value;
            }
        }

        private void ustawLiczbe()
        {
        }



    }
}
