using System.Threading.Tasks;

namespace Mindbox.Bl.Bl
{
    /// <summary>
    /// Интерфейс взаимодействия с БД хранящих фигуры
    /// </summary>
    public interface IFigureManipulation
    {
        /// <summary>
        /// Добавляет фигуру в БД
        /// </summary>
        /// <param name="figuretype">Тип фигуры</param>
        /// <param name="area">Площадь фигуры</param>
        /// <param name="array">Массив параметров фигуры</param>
        /// <returns>ID - добавленной фигуры</returns>
        public Task<(bool, long, string)> FigureInsert(int figuretype, double area, double[] array);

        /// <summary>
        /// Получает площадь фигуры из БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns>успех/неудача,значение,сообщение об ошибке</returns>
        public Task<(bool, double, string)> FigureGet(long id);

    }
}
