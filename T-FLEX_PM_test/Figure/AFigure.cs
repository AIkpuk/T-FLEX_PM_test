namespace Infrastructure.Figure
{
    public abstract class AFigure
    {
        public abstract FigureType FigureType { get; }
        public abstract double[] Parameters { get; }
    }
}
