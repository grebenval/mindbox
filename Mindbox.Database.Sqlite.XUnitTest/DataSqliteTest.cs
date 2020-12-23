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
            MindboxContext mindboxContext = new MindboxContext(databaseConnect.GetConnectionString());
            
            // � ������������ ���������������� ������ �����������, ���������
            DataSqlite dataSqlite = new DataSqlite(mindboxContext);
            var fieldInfo = typeof(DataSqlite).GetField(@"MindboxContext", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = fieldInfo.GetValue(dataSqlite);
            MindboxContext mindboxContext1 = (MindboxContext)value;
            Assert.Equal(mindboxContext, mindboxContext1);
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

            // ������� �����
            {
                MindboxContextMemory mindboxContextMemory = new MindboxContextMemory(databaseConnect.GetConnectionString());
                DataSqlite dataSqlite = new DataSqlite(mindboxContextMemory);

                // ��������� ����
                var result1 = await dataSqlite.FigureInsert(1, 0.2d, new double[] { 1d });
                long id1 = result1.Item2;
                Assert.True(result1.Item1);
                Assert.True(result1.Item2 > 0);
                Assert.True(string.IsNullOrEmpty(result1.Item3));
                await using var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString());
                // ��������� ������� ������ � ��
                var figure = await mindboxContext.Figures.SingleAsync(f => f.Id == id1);
                Assert.NotNull(figure);
                Assert.Equal(1, figure.Type);
                Assert.Equal(0.2d, figure.Area);
                // ��������� ������� ����� � ��
                var circle = await mindboxContext.Circles.SingleAsync(c => c.Idcircle == id1);
                Assert.NotNull(circle);
                Assert.Equal(1d, circle.Radius);
            }

            // ������� ������������
            {
                MindboxContextMemory mindboxContextMemory = new MindboxContextMemory(databaseConnect.GetConnectionString());
                DataSqlite dataSqlite = new DataSqlite(mindboxContextMemory);

                // ��������� �����������
                var result1 = await dataSqlite.FigureInsert(2, 3.3d, new double[] { 1d, 3d, 3d });
                long id1 = result1.Item2;
                Assert.True(result1.Item1);
                Assert.True(result1.Item2 > 0);
                Assert.True(string.IsNullOrEmpty(result1.Item3));
                await using var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString());
                // ��������� ������� ������ � ��
                var figure = await mindboxContext.Figures.SingleAsync(f => f.Id == id1);
                Assert.NotNull(figure);
                Assert.Equal(2, figure.Type);
                Assert.Equal(3.3d, figure.Area);
                // ��������� ������� ������������ � ��
                var triangle = await mindboxContext.Triangles.SingleAsync(t => t.Id == id1);
                Assert.NotNull(triangle);
                Assert.Equal(1d, triangle.A);
                Assert.Equal(3d, triangle.B);
                Assert.Equal(3d, triangle.C);
            }

            // ���� �� NULL
            {
                MindboxContextMemory mindboxContextMemory = new MindboxContextMemory(databaseConnect.GetConnectionString());
                DataSqlite dataSqlite = new DataSqlite(mindboxContextMemory);

                // ��������� � NULL
                var result2 = await dataSqlite.FigureInsert(2, 0.3d, null);
                long id2 = result2.Item2;
                Assert.False(result2.Item1);
                Assert.Equal(0L, id2);
                Assert.Contains(@"������ ��� ������� ���������� �� ����� ���� ���������!", result2.Item3);

            }

            // ���� �� empty
            {
                MindboxContextMemory mindboxContextMemory = new MindboxContextMemory(databaseConnect.GetConnectionString());
                DataSqlite dataSqlite = new DataSqlite(mindboxContextMemory);

                // ��������� � NULL
                var result2 = await dataSqlite.FigureInsert(2, 0.3d, Array.Empty<double>());
                long id2 = result2.Item2;
                Assert.False(result2.Item1);
                Assert.Equal(0L, id2);
                Assert.Contains(@"������ ��� ������� ���������� �� ����� ���� ���������!", result2.Item3);
            }

            // ����� ����������
            DataSqlite dataSqliteNull = new DataSqlite(null);
            try
            {
                var getresult1 = await dataSqliteNull.FigureInsert(1, 2d, new double[] { 1d });
                Assert.True(false, "�� ������ ���� ������� - ����������.");
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
                // ��������� ������ ��������
                Figure figure1 = new Figure {Area = 0.2D};
                await mindboxContext.Figures.AddAsync(figure1);
                await mindboxContext.SaveChangesAsync();
                id1 = figure1.Id;

                // ��������� ������ ��������
                Figure figure2 = new Figure {Area = 0.3D};
                await mindboxContext.Figures.AddAsync(figure2);
                await mindboxContext.SaveChangesAsync();
                id2 = figure2.Id;
            }

            await using (var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString()))
            {
                DataSqlite dataSqlite = new DataSqlite(mindboxContext);

                // �������� ������ ��������
                var getresult1 = await dataSqlite.FigureGet(id1);
                Assert.True(getresult1.Item1);
                Assert.Equal(0.2d, getresult1.Item2);
                Assert.True(string.IsNullOrEmpty(getresult1.Item3));
            }
            await using (var mindboxContext = new MindboxContextMemory(databaseConnect.GetConnectionString()))
            {
                DataSqlite dataSqlite = new DataSqlite(mindboxContext);
                // �������� ������ ��������
                var getresult2 = await dataSqlite.FigureGet(id2);
                Assert.True(getresult2.Item1);
                Assert.Equal(0.3d, getresult2.Item2);
                Assert.True(string.IsNullOrEmpty(getresult2.Item3));
            }                             
            // ����� ����������
            DataSqlite dataSqliteNull = new DataSqlite(null);
            try
            {
                var getresult1 = await dataSqliteNull.FigureGet(2);
                Assert.True(false, "�� ������ ���� ������� - ����������.");
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

            MindboxContext mindboxContext = new MindboxContext(databaseConnect.GetConnectionString());
            
            // � ������������ ���������������� ��������, ���������
            DataSqlite dataSqlite = new DataSqlite(mindboxContext);
            using var context = dataSqlite.GetContext();
            Assert.NotNull(context);
            Assert.Equal(mindboxContext, context);
        }

    }
}
