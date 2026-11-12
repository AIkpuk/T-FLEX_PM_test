using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Infrastructure;
using Infrastructure.Figure;
using WpfRectangle = System.Windows.Shapes.Rectangle;

namespace View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            FigureTypeComboBox.ItemsSource = Enum.GetValues<FigureType>();
            FigureTypeComboBox.SelectedIndex = 0;
        }

        private void FigureTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FigureTypeComboBox.SelectedItem is not FigureType type)
                return;

            ParametersPanel.Children.Clear();

            string[] labels = GetParameterLabels(type);
            int parameterCount = FigureFactory.GetParameterCount(type);

            for (int i = 0; i < parameterCount; i++)
            {
                ParametersPanel.Children.Add(new TextBlock
                {
                    Text = labels[i],
                    Margin = new Thickness(0, 0, 0, 5)
                });

                ParametersPanel.Children.Add(new TextBox
                {
                    Height = 28,
                    Margin = new Thickness(0, 0, 0, 10)
                });
            }

            FigureCanvas.Children.Clear();
        }

        private static string[] GetParameterLabels(FigureType type)
        {
            return type switch
            {
                FigureType.Circle => new[] { "Радиус" },
                FigureType.Square => new[] { "Сторона" },
                FigureType.Rectangle => new[] { "Ширина", "Высота" },
                FigureType.Triangle => new[] { "Сторона A", "Сторона B", "Сторона C" },
                _ => Array.Empty<string>()
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FigureTypeComboBox.SelectedItem is not FigureType type)
                return;

            List<double> parameters = new();

            foreach (object element in ParametersPanel.Children)
            {
                if (element is not TextBox textBox)
                    continue;

                if (!double.TryParse(
                        textBox.Text,
                        NumberStyles.Float,
                        CultureInfo.CurrentCulture,
                        out double value))
                {
                    MessageBox.Show("Введите корректные числовые параметры.", "Некорректный ввод",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    textBox.Focus();
                    return;
                }

                parameters.Add(value);
            }

            try
            {
                AFigure figure = FigureFactory.Create(type, parameters.ToArray());
                FigureCanvas.Children.Clear();
                DrawFigure(figure);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка создания фигуры",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DrawFigure(AFigure figure)
        {
            switch (figure.FigureType)
            {
                case FigureType.Circle:
                    DrawCircle(figure.Parameters[0]);
                    break;
                case FigureType.Square:
                    DrawRectangle(figure.Parameters[0], figure.Parameters[0], Brushes.LightGreen);
                    break;
                case FigureType.Rectangle:
                    DrawRectangle(figure.Parameters[0], figure.Parameters[1], Brushes.LightBlue);
                    break;
                case FigureType.Triangle:
                    DrawTriangle(figure.Parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(figure.FigureType));
            }
        }

        private void DrawCircle(double radius)
        {
            double diameter = radius * 2;
            double scale = GetScale(diameter, diameter);
            double size = diameter * scale;

            Ellipse circle = new()
            {
                Width = size,
                Height = size,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.LightBlue
            };

            Canvas.SetLeft(circle, (FigureCanvas.ActualWidth - size) / 2);
            Canvas.SetTop(circle, (FigureCanvas.ActualHeight - size) / 2);
            FigureCanvas.Children.Add(circle);
        }

        private void DrawRectangle(double width, double height, Brush fill)
        {
            double scale = GetScale(width, height);
            double drawWidth = width * scale;
            double drawHeight = height * scale;

            WpfRectangle rectangle = new()
            {
                Width = drawWidth,
                Height = drawHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = fill
            };

            Canvas.SetLeft(rectangle, (FigureCanvas.ActualWidth - drawWidth) / 2);
            Canvas.SetTop(rectangle, (FigureCanvas.ActualHeight - drawHeight) / 2);
            FigureCanvas.Children.Add(rectangle);
        }

        private void DrawTriangle(double[] sides)
        {
            double a = sides[0];
            double b = sides[1];
            double c = sides[2];

            // Основание равно стороне A; расстояния до третьей вершины — C и B.
            double x = (a * a + c * c - b * b) / (2 * a);
            double ySquared = Math.Max(0, c * c - x * x);
            double y = Math.Sqrt(ySquared);

            double scale = GetScale(a, y);
            double drawWidth = a * scale;
            double drawHeight = y * scale;
            double offsetX = (FigureCanvas.ActualWidth - drawWidth) / 2;
            double offsetY = (FigureCanvas.ActualHeight - drawHeight) / 2;

            Polygon triangle = new()
            {
                Points = new PointCollection
                {
                    new Point(offsetX, offsetY + drawHeight),
                    new Point(offsetX + drawWidth, offsetY + drawHeight),
                    new Point(offsetX + x * scale, offsetY)
                },
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.LightGoldenrodYellow
            };

            FigureCanvas.Children.Add(triangle);
        }

        private double GetScale(double width, double height)
        {
            double availableWidth = Math.Max(1, FigureCanvas.ActualWidth - 40);
            double availableHeight = Math.Max(1, FigureCanvas.ActualHeight - 40);

            return Math.Min(availableWidth / width, availableHeight / height);
        }
    }
}
