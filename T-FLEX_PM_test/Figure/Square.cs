namespace Infrastructure.Figure
{
    public sealed class Square : AFigure
    {
        public double Side { get; }
        public override FigureType FigureType => FigureType.Square;
        public override double[] Parameters => new[] { Side };

        public Square(double side)
        {
            if (side > 0)
                Side = side;
            else
                throw new ArgumentOutOfRangeException(nameof(side), "Сторона должна быть конечным числом больше нуля.");
        }
    }
}
