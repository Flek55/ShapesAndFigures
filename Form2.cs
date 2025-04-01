using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ShapeDll;

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
        {}

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (RadiusChanged != null)
            {
                RadiusChanged(this, new RadiusEventArgs(trackBar1.Value));
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}