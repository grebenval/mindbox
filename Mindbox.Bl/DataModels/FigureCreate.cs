using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindbox.Bl.DataModels
{
    /// <summary>
    /// Запрос на создание фигуры
    /// </summary>
    public class FigureCreate : IValidatableObject
    {
        /// <summary>
        /// Тип фигуры
        /// 1 - круг
        /// 2 - треугольник
        /// </summary>
        [Range(1, 2)]
        public int FigureType { get; set; }


        /// <summary>
        /// Входные данные фигуры
        /// </summary>
        [Required]
        public double[] FigureInputData { get; set; }

        /// <summary>
        /// Массив валидаторов для фигур
        /// </summary>
        private static Func<double[], IEnumerable<ValidationResult>>[] _validatorsFigures =
            new Func<double[], IEnumerable<ValidationResult>>[] {FigureCircle.Validate, FigureTriangle.Validate};


        /// <summary>
        /// Валидация входного массива данных
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (FigureType == 1)
            {
                var validator = _validatorsFigures[FigureType - 1];
                // Проверяем круг
                foreach (var validationResult in validator(FigureInputData)) yield return validationResult;
            }
            else if (FigureType == 2)
            {
                var validator = _validatorsFigures[FigureType - 1];
                // Проверяем треугольник
                foreach (var validationResult1 in validator(FigureInputData)) yield return validationResult1;
            }
            else
            {
                // Не известная фигура 
                yield return new ValidationResult($"Входная фигура не определена, не известное значение, текущее значение {FigureType}.",
                    new[] { nameof(FigureInputData) });
            }
        }

        /// <summary>
        /// Создает фигуру согласно входным данным
        /// </summary>
        /// <returns></returns>
        public virtual IFigureInput Create()
        {
            if (FigureType == 1)
                return new FigureCircle(FigureInputData[0]);

            if (FigureType == 2)
                return new FigureTriangle(FigureInputData[0], FigureInputData[1], FigureInputData[2]);

            throw new Exception($"Не определен тип фигуры {FigureType}");
        }
    }
}
