using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mindbox.Bl.DataModels;
using Newtonsoft.Json;
using Xunit;

namespace Mindbox.Bl.XUnitTest
{
    public class FigureCircleTest
    {
        [Fact]
        public void ConstructorTest()
        {
            FigureCircle figureCircle = new FigureCircle(1);

            // Получаем радиус
            var fieldInfo = typeof(FigureCircle).GetField(@"_radius", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = fieldInfo.GetValue(figureCircle);
            double radius = (double)value;
            Assert.Equal(1d, radius);

            // Получаем максимальный радиус
            fieldInfo = typeof(FigureCircle).GetField(@"_maxValue", BindingFlags.Static | BindingFlags.NonPublic);
            value = fieldInfo.GetValue(figureCircle);
            double maxValue = (double)value;
            double exp_maxValue = Math.Sqrt(double.MaxValue) / Math.PI;
            Assert.Equal(exp_maxValue, maxValue);
        }

        [Fact]
        public void GetAreaTest()
        {
            double expectedarea = Math.PI;
            FigureCircle figureCircle = new FigureCircle(1);
            double area = figureCircle.GetArea();
            
            Assert.Equal(expectedarea, area);

            expectedarea = Math.PI*2d*2d;
            figureCircle = new FigureCircle(2d);
            area = figureCircle.GetArea();

            Assert.Equal(expectedarea, area);
        }

        [Fact]
        public void ValidateTest()
        {

            // Нормальный тест проходной
            double[] figureInputData = {1d} ;
            var results = FigureCircle.Validate(figureInputData);
            Assert.Empty(results);

            // Мало аргументов
            figureInputData = new double[0];
            results = FigureCircle.Validate(figureInputData);
            Assert.Single(results);
            ValidationResult validationResult = results.First();
            Assert.Contains(@"Входная фигура круг, параметр входной только один - радиус, а не", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Много аргументов
            figureInputData = new []{1d, 2d};
            results = FigureCircle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура круг, параметр входной только один - радиус, а не", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не  число на вход
            figureInputData = new[] { Double.NaN };
            results = FigureCircle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура круг, радиус не может быть таким, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не -бесконечность на вход
            figureInputData = new[] { Double.NegativeInfinity };
            results = FigureCircle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура круг, радиус не может быть таким, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не +бесконечность на вход
            figureInputData = new[] { Double.PositiveInfinity };
            results = FigureCircle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура круг, радиус не может быть таким, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Отрицательное число на вход
            figureInputData = new[] { -1d};
            results = FigureCircle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура круг, радиус не может отрицательным, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Больше максимально допустимого числа на вход
            double gr_maxValue = Math.Sqrt(double.MaxValue) / Math.PI * 2;
            figureInputData = new[] { gr_maxValue };
            results = FigureCircle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура круг, радиус не может быть более -", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            
        }

    }
}
