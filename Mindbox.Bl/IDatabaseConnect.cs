using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindbox.Bl
{
    /// <summary>
    /// Интерфейс соединения с БД
    /// </summary>
    public interface IDatabaseConnect
    {
        /// <summary>
        /// Возвращает строку соединения
        /// </summary>
        /// <returns></returns>
        string GetConnectionString();
    }
}
