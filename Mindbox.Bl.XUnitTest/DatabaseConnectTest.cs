using System;
using System.Reflection;
using Xunit;

namespace Mindbox.Bl.XUnitTest
{
    public class DatabaseConnectTest
    {
        [Fact]
        public void GetConnectionStringTest()
        {
            IDatabaseConnect databaseConnect = new DatabaseConnect(@"TestConnect");
            
            Assert.Equal(@"TestConnect", databaseConnect.GetConnectionString());
        }

        [Fact]
        public void ConstructorTest()
        {
            IDatabaseConnect databaseConnect = new DatabaseConnect(@"TestConnect");

            var fieldInfo = typeof(DatabaseConnect).GetField(@"_connectionString", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = fieldInfo.GetValue(databaseConnect);
            string connectionString = (string)value;
            Assert.Equal(@"TestConnect", connectionString);
        }
    }
}
