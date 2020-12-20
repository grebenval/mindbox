using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mindbox.Bl;
using Mindbox.Bl.Bl;
using Mindbox.Bl.DataModels;
using Mindbox.Database.Sqlite;
using MindboxApi.Controllers;
using Moq;
using Xunit;

namespace MindboxApi.XUnitTest
{
    public class FigureControllerTest
    {
        [Fact]
        public void ConstructorTest()
        {
            IDatabaseConnect databaseConnect = new DatabaseConnect(nameof(ConstructorTest));
            var figureManipulationMock = new Mock<IFigureManipulation>();
            FigureController figureController = new FigureController(figureManipulationMock.Object);

            var fieldInfo = typeof(FigureController).GetField(@"_iFigureManipulation", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = fieldInfo.GetValue(figureController);
            var figureManipulation = (IFigureManipulation)value;
            Assert.Equal(figureManipulationMock.Object, figureManipulation);
        }


        [Fact]
        public async Task InsertTest()
        {
            IDatabaseConnect databaseConnect = new DatabaseConnect(nameof(ConstructorTest));

            // Создаваемая фигура возвращает площадь 1d
            var figureInput = new Mock<IFigureInput>();
            figureInput.Setup(input => input.GetArea()).Returns(1d);

            var figureCreateMock = new Mock<FigureCreate>();
            figureCreateMock.Setup(create => create.Create()).Returns(figureInput.Object);
            

            var figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureInsert(It.IsAny<int>(),It.IsAny<double>(), It.IsAny<double[]>()))
                .Returns(Task.FromResult((true, 1L, string.Empty)));
            FigureController figureController = new FigureController(figureManipulationMock.Object);

            var result = await figureController.Insert(figureCreateMock.Object);

            var valueresult = result as JsonResult;
            Assert.NotNull(valueresult);
            Assert.Equal(1L, valueresult.Value);

            // Не удача при добавлении в БД
            figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureInsert(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double[]>()))
                .Returns(Task.FromResult((false, 0L, @"Не удача")));
            figureController = new FigureController(figureManipulationMock.Object);

            result = await figureController.Insert(figureCreateMock.Object);

            var valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status500InternalServerError, valueresult1.StatusCode.Value);
            Assert.Equal(@"Не удача", valueresult1.Value);

            // Не корректное значение площади на выходе
            
            // Отрицательное число
            figureInput = new Mock<IFigureInput>();
            figureInput.Setup(input => input.GetArea()).Returns(double.MinValue);

            figureCreateMock = new Mock<FigureCreate>();
            figureCreateMock.Setup(create => create.Create()).Returns(figureInput.Object);


            figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureInsert(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double[]>()))
                .Returns(Task.FromResult((true, 1L, string.Empty)));
            figureController = new FigureController(figureManipulationMock.Object);

            result = await figureController.Insert(figureCreateMock.Object);

            valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status400BadRequest, valueresult1.StatusCode.Value);
            Assert.Contains(@"Не корректные входные данные. Ошибка при расчете площади фигуры, площадь = ", (string)valueresult1.Value);

            // Не число
            figureInput = new Mock<IFigureInput>();
            figureInput.Setup(input => input.GetArea()).Returns(double.NaN);

            figureCreateMock = new Mock<FigureCreate>();
            figureCreateMock.Setup(create => create.Create()).Returns(figureInput.Object);


            figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureInsert(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double[]>()))
                .Returns(Task.FromResult((true, 1L, string.Empty)));
            figureController = new FigureController(figureManipulationMock.Object);

            result = await figureController.Insert(figureCreateMock.Object);

            valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status400BadRequest, valueresult1.StatusCode.Value);
            Assert.Contains(@"Не корректные входные данные. Ошибка при расчете площади фигуры, площадь = ", (string)valueresult1.Value);

            // -бесконечность
            figureInput = new Mock<IFigureInput>();
            figureInput.Setup(input => input.GetArea()).Returns(double.NegativeInfinity);

            figureCreateMock = new Mock<FigureCreate>();
            figureCreateMock.Setup(create => create.Create()).Returns(figureInput.Object);

            figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureInsert(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double[]>()))
                .Returns(Task.FromResult((true, 1L, string.Empty)));
            figureController = new FigureController(figureManipulationMock.Object);

            result = await figureController.Insert(figureCreateMock.Object);

            valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status400BadRequest, valueresult1.StatusCode.Value);
            Assert.Contains(@"Не корректные входные данные. Ошибка при расчете площади фигуры, площадь = ", (string)valueresult1.Value);

            // +бесконечность
            figureInput = new Mock<IFigureInput>();
            figureInput.Setup(input => input.GetArea()).Returns(double.PositiveInfinity);

            figureCreateMock = new Mock<FigureCreate>();
            figureCreateMock.Setup(create => create.Create()).Returns(figureInput.Object);

            figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureInsert(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double[]>()))
                .Returns(Task.FromResult((true, 1L, string.Empty)));
            figureController = new FigureController(figureManipulationMock.Object);

            result = await figureController.Insert(figureCreateMock.Object);

            valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status400BadRequest, valueresult1.StatusCode.Value);
            Assert.Contains(@"Не корректные входные данные. Ошибка при расчете площади фигуры, площадь = ", (string)valueresult1.Value);


            // Исключение при расчете площади
            figureInput = new Mock<IFigureInput>();
            figureInput.Setup(input => input.GetArea()).Throws(new Exception(@"Fail!"));

            figureCreateMock = new Mock<FigureCreate>();
            figureCreateMock.Setup(create => create.Create()).Returns(figureInput.Object);

            figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureInsert(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double[]>()))
                .Returns(Task.FromResult((true, 1L, string.Empty)));
            figureController = new FigureController(figureManipulationMock.Object);

            result = await figureController.Insert(figureCreateMock.Object);

            valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status400BadRequest, valueresult1.StatusCode.Value);
            Assert.Contains(@"Не корректные входные данные. Ошибка при расчете площади фигуры: Fail!", (string)valueresult1.Value);

        }

        [Fact]
        public async Task GetAreaTest()
        {
            // Не корректные входные данные

            var figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureGet(It.IsAny<long>()))
                .Returns(Task.FromResult((true, 1d, string.Empty)));
            FigureController figureController = new FigureController(figureManipulationMock.Object);

            var result = await figureController.GetArea(-1);

            var valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status400BadRequest, valueresult1.StatusCode.Value);
            Assert.Equal("Не корректные входные данные. id не может быть меньше или равен нулю.", valueresult1.Value);

            result = await figureController.GetArea(0);

            valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status400BadRequest, valueresult1.StatusCode.Value);
            Assert.Equal("Не корректные входные данные. id не может быть меньше или равен нулю.", valueresult1.Value);

            // Успешный проход
            result = await figureController.GetArea(1);

            var valueresult = result as JsonResult;
            Assert.NotNull(valueresult);
            Assert.Equal(1d, (double)valueresult.Value);


            // Не удача при получении из БД
            figureManipulationMock = new Mock<IFigureManipulation>();
            figureManipulationMock.Setup(manipulation => manipulation.FigureGet(It.IsAny<long>()))
                .Returns(Task.FromResult((false, 0D, @"Не удача")));
            figureController = new FigureController(figureManipulationMock.Object);

            result = await figureController.GetArea(1);

            valueresult1 = result as ObjectResult;
            Assert.NotNull(valueresult1);
            Assert.Equal(StatusCodes.Status500InternalServerError, valueresult1.StatusCode.Value);
            Assert.Equal(@"Не удача", valueresult1.Value);
        }
    }
}
