using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindbox.Bl;
using Mindbox.Database.Sqlite.Data;

namespace Mindbox.Database.Sqlite.XUnitTest
{
    public class DataSqliteNullContext : DataSqlite
    {
        public DataSqliteNullContext(IDatabaseConnect databaseConnect) : base(databaseConnect)
        {
        }

        /// <summary>
        /// Возвращает контекст
        /// </summary>
        /// <returns></returns>
        public override MindboxContext GetContext()
        {
            return null;
        }
    }
}
