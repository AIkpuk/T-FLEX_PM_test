namespace Infrastructure.Figure
{
    public sealed class Rectangle : AFigure
    {
        public double Width { get; }
        public double Height { get; }
        public override FigureType FigureType => FigureType.Rectangle;
        public override double[] Parameters => new[] { Width, Height };

        public Rectangle(double width, double height)
        {
            if (width > 0 && height > 0)
            {
                Width = width;
                Height = height;
            }
            else
                throw new ArgumentOutOfRangeException("Ширина и высота должна быть больше нуля.");
        }
    }
}
