using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindbox.Bl.DataModels
{
    /// <summary>
    /// Круг
    /// </summary>
    public class FigureCircle : IFigureInput
    {
        private readonly double _radius;

        /// <summary>
        /// Возвращает площадь фигуры
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            double area = Math.PI * _radius * _radius;
            return area;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FigureCircle(double radius)
        {
            _radius = radius;
        }
        
        
        /// <summary>
        /// Максимальное значение для радиуса
        /// </summary>
        static readonly double _maxValue = Math.Sqrt(double.MaxValue) / Math.PI;

        /// <summary>
        /// Валидация входных данных для круга
        /// </summary>
        /// <param name="figureInputData"></param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> Validate(double[] figureInputData)
        {
            
            // Это круг
            if (figureInputData.Length != 1)
            {
                yield return new ValidationResult(
                    $"Входная фигура круг, параметр входной только один - радиус, а не {figureInputData.Length}.",
                    new[] { nameof(figureInputData) });
            }
            else if (double.IsNaN(figureInputData[0]) || double.IsInfinity(figureInputData[0]))
            {
                yield return new ValidationResult(
                    $"Входная фигура круг, радиус не может быть таким, текущее значение {figureInputData[0]}.",
                    new[] { nameof(figureInputData) });
            }
            else if (double.IsNegative(figureInputData[0]))
            {
                yield return new ValidationResult(
                    $"Входная фигура круг, радиус не может отрицательным, текущее значение {figureInputData[0]}.",
                    new[] { nameof(figureInputData) });
            }
            else if (figureInputData[0] > _maxValue)
            {
                yield return new ValidationResult(
                    $"Входная фигура круг, радиус не может быть более - {_maxValue}, текущее значение {figureInputData[0]}.",
                    new[] { nameof(figureInputData) });
            }
        }
    }
}
