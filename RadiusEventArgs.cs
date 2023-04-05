namespace ShapesAndFigures
{
    public delegate void RadiusDelegate(object sender, RadiusEventArgs e);
    public class RadiusEventArgs
    {
        private int Radius;
        public RadiusEventArgs()
        {
            Radius = 30;
        }
        public RadiusEventArgs(int Radius)
        {
            this.Radius = Radius;
        }
        public int r
        {
            get
            {
                return Radius;
            }
        }
    }
}