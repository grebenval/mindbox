using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mindbox.Bl;
using Mindbox.Database.Sqlite.Data;

namespace Mindbox.Database.Sqlite.XUnitTest
{
    public class MindboxContextMemory : MindboxContext
    {
        public MindboxContextMemory(string connectionString) : base(connectionString)
        {
        }

        public MindboxContextMemory(DbContextOptions<MindboxContext> options, IDatabaseConnect databaseConnect) : base(options, databaseConnect)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(_connectionString)
                    .ConfigureWarnings(x=> x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
        }
    }
}
