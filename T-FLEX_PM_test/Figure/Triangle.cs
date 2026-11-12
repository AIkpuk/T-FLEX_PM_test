namespace Infrastructure.Figure
{
    public sealed class Triangle : AFigure
    {
        public double SideA { get; }
        public double SideB { get; }
        public double SideC { get; }
        public override FigureType FigureType => FigureType.Triangle;
        public override double[] Parameters => new[] { SideA, SideB, SideC };

        public Triangle(double sideA, double sideB, double sideC)
        {
            if (sideA > 0 && sideB > 0 && sideC > 0)
            {
                if (sideA + sideB <= sideC ||
                    sideA + sideC <= sideB ||
                    sideB + sideC <= sideA)
                {
                    throw new ArgumentException("Треугольник с такими сторонами не существует.");
                }

                SideA = sideA;
                SideB = sideB;
                SideC = sideC;
            }
            else
                throw new ArgumentOutOfRangeException("Сторона должна быть больше нуля.");

        }
    }
}
