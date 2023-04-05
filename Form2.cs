using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ShapeAndOther;

namespace ShapesAndFigures
{
    [Serializable]
    public partial class Form2 : Form
    {
        public event RadiusDelegate RadiusChanged;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            trackBar1.Value = Shape.r;
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Test()
        {
            List<Shape> Analyse = new List<Shape>();
            Random rand = new Random();
            Pen p = new Pen(Color.Black, 5);
            Circle Jarvis;
            DateTime start = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                Circle fig = new Circle(rand.Next(0, 1500), rand.Next(0, 1000));
                Analyse.Add(fig);
            }
            int up, down;
            for (int i = 0; i < Analyse.Count; i++)//По определению
            {

                for (int j = 0; j < Analyse.Count; j++)
                {
                    down = 0;
                    up = 0;
                    float k, b;
                    if (Analyse[i].x0 == Analyse[j].x0)
                    {

                        for (int n = 0; n < Analyse.Count; n++)
                        {
                            if ((n != i) && (n != j))
                            {
                                if (Analyse[n].x0 > Analyse[i].x0)
                                {
                                    up++;
                                }
                                else
                                {
                                    down++;
                                }
                            }
                        }
                    }
                    else
                    {
                        k = ((float)Analyse[j].y0 - (float)Analyse[i].y0) / ((float)Analyse[j].x0 - ((float)Analyse[i].x0));
                        b = (float)Analyse[i].y0 - k * Analyse[i].x0;
                        for (int n = 0; n < Analyse.Count; n++)
                        {
                            if ((n != i) && (n != j))
                            {
                                if (Analyse[n].y0 > k * Analyse[n].x0 + b)
                                {
                                    up++;
                                }
                                else
                                {
                                    down++;
                                }
                            }
                        }
                    }
                    if (up * down == 0)
                    {
                        Analyse[i].lineDrawn = true;
                        Analyse[j].lineDrawn = true;
                    }
                }
            }
            DateTime finish = DateTime.Now;
            TimeSpan ans = finish - start;
            //Jarvis
            start = DateTime.Now;
            float vec1x, vec1y, vec2x, vec2y;
            float mincos = 2;
            float cosnow = 2;
            int count = 0;
            int idstart = -1;
            int idsecond = -1;
            for (int i = 0; i < Analyse.Count; i++)
            {
                count = 0;
                for (int j = 0; j < Analyse.Count; j++)
                {
                    if (i != j)
                    {
                        if (Analyse[i].y0 > Analyse[j].y0)
                        {
                            count++;
                        }
                        else if (Analyse[i].y0 == Analyse[j].y0)
                        {
                            if (Analyse[i].x0 < Analyse[j].x0)
                            {
                                count++;
                            }
                        }
                    }
                    if (count == Analyse.Count - 1)
                    {
                        idstart = i;
                    }
                }
            }
            Jarvis = new Circle(Analyse[idstart].x0 - 100, Analyse[idstart].y0);
            vec1x = (float)Analyse[idstart].x0 - Jarvis.x0;
            vec1y = (float)Analyse[idstart].y0 - Jarvis.y0;
            for (int k = 0; k < Analyse.Count; k++)
            {
                for (int i = 0; i < Analyse.Count; i++)
                {
                    vec2x = (float)(Analyse[idstart].x0 - Analyse[i].x0);
                    vec2y = (float)(Analyse[idstart].y0 - Analyse[i].y0);
                    cosnow = (float)((vec1x * vec2x + vec1y * vec2y) / ((float)Math.Sqrt(vec1x * vec1x + vec1y * vec1y) * (float)Math.Sqrt(vec2x * vec2x + vec2y * vec2y)));
                    if (cosnow < mincos)
                    {
                        mincos = cosnow;
                        idsecond = i;
                    }
                }
                Analyse[idstart].lineDrawn = true;
                Analyse[idsecond].lineDrawn = true;
                Jarvis.x0 = Analyse[idstart].x0;
                Jarvis.y0 = Analyse[idstart].y0;
                idstart = idsecond;
                idsecond = 0;
                mincos = 2;
                vec1x = (float)Analyse[idstart].x0 - Jarvis.x0;
                vec1y = (float)Analyse[idstart].y0 - Jarvis.y0;
            }
            finish = DateTime.Now;
            ans = finish - start;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (RadiusChanged != null)
            {
                RadiusChanged(this, new RadiusEventArgs(trackBar1.Value));
            }
        }
    }
}