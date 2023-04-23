using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ShapeDll;


namespace ShapesAndFigures
{
    [Serializable]
    public partial class Form1 : Form
    {
        public List<Shape> FigList;
        public Form2 form2;
        public Circle Jarvis;
        private string FileName;
        private bool isSaved = false;

        public Form1()
        {
            InitializeComponent();
            FigList = new List<Shape>();
            DoubleBuffered = true;
        }

        private void SaveFile()
        {
            NameCheck();
            BinaryFormatter bf = new BinaryFormatter();
            if (FileName != null)
            {
                FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, FigList);
                bf.Serialize(fs, Shape.C);
                bf.Serialize(fs, Shape.Radius);
                isSaved = true;
                fs.Close();             
            }
        }
        private void NameCheck()
        {
            if (FileName == null)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                    dialog.FilterIndex = 1;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        FileName = dialog.FileName;
                    }
                }
            }
        }
        private void SaveFileAs()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = dialog.FileName;
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                    bf.Serialize(fs, FigList);
                    bf.Serialize(fs, Shape.C);
                    bf.Serialize(fs, Shape.Radius);
                    isSaved = true;
                    fs.Close();
                }
            }
        }
        private void LoadFile()
        {
            if (!isSaved)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                string message = "Файл не сохранен! Продолжить?";
                string title = "Предупреждение";
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                        dialog.FilterIndex = 1;
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            FileName = dialog.FileName;
                            BinaryFormatter bf = new BinaryFormatter();
                            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                            FigList = (List<Shape>)bf.Deserialize(fs);
                            Shape.C = (Color)bf.Deserialize(fs);
                            Shape.Radius = (int)bf.Deserialize(fs);
                            isSaved = true;
                            fs.Close();
                        }                      
                    }
                }
            }
            else
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                    dialog.FilterIndex = 1;
                    dialog.RestoreDirectory = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        FileName = dialog.FileName;
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                        FigList = (List<Shape>)bf.Deserialize(fs);
                        Shape.C = (Color)bf.Deserialize(fs);
                        Shape.Radius = (int)bf.Deserialize(fs);
                        isSaved = true;
                        fs.Close();
                    }                   
                }
            }          
        }
        private void NewFile()
        {
            if (!isSaved)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                string message = "Файл не сохранен! Продолжить?";
                string title = "Предупреждение";
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    FigList.Clear();
                    isSaved = true;
                    Shape.r = 30;
                    Shape.C = Color.Red;
                    FileName = null;
                    Refresh();
                }
            }
            else
            {
                FigList.Clear();
                isSaved = true;
                Shape.r = 30;
                Shape.C = Color.Red;
                FileName = null;
                Refresh();
            }                             
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (FigList.Count > 2)
            {
                Pen p = new Pen(Color.Black, 5);
                int up, down;
                foreach (Shape i in FigList)
                {
                    i.lineDrawn = false;
                }
                if (поОпределениюToolStripMenuItem.Checked)
                {
                    for (int i = 0; i < FigList.Count; i++)//По определению
                    {

                        for (int j = 0; j < FigList.Count; j++)
                        {
                            down = 0;
                            up = 0;
                            float k, b;
                            if (FigList[i].x0 == FigList[j].x0)
                            {

                                for (int n = 0; n < FigList.Count; n++)
                                {
                                    if ((n != i) && (n != j))
                                    {
                                        if (FigList[n].x0 > FigList[i].x0)
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
                                k = ((float)FigList[j].y0 - (float)FigList[i].y0) / ((float)FigList[j].x0 - ((float)FigList[i].x0));
                                b = (float)FigList[i].y0 - k * FigList[i].x0;
                                for (int n = 0; n < FigList.Count; n++)
                                {
                                    if ((n != i) && (n != j))
                                    {
                                        if (FigList[n].y0 > k * FigList[n].x0 + b)
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
                                e.Graphics.DrawLine(p, FigList[i].x0, FigList[i].y0, FigList[j].x0, FigList[j].y0);
                                FigList[i].lineDrawn = true;
                                FigList[j].lineDrawn = true;
                            }
                        }
                    }
                }
                else if (jarvisToolStripMenuItem.Checked)
                {
                    //Jarvis
                    float vec1x, vec1y, vec2x, vec2y;
                    float mincos = 2;
                    float cosnow = 2;
                    int count = 0;
                    int idstart = -1;
                    int idsecond = -1;
                    for (int i = 0; i < FigList.Count; i++)
                    {
                        count = 0;
                        for (int j = 0; j < FigList.Count; j++)
                        {
                            if (i != j)
                            {
                                if (FigList[i].y0 > FigList[j].y0)
                                {
                                    count++;
                                }
                                else if (FigList[i].y0 == FigList[j].y0)
                                {
                                    if (FigList[i].x0 < FigList[j].x0)
                                    {
                                        count++;
                                    }
                                }
                            }
                            if (count == FigList.Count - 1)
                            {
                                idstart = i;
                            }
                        }
                    }
                    Jarvis = new Circle(FigList[idstart].x0 - 100, FigList[idstart].y0);
                    vec1x = (float)FigList[idstart].x0 - Jarvis.x0;
                    vec1y = (float)FigList[idstart].y0 - Jarvis.y0;
                    for (int k = 0; k < FigList.Count; k++)
                    {
                        for (int i = 0; i < FigList.Count; i++)
                        {
                            vec2x = (float)(FigList[idstart].x0 - FigList[i].x0);
                            vec2y = (float)(FigList[idstart].y0 - FigList[i].y0);
                            cosnow = (float)((vec1x * vec2x + vec1y * vec2y) / ((float)Math.Sqrt(vec1x * vec1x + vec1y * vec1y) * (float)Math.Sqrt(vec2x * vec2x + vec2y * vec2y)));
                            if (cosnow < mincos)
                            {
                                mincos = cosnow;
                                idsecond = i;
                            }
                        }
                        e.Graphics.DrawLine(p, FigList[idstart].x0, FigList[idstart].y0, FigList[idsecond].x0, FigList[idsecond].y0);
                        FigList[idstart].lineDrawn = true;
                        FigList[idsecond].lineDrawn = true;
                        Jarvis.x0 = FigList[idstart].x0;
                        Jarvis.y0 = FigList[idstart].y0;
                        idstart = idsecond;
                        idsecond = 0;
                        mincos = 2;
                        vec1x = (float)FigList[idstart].x0 - Jarvis.x0;
                        vec1y = (float)FigList[idstart].y0 - Jarvis.y0;
                    }
                }
            }
            int x = 0;
            foreach (Shape i in FigList)//D&D всего
            {
                if (!i.lineDrawn && x == FigList.Count - 1 && FigList.Count > 2)
                {

                }
                else
                {
                    i.draw(e.Graphics);
                }
                x++;
            }
        }
        private void UpdateRadius(object sender, RadiusEventArgs e)
        {
            isSaved = false;
            Shape.r = e.r;
            Refresh();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            bool move = false;
            foreach (Shape i in FigList)
            {
                if (i.isInside(e.X, e.Y))
                {
                    i.ISMOVING = true;
                    i.DX = i.x0 - e.X;
                    i.DY = i.y0 - e.Y;
                    move = true;
                    isSaved = false;
                }

            }
            if (move == false)
            {
                isSaved = false;
                if (кругToolStripMenuItem.Checked == true)
                {
                    Circle fig = new Circle(e.X, e.Y);
                    FigList.Add(fig);
                    Refresh();
                }
                else if (квадратToolStripMenuItem.Checked == true)
                {
                    Square fig = new Square(e.X, e.Y);
                    FigList.Add(fig);
                    Refresh();
                }
                else if (треугольникToolStripMenuItem.Checked == true)
                {
                    Triangle fig = new Triangle(e.X, e.Y);
                    FigList.Add(fig);
                    Refresh();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < FigList.Count; i++)
                {
                    if (FigList[i].isInside(e.X, e.Y))
                    {
                        FigList.RemoveAt(i);
                        isSaved = false;
                    }
                }
                Refresh();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Shape i in FigList)
            {
                if (i.isMoving)
                {
                    i.x0 = e.X + i.DX;
                    i.y0 = e.Y + i.DY;
                }
            }
            for (int i = 0; i < FigList.Count; i++)
            {
                if (!FigList[i].lineDrawn && FigList.Count > 2)
                {
                    if (i == FigList.Count - 1 && !FigList[i].isMoving)
                    {
                        isSaved = false;
                        for (int j = 0; j < FigList.Count - 1; j++)
                        {
                            FigList[j].ISMOVING = true;
                            FigList[j].DX = FigList[j].x0 - e.X;
                            FigList[j].DY = FigList[j].y0 - e.Y;                           
                            Refresh();
                        }
                    }
                    FigList.RemoveAt(i);
                }
            }
            Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Shape i in FigList)
            {
                i.ISMOVING = false;
                i.DX = 0;
                i.DY = 0;
            }
            if (FigList.Count > 2)
            {
                for (int i = 0; i < FigList.Count; i++)
                {
                    if (!FigList[i].lineDrawn)
                    {
                        FigList.RemoveAt(i);
                        i--;
                        Refresh();
                    }
                }
            }
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (кругToolStripMenuItem.Checked == false)
            {
                квадратToolStripMenuItem.Checked = false;
                треугольникToolStripMenuItem.Checked = false;
                кругToolStripMenuItem.Checked = true;
            }
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (треугольникToolStripMenuItem.Checked == false)
            {
                квадратToolStripMenuItem.Checked = false;
                треугольникToolStripMenuItem.Checked = true;
                кругToolStripMenuItem.Checked = false;
            }
        }

        private void квадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (квадратToolStripMenuItem.Checked == false)
            {
                квадратToolStripMenuItem.Checked = true;
                треугольникToolStripMenuItem.Checked = false;
                кругToolStripMenuItem.Checked = false;
            }
        }

        private void поОпределениюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!поОпределениюToolStripMenuItem.Checked)
            {
                поОпределениюToolStripMenuItem.Checked = true;
                jarvisToolStripMenuItem.Checked = false;
                сравнениеToolStripMenuItem.Checked = false;
            }

        }

        private void jarvisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!jarvisToolStripMenuItem.Checked)
            {
                jarvisToolStripMenuItem.Checked = true;
                поОпределениюToolStripMenuItem.Checked = false;
                сравнениеToolStripMenuItem.Checked = false;
            }

        }

        private void сравнениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form3 form3 = new Form3();
            //form3.Show();
        }

        private void цветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color delta = colorDialog1.Color;
                foreach (Shape i in FigList)
                {
                    i.gsColor = delta;
                }
                isSaved = false;
            }
        }

        private void радиусToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (form2 == null || form2.IsDisposed)
            {
                form2 = new Form2();
                form2.RadiusChanged += new RadiusDelegate(UpdateRadius);
                form2.Show();
            }
            else if (form2.WindowState == FormWindowState.Minimized)
            {
                form2.WindowState = FormWindowState.Normal;
            }
            else
            {
                form2.Focus();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }
    }
}