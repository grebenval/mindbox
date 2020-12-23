using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mindbox.Bl;
using Mindbox.Bl.Bl;
using Mindbox.Database.Sqlite.Data;

namespace Mindbox.Database.Sqlite
{
    public class DataSqlite : IFigureManipulation
    {
        ///// <summary>
        ///// Строка соединения
        ///// </summary>
        //protected readonly string ConnectionString;

        protected readonly MindboxContext MindboxContext;

        ///// <summary>
        ///// Конструктор
        ///// </summary>
        ///// <param name="databaseConnect">Интерфейс соединения с БД</param>
        //public DataSqlite(MindboxContext mindboxContext)
        //{
        //    //ConnectionString = databaseConnect.GetConnectionString();
        //}

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="mindboxContext">Контекст БД</param>
        public DataSqlite(MindboxContext mindboxContext)
        {
            MindboxContext = mindboxContext;
        }

        /// <summary>
        /// Возвращает контекст
        /// </summary>
        /// <returns></returns>
        public virtual MindboxContext GetContext()
        {
            // return new MindboxContext(ConnectionString);
            return MindboxContext;
        }

        /// <summary>
        /// Добавляет фигуру в БД
        /// </summary>
        /// <param name="figuretype">Тип фигуры</param>
        /// <param name="area">Площадь</param>
        /// <param name="array">Массив параметров фигуры</param>
        /// <returns>ID - добавленной фигуры</returns>
        public async Task<(bool, long, string)> FigureInsert(int figuretype, double area, double[] array)
        {
            if (array == null || array.Length == 0)
                return (false, 0, "Фигура без входных параметров не может быть сохранена!");

            (bool, long, string) result;

            try
            {
                await using var context = GetContext();
                
                Figure figure = new Figure { Type = figuretype, Area = area };
                // Добавляем специализацию фигуры
                figure.AddSpecFigure(array);
                await using var transaction = await context.Database.BeginTransactionAsync();

                var blog = await context.Figures.AddAsync(figure);

                await context.SaveChangesAsync();
                long id = figure.Id;
                
               
                await transaction.CommitAsync();

                result = (true, id, null);
            }
            catch (Exception e)
            {
                return (false, 0, e.Message);
            }

            return result;
        }

        /// <summary>
        /// Получает площадь фигуры из БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns>успех/неудача,значение,сообщение об ошибке</returns>
        public async Task<(bool, double, string)> FigureGet(long id)
        {
            (bool, double, string) result;
            try
            {
                await using var context = GetContext();
                var figure = await context.Figures.SingleAsync(f => f.Id == id);
                double area = figure.Area;
                result = (true, area, null);
            }
            catch (Exception e)
            {
                return (false, 0, e.Message);
            }

            return result;
        }
    }
}
