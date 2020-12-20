using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindbox.Bl
{
    /// <summary>
    /// Соединение с БД
    /// </summary>
    public class DatabaseConnect : IDatabaseConnect
    {
        /// <summary>
        /// Строка соединения
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionString"> Строка соединения</param>
        public DatabaseConnect(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Возвращает строку соединения
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
