using Infrastructure.Figure;

namespace Infrastructure
{
    public static class FigureFactory
    {
        public static AFigure Create(FigureType type, params double[] parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            int expectedCount = GetParameterCount(type);
            if (parameters.Length != expectedCount)
            {
                throw new ArgumentException(
                    $"Для фигуры {type} требуется параметров: {expectedCount}. Получено: {parameters.Length}.",
                    nameof(parameters));
            }

            return type switch
            {
                FigureType.Circle => new Circle(parameters[0]),
                FigureType.Square => new Square(parameters[0]),
                FigureType.Triangle => new Triangle(parameters[0], parameters[1], parameters[2]),
                FigureType.Rectangle => new Rectangle(parameters[0], parameters[1]),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Неизвестный тип фигуры.")
            };
        }

        public static int GetParameterCount(FigureType type)
        {
            return type switch
            {
                FigureType.Circle => 1,
                FigureType.Square => 1,
                FigureType.Triangle => 3,
                FigureType.Rectangle => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Неизвестный тип фигуры.")
            };
        }
    }
}
