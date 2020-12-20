using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindbox.Bl.DataModels
{
    /// <summary>
    /// Базовый интерфейс для данных фигуры
    /// </summary>
    public interface IFigureInput
    {
        /// <summary>
        /// Возвращает площадь фигуры
        /// </summary>
        /// <returns></returns>
        double GetArea();
    }
}
