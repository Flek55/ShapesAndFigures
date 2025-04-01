using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ShapeDll
{
    [Serializable]
    abstract public class Shape
    {
        public static int Radius;
        public static Color C;
        public bool isMoving;
        public bool lineDrawn;
        protected int x, y;
        protected int Dx, Dy;
        static Shape()
        {
            Radius = 30;
            C = Color.Red;
        }
        public Shape(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.isMoving = false;
            this.lineDrawn = false;
        }
        public int x0
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int y0
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public int DX
        {
            get
            {
                return Dx;
            }
            set
            {
                Dx = value;
            }
        }
        public int DY
        {
            get
            {
                return Dy;
            }
            set
            {
                Dy = value;
            }
        }
        public static int r
        {
            get
            {
                return Radius;
            }
            set
            {
                Radius = value;
            }
        }
        public Color gsColor
        {
            get
            {
                return C;
            }
            set { C = value; }
        }
        public bool ISMOVING
        {
            get
            {
                return isMoving;
            }
            set
            {
                isMoving = value;
            }
        }
        public abstract void draw(Graphics G);
        public abstract bool isInside(int mouse_x, int mouse_y);
    }
    [Serializable]
    public class Circle : Shape
    {
        public Circle(int x, int y) : base(x, y)
        { }
        public override bool isInside(int mouse_x, int mouse_y)
        {
            return Math.Sqrt((mouse_x - x) * (mouse_x - x) + (mouse_y - y) * (mouse_y - y)) <= r;
        }
        public override void draw(Graphics G)
        {
            Brush B = new SolidBrush(gsColor);
            G.FillEllipse(B, x0 - r, y0 - r, 2 * r, 2 * r);
        }
    }
    [Serializable]
    public class Square : Shape
    {
        public Square(int x, int y) : base(x, y)
        { }
        public override bool isInside(int mouse_x, int mouse_y)
        {
            return mouse_x > this.x - r / Math.Sqrt(2) && mouse_x < this.x + r / Math.Sqrt(2) && mouse_y > this.y - r / Math.Sqrt(2) && mouse_y < this.y + r / Math.Sqrt(2);
        }
        public override void draw(Graphics G)
        {
            Brush B = new SolidBrush(gsColor);
            G.FillRectangle(B, x - (int)(r * 2 / Math.Sqrt(2) / 2), y - (int)(r * 2 / Math.Sqrt(2) / 2), (int)(r * 2 / Math.Sqrt(2)), (int)(r * 2 / Math.Sqrt(2)));
        }
    }
    [Serializable]
    public class Triangle : Shape
    {
        public Triangle(int x, int y) : base(x, y)
        { }
        public override bool isInside(int mouse_x, int mouse_y)
        {
            Point p1 = new Point(x0, y0 - r);
            Point p2 = new Point(x0 - (int)(r * Math.Sqrt(3) / 2), y0 + r / 2);
            Point p3 = new Point(x0 + (int)(r * Math.Sqrt(3) / 2), y0 + r / 2);
            Point[] points = { p1, p2, p3 };
            int a = (points[0].X - mouse_x) * (points[1].Y - points[0].Y) - (points[1].X - points[0].X) * (points[0].Y - mouse_y);
            int b = (points[1].X - mouse_x) * (points[2].Y - points[1].Y) - (points[2].X - points[1].X) * (points[1].Y - mouse_y);
            int c = (points[2].X - mouse_x) * (points[0].Y - points[2].Y) - (points[0].X - points[2].X) * (points[2].Y - mouse_y);
            return (a <= 0 && b <= 0 && c <= 0);
        }
        public override void draw(Graphics G)
        {
            Brush B = new SolidBrush(gsColor);
            Point p1 = new Point(x0, y0 - r);
            Point p2 = new Point(x0 - (int)(r * Math.Sqrt(3) / 2), y0 + r / 2);
            Point p3 = new Point(x0 + (int)(r * Math.Sqrt(3) / 2), y0 + r / 2);
            Point[] points = { p1, p2, p3 };
            G.FillPolygon(B, points);
        }
    }
}
