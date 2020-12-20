using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mindbox.Database.Sqlite.Data;

namespace Mindbox.Database.Sqlite.XUnitTest
{
    public class MindboxContextMemory : MindboxContext
    {
        public MindboxContextMemory(string connectionString) : base(connectionString)
        {
        }

        public MindboxContextMemory(DbContextOptions<MindboxContext> options) : base(options)
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
