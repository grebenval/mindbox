using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindbox.Database.Sqlite.Data;

namespace Mindbox.Database.Sqlite
{
    /// <summary>
    /// Специализация фигуры, позволяет добавлять
    /// фигуру для связи при создании фигуры
    /// </summary>
    public static class FigureExtension
    {
        /// <summary>
        /// Добавляем фигуру 
        /// </summary>
        /// <param name="figure">Фигура</param>
        /// <param name="array">массив параметров</param>
        public static void AddSpecFigure(this Figure figure, double[] array)
        {
            // Контроль входных данных
            if (ReferenceEquals(null, array) || array.Length == 0)
                throw new Exception( "Фигура без входных параметров не может быть сохранена!");

            // круг
            if (figure.Type == 1)
            {
                if (array.Length != 1)
                    throw new Exception("Круг должен иметь один входной параметр!");
                // Создаем круг и добавляем
                figure.Circle = new Circle {Radius = array[0]};
                return;
            }

            // треугольник
            if (figure.Type == 2)
            {
                if (array.Length != 3)
                    throw new Exception("Треугольник должен иметь три входных параметра!");
                // Создаем треугольник и добавляем
                figure.Triangle = new Triangle() { A = array[0], B = array[1], C = array[2] };
                return;
            }

            // Если дошли до этого места, то тип фигуры не поддержан
            throw new Exception($"Не известный тип фигуры {figure.Type}!");
        }
    }
}
