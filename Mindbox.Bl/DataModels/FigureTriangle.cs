using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindbox.Bl.DataModels
{
    /// <summary>
    /// Треугольник
    /// </summary>
    public class FigureTriangle : IFigureInput
    {

        /// <summary>
        /// Возвращает площадь фигуры
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            double p = (_a+_b+_c)*0.5;
            double area = Math.Sqrt(p*(p-_a)*(p-_b)*(p-_c));
            return area;
        }

        /// <summary>
        /// Сторона треугольника
        /// </summary>
        private readonly double _a;

        /// <summary>
        /// Сторона треугольника
        /// </summary>
        private readonly double _b;

        /// <summary>
        /// Сторона треугольника
        /// </summary>
        private readonly double _c;

        /// <summary>
        /// Конструктор
        /// </summary>
        public FigureTriangle(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        /// <summary>
        /// Валидация входных данных для круга
        /// </summary>
        /// <param name="figureInputData">Массив входных данных</param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> Validate(double[] figureInputData)
        {

            // Это треугольник
            if (figureInputData.Length != 3)
            {
                yield return new ValidationResult(
                    $"Входная фигура треугольник, входных параметров должно быть 3, а не {figureInputData.Length}.",
                    new[] { nameof(figureInputData) });
            }
            else
            {
                bool invalid = false;
                // Первая сторона
                if (double.IsNaN(figureInputData[0]) || double.IsInfinity(figureInputData[0]))
                {
                    invalid = true;
                    yield return new ValidationResult(
                        $"Входная фигура треугольник, длина первой стороны не может быть такой, текущее значение {figureInputData[0]}.",
                        new[] {nameof(figureInputData)});
                }
                else if (double.IsNegative(figureInputData[0]) || figureInputData[0] == 0)
                {
                    invalid = true;
                    yield return new ValidationResult(
                        $"Входная фигура треугольник, длина первой стороны не может быть отрицательной или = 0, текущее значение {figureInputData[0]}.",
                        new[] { nameof(figureInputData) });
                }
                // Вторая сторона
                if (double.IsNaN(figureInputData[1]) || double.IsInfinity(figureInputData[1]))
                {
                    invalid = true;
                    yield return new ValidationResult(
                        $"Входная фигура треугольник, длина второй стороны не может быть такой, текущее значение {figureInputData[1]}.",
                        new[] {nameof(figureInputData)});
                }
                else if (double.IsNegative(figureInputData[1]) || figureInputData[1] == 0)
                {
                    invalid = true;
                    yield return new ValidationResult(
                        $"Входная фигура треугольник, длина второй стороны не может быть отрицательной или = 0, текущее значение {figureInputData[1]}.",
                        new[] { nameof(figureInputData) });
                }
                // Третья сторона
                if (double.IsNaN(figureInputData[2]) || double.IsInfinity(figureInputData[2]))
                {
                    invalid = true;
                    yield return new ValidationResult(
                        $"Входная фигура треугольник, длина третьей стороны не может быть такой, текущее значение {figureInputData[2]}.",
                        new[] {nameof(figureInputData)});
                } 
                else if (double.IsNegative(figureInputData[2]) || figureInputData[2] == 0)
                {
                    invalid = true;
                    yield return new ValidationResult(
                        $"Входная фигура треугольник, длина третьей стороны не может быть отрицательной или = 0, текущее значение {figureInputData[2]}.",
                        new[] {nameof(figureInputData)});
                }

                // Прошли все предыдущие проверки
                if (!invalid)
                {
                    // проверяем это вообще треугольник?
                    var list = new List<double>(figureInputData);
                    list.Sort();
                    // Сумма двух первых должна быть более последней
                    double ab = list[0] + list[1];
                    double c = list[2];
                    if (double.IsInfinity(ab))
                        yield return new ValidationResult(
                            $"Входная фигура треугольник, сумма двух меньших сторон больше double.MaxValue = {double.MaxValue}. Не возможно рассчитать площадь.",
                            new[] { nameof(figureInputData) });
                    else if (c > ab)
                        yield return new ValidationResult(
                            $"Входная фигура треугольник, сумма двух меньших сторон меньше максимальной стороны {c} < {ab}. Это не треугольник!",
                            new[] { nameof(figureInputData) });
                }
            }
        }
    }
}
