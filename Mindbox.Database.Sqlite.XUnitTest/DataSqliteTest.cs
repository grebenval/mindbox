using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mindbox.Bl;
using Mindbox.Database.Sqlite.Data;
using Xunit;

namespace Mindbox.Database.Sqlite.XUnitTest
{
    public class DataSqliteTest
    {
        [Fact]
        public void TestConstructor()
        {
            IDatabaseConnect databaseConnect = new DatabaseConnect(nameof(TestConstructor));

            // В конструкторе инициализируется строка подключения, проверяем
            DataSqlite dataSqlite = new DataSqlite(databaseConnect);
            var fieldInfo = typeof(DataSqlite).GetField(@"ConnectionString", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = fieldInfo.GetValue(dataSqlite);
            string connectionString = (string)value;
            Assert.Equal(nameof(TestConstructor), connectionString);
        }

        [Fact]
        public async Task TestFigureInsert()
        {
            
            IDatabaseConnect databaseConnect = new DatabaseConnect(nameof(TestFigureInsert));
            await using (var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString()))
            {
                // Create the dabase schema
                await mindboxContext.Database.EnsureCreatedAsync();
            }

            // Вставка круга
            {
                DataSqliteInMemoryEntity dataSqliteInMemoryEntity = new DataSqliteInMemoryEntity(databaseConnect);
                DataSqlite dataSqlite = (DataSqlite)dataSqliteInMemoryEntity;

                // Вставляем круг
                var result1 = await dataSqlite.FigureInsert(1, 0.2d, new double[] { 1d });
                long id1 = result1.Item2;
                Assert.True(result1.Item1);
                Assert.True(result1.Item2 > 0);
                Assert.True(string.IsNullOrEmpty(result1.Item3));
                await using var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString());
                // Проверяем наличие фигуры в БД
                var figure = await mindboxContext.Figures.SingleAsync(f => f.Id == id1);
                Assert.NotNull(figure);
                Assert.Equal(1, figure.Type);
                Assert.Equal(0.2d, figure.Area);
                // Проверяем наличие круга в БД
                var circle = await mindboxContext.Circles.SingleAsync(c => c.Idcircle == id1);
                Assert.NotNull(circle);
                Assert.Equal(1d, circle.Radius);
            }

            // Вставка треугольника
            {
                DataSqliteInMemoryEntity dataSqliteInMemoryEntity = new DataSqliteInMemoryEntity(databaseConnect);
                DataSqlite dataSqlite = (DataSqlite)dataSqliteInMemoryEntity;

                // Вставляем треугольник
                var result1 = await dataSqlite.FigureInsert(2, 3.3d, new double[] { 1d, 3d, 3d });
                long id1 = result1.Item2;
                Assert.True(result1.Item1);
                Assert.True(result1.Item2 > 0);
                Assert.True(string.IsNullOrEmpty(result1.Item3));
                await using var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString());
                // Проверяем наличие фигуры в БД
                var figure = await mindboxContext.Figures.SingleAsync(f => f.Id == id1);
                Assert.NotNull(figure);
                Assert.Equal(2, figure.Type);
                Assert.Equal(3.3d, figure.Area);
                // Проверяем наличие треугольника в БД
                var triangle = await mindboxContext.Triangles.SingleAsync(t => t.Id == id1);
                Assert.NotNull(triangle);
                Assert.Equal(1d, triangle.A);
                Assert.Equal(3d, triangle.B);
                Assert.Equal(3d, triangle.C);
            }

            // Тест на NULL
            using (DataSqliteInMemoryMode dataSqliteInMemoryMode = new DataSqliteInMemoryMode(databaseConnect))
            {
                DataSqlite dataSqlite = (DataSqlite)dataSqliteInMemoryMode;

                // Вставляем с NULL
                var result2 = await dataSqlite.FigureInsert(2, 0.3d, null);
                long id2 = result2.Item2;
                Assert.False(result2.Item1);
                Assert.Equal(0L, id2);
                Assert.Contains(@"Фигура без входных параметров не может быть сохранена!", result2.Item3);

            }

            // Тест на empty
            using (DataSqliteInMemoryMode dataSqliteInMemoryMode = new DataSqliteInMemoryMode(databaseConnect))
            {
                DataSqlite dataSqlite = (DataSqlite)dataSqliteInMemoryMode;

                // Вставляем с NULL
                var result2 = await dataSqlite.FigureInsert(2, 0.3d, Array.Empty<double>());
                long id2 = result2.Item2;
                Assert.False(result2.Item1);
                Assert.Equal(0L, id2);
                Assert.Contains(@"Фигура без входных параметров не может быть сохранена!", result2.Item3);
            }

            // Ловим исключение
            DataSqlite dataSqliteNull = new DataSqliteNullContext(databaseConnect);
            try
            {
                var getresult1 = await dataSqliteNull.FigureInsert(1, 2d, new double[] { 1d });
                Assert.True(false, "Не должны сюда попасть - исключение.");
            }
            catch
            {
                Assert.True(true);
            }

        }
        
        [Fact]
        public async Task TestFigureGet()
        {

            IDatabaseConnect databaseConnect = new DatabaseConnect(nameof(TestFigureGet));
            await using (var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString()))
            {
                // Create the dabase schema
                await mindboxContext.Database.EnsureCreatedAsync();
            }

            long id1;
            long id2;

            await using (var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString()))
            {
                // Вставляем первое значение
                Figure figure1 = new Figure { Area = 0.2D };
                await mindboxContext.Figures.AddAsync(figure1);
                await mindboxContext.SaveChangesAsync();
                id1 = figure1.Id;

                // Вставляем второе значение
                Figure figure2 = new Figure { Area = 0.3D };
                await mindboxContext.Figures.AddAsync(figure2);
                await mindboxContext.SaveChangesAsync();
                id2 = figure2.Id;
            }

            DataSqlite dataSqlite = new DataSqliteInMemoryEntity(databaseConnect);

            // Получаем первое значение
            var getresult1 = await dataSqlite.FigureGet(id1);
            Assert.True(getresult1.Item1);
            Assert.Equal(0.2d, getresult1.Item2);
            Assert.True(string.IsNullOrEmpty(getresult1.Item3));

            // Получаем второе значение
            var getresult2 = await dataSqlite.FigureGet(id2);
            Assert.True(getresult2.Item1);
            Assert.Equal(0.3d, getresult2.Item2);
            Assert.True(string.IsNullOrEmpty(getresult2.Item3));

            // Ловим исключение
            DataSqlite dataSqliteNull = new DataSqliteNullContext(databaseConnect);
            try
            {
                getresult1 = await dataSqliteNull.FigureGet(2);
                Assert.True(false, "Не должны сюда попасть - исключение.");
            }
            catch 
            {
                Assert.True(true);
            }

        }

        [Fact]
        public void GetContextTest()
        {
            IDatabaseConnect databaseConnect = new DatabaseConnect(nameof(GetContextTest));

            // В конструкторе инициализируется строка подключения, проверяем
            DataSqlite dataSqlite = new DataSqlite(databaseConnect);
            using (var context = dataSqlite.GetContext())
            {
                Assert.NotNull(context);
                Assert.Equal(typeof(MindboxContext), context.GetType());
            }
        }

    }
}
