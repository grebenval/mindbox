using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Mindbox.Bl;
using Mindbox.Database.Sqlite.Data;

namespace Mindbox.Database.Sqlite.XUnitTest
{
    public class DataSqliteInMemoryMode : DataSqlite, IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        
        private readonly SqliteConnection _connection;

        protected readonly MindboxContext DbContext;

        public DataSqliteInMemoryMode(IDatabaseConnect databaseConnect) : base(databaseConnect)
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<MindboxContext>()
                .UseSqlite(_connection)
                .Options;
            DbContext = new MindboxContext(options);
            DbContext.Database.EnsureCreated();
        }

        /// <summary>
        /// Возвращает контекст
        /// </summary>
        /// <returns></returns>
        public override MindboxContext GetContext()
        {
            return DbContext;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
