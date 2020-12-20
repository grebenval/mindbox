using System;
using System.Reflection;
using Mindbox.Bl;
using Mindbox.Database.Sqlite.Data;
using Xunit;

namespace Mindbox.Database.Sqlite.XUnitTest
{
    public class FigureFabricTest
    {
        [Fact]
        public void TestConstructor()
        {

            try
            {
                Figure figure = new Figure();
                // На вход null массив
                figure.AddSpecFigure(null);
                Assert.True(false, "Не должны попасть сюда, бросается исключение.");
            }
            catch (Exception e)
            {
                Assert.Contains("Фигура без входных параметров не может быть сохранена!", e.Message);
            }

            try
            {
                Figure figure = new Figure();
                // На вход Empty массив
                figure.AddSpecFigure(Array.Empty<double>());
                Assert.True(false, "Не должны попасть сюда, бросается исключение.");
            }
            catch (Exception e)
            {
                Assert.Contains("Фигура без входных параметров не может быть сохранена!", e.Message);
            }

            try
            {
                Figure figure = new Figure();
                figure.Type = -1;
                // тип фигуры не поддержан
                figure.AddSpecFigure(new[] {0.1D});
                Assert.True(false, "Не должны попасть сюда, бросается исключение.");
            }
            catch (Exception e)
            {
                Assert.Contains("Не известный тип фигуры", e.Message);
            }

            {
                // Круг
                Figure figure = new Figure {Type = 1};
                figure.AddSpecFigure(new[] {0.1D});

                Assert.NotNull(figure.Circle);
                Assert.Equal(0.1D, figure.Circle.Radius);
            }

            try
            {

                // Круг не корректное число входных параметров
                Figure figure = new Figure {Type = 1};
                figure.AddSpecFigure(new[] {0.1D, 0.1D});
                Assert.True(false, "Не должны попасть сюда, бросается исключение.");

            }
            catch (Exception e)
            {
                Assert.Contains("Круг должен иметь один входной параметр!", e.Message);
            }

            {
                // Треугольник
                Figure figure = new Figure {Type = 2};
                figure.AddSpecFigure(new[] {1d, 3d, 3d});

                Assert.NotNull(figure.Triangle);
                Assert.Equal(1d, figure.Triangle.A);
                Assert.Equal(3d, figure.Triangle.B);
                Assert.Equal(3d, figure.Triangle.C);
            }

            try
            {
                // Треугольник не корректное число входных параметров
                Figure figure = new Figure {Type = 2};
                figure.AddSpecFigure(new[] {1d, 3d});
                Assert.True(false, "Не должны попасть сюда, бросается исключение.");

            }
            catch (Exception e)
            {
                Assert.Contains("Треугольник должен иметь три входных параметра!", e.Message);
            }
        }
    }
}