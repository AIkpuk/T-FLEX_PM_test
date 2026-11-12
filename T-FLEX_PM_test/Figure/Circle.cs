namespace Infrastructure.Figure
{
    public sealed class Circle : AFigure
    {
        public double Radius { get; }
        public override FigureType FigureType => FigureType.Circle;
        public override double[] Parameters => new[] { Radius };

        public Circle(double radius)
        {
            if (radius > 0)
                Radius = radius;
            else
                throw new ArgumentOutOfRangeException(nameof(radius), "Радиус должен быть конечным числом больше нуля.");
        }
    }
}
