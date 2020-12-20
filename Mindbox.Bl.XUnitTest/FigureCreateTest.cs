using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mindbox.Bl.DataModels;
using Xunit;

namespace Mindbox.Bl.XUnitTest
{
    public class FigureCreateTest
    {

        //[Fact]
        //public void ConstructorTest()
        //{
        //    FigureCreate figureCreate = new FigureCreate( );

        //    // массив делегатов функций
        //    Func<double[], IEnumerable<ValidationResult>>[] exp_validatorsFigures = new Func<double[], IEnumerable<ValidationResult>>[] { FigureCircle.Validate, FigureTriangle.Validate };

        //    var fieldInfo = typeof(FigureCreate).GetField(@"_validatorsFigures", BindingFlags.Static | BindingFlags.NonPublic);
        //    var value = fieldInfo.GetValue(figureCreate);
        //    var array = (Func<double[], IEnumerable<ValidationResult>>[])value;
        //    Assert.Equal(exp_validatorsFigures, array);
        //}

        [Fact]
        public void FigureTypeTest()
        {
            FigureCreate figureCreate = new FigureCreate {FigureType = 1};

            Assert.Equal(1, figureCreate.FigureType);
        }

        [Fact]
        public void FigureInputDataTest()
        {
            FigureCreate figureCreate = new FigureCreate { FigureInputData = new []{1d, 2d}};

            Assert.Equal(new[] { 1d, 2d }, figureCreate.FigureInputData);
        }

        [Fact]
        public void ValidateTest()
        {
            // На вход круг
            FigureCreate figureCreate = new FigureCreate { FigureType = 1 ,FigureInputData = new[] { 1d } };

            ValidationContext validationContext = new ValidationContext(figureCreate);
            var results = figureCreate.Validate(validationContext);
            Assert.Empty(results);

            // На вход круг с не корректными входными данными
            figureCreate = new FigureCreate { FigureType = 1, FigureInputData = new[] { -1d } };

            validationContext = new ValidationContext(figureCreate);
            results = figureCreate.Validate(validationContext);
            Assert.Single(results);

            // На вход треугольник
            figureCreate = new FigureCreate { FigureType = 2, FigureInputData = new[] { 1d, 2d, 3d} };

            validationContext = new ValidationContext(figureCreate);
            results = figureCreate.Validate(validationContext);
            Assert.Empty(results);

            // На вход треугольник с не корректными входными данными
            figureCreate = new FigureCreate { FigureType = 2, FigureInputData = new[] { 1d, 2d, -3d } };

            validationContext = new ValidationContext(figureCreate);
            results = figureCreate.Validate(validationContext);
            Assert.Single(results);

            // На вход не ведомая фигура
            figureCreate = new FigureCreate { FigureType = Int32.MinValue, FigureInputData = new[] { 1d, 2d, 3d, 4d } };

            validationContext = new ValidationContext(figureCreate);
            results = figureCreate.Validate(validationContext);
            Assert.Single(results);
            var validationResult = results.First();
            Assert.Contains($"Входная фигура не определена, не известное значение, текущее значение {Int32.MinValue}.", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"FigureInputData", validationResult.MemberNames.First());
            
            // Проверяем задействование валидаторов
            bool[] useValidators = new[] {false, false};

            Func<double[], IEnumerable<ValidationResult>> validatorCircle = doubles =>
            {
                useValidators[0] = true;
                return Array.Empty<ValidationResult>();
            };

            Func<double[], IEnumerable<ValidationResult>> validatorTriangle = doubles =>
            {
                useValidators[1] = true;
                return Array.Empty<ValidationResult>();
            };

            // Новый массив делегатов с валидаторами
            Func<double[], IEnumerable<ValidationResult>>[] validatorsFigures =
                new Func<double[], IEnumerable<ValidationResult>>[] { validatorCircle, validatorTriangle };

            // Круг
            figureCreate = new FigureCreate { FigureType = 1, FigureInputData = new[] { 1d } };
            var fieldInfo = typeof(FigureCreate).GetField(@"_validatorsFigures", BindingFlags.Static | BindingFlags.NonPublic);
            fieldInfo.SetValue(figureCreate, validatorsFigures);

            validationContext = new ValidationContext(figureCreate);
            results = figureCreate.Validate(validationContext);
            Assert.Empty(results);
            Assert.True(useValidators[0]);
            
            // Сбрасываем массив
            for (int i = 0; i < useValidators.Length; i++)
                useValidators[i] = false;

            // Треугольник
            figureCreate = new FigureCreate { FigureType = 2, FigureInputData = new[] { 1d, 2d, 3d } };
            fieldInfo = typeof(FigureCreate).GetField(@"_validatorsFigures", BindingFlags.Static | BindingFlags.NonPublic);
            fieldInfo.SetValue(figureCreate, validatorsFigures);

            validationContext = new ValidationContext(figureCreate);
            results = figureCreate.Validate(validationContext);
            Assert.Empty(results);
            Assert.True(useValidators[1]);

        }

        [Fact]
        public void CreateTest()
        {
            // Создаем круг
            FigureCreate figureCreate = new FigureCreate { FigureType = 1, FigureInputData = new[] { 1d } };

            IFigureInput figureInput = figureCreate.Create();

            FigureCircle figureCircle = figureInput as FigureCircle;
            Assert.NotNull(figureCircle);

            // Получаем радиус круга
            var fieldInfo = typeof(FigureCircle).GetField(@"_radius", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = fieldInfo.GetValue(figureCircle);
            double radius = (double)value;
            Assert.Equal(1d, radius);

            // Создаем Треугольник
            figureCreate = new FigureCreate { FigureType = 2, FigureInputData = new[] { 1d, 2d, 3d } };

            figureInput = figureCreate.Create();

            FigureTriangle figureTriangle = figureInput as FigureTriangle;
            Assert.NotNull(figureTriangle);

            // Получаем a
            fieldInfo = typeof(FigureTriangle).GetField(@"_a", BindingFlags.Instance | BindingFlags.NonPublic);
            value = fieldInfo.GetValue(figureTriangle);
            double a = (double)value;
            Assert.Equal(1d, a);

            // Получаем b
            fieldInfo = typeof(FigureTriangle).GetField(@"_b", BindingFlags.Instance | BindingFlags.NonPublic);
            value = fieldInfo.GetValue(figureTriangle);
            double b = (double)value;
            Assert.Equal(2d, b);

            // Получаем c
            fieldInfo = typeof(FigureTriangle).GetField(@"_c", BindingFlags.Instance | BindingFlags.NonPublic);
            value = fieldInfo.GetValue(figureTriangle);
            double c = (double)value;
            Assert.Equal(3d, c);

            // Тестируем на исключение - нет такой фигуры
            try
            {
                // Создаем невиданною фигуру
                figureCreate = new FigureCreate { FigureType = Int32.MinValue, FigureInputData = new[] { 1d, 2d, 3d } };
                figureInput = figureCreate.Create();

                Assert.True(false, "Должно быть исключение!");
                // throw new Exception($"Не определен тип фигуры {figureCreate.FigureType}");
            }
            catch (Exception e)
            {
                Assert.Equal($"Не определен тип фигуры {Int32.MinValue}", e.Message);
            }

        }
    }
}
