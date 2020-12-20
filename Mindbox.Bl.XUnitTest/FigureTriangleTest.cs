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
    public class FigureTriangleTest
    {
        [Fact]
        public void ConstructorTest()
        {
            FigureTriangle figureTriangle = new FigureTriangle(1d, 2d,3d);

            // Получаем a
            var fieldInfo = typeof(FigureTriangle).GetField(@"_a", BindingFlags.Instance | BindingFlags.NonPublic);
            var value = fieldInfo.GetValue(figureTriangle);
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

        }

        [Fact]
        public void GetAreaTest()
        {
            // Простой тест
            double a = 1d;
            double b = 3d;
            double c = 3d;
            double p = (a + b + c) * 0.5;
            double expectedarea = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            FigureTriangle figureTriangle = new FigureTriangle(a, b, c);
            double area = figureTriangle.GetArea();

            Assert.Equal(expectedarea, area);
            
            // Тест на ноль - не обязателен можно удалить
            a = 0d;
            b = 0d;
            c = 0d;
            p = (a + b + c) * 0.5;
            expectedarea = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            figureTriangle = new FigureTriangle(a, b, c);
            area = figureTriangle.GetArea();

            Assert.Equal(expectedarea, area);

            // Тест на бесконечность - проходит если отключено переполнение (а по умолчанию отключено)
            a = Double.MaxValue;
            b = Double.MaxValue;
            c = Double.MaxValue;
            p = (a + b + c) * 0.5;
            expectedarea = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            figureTriangle = new FigureTriangle(a, b, c);
            area = figureTriangle.GetArea();

            Assert.Equal(expectedarea, area);
        }

        [Fact]
        public void ValidateTest()
        {

            // Нормальный тест проходной
            double[] figureInputData = { 1d, 2.3d, 3d };
            var results = FigureTriangle.Validate(figureInputData);
            Assert.Empty(results);

            // Мало аргументов - 0
            figureInputData = new double[0];
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            ValidationResult validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, входных параметров должно быть 3, а не", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());


            // Мало аргументов - 1
            figureInputData = new[] { 1d }; 
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, входных параметров должно быть 3, а не", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Мало аргументов - 2
            figureInputData = new[] { 1d,2d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, входных параметров должно быть 3, а не", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Много аргументов
            figureInputData = new[] { 1d, 2d, 3d, 4d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, входных параметров должно быть 3, а не", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не  число на вход 1
            figureInputData = new[] { double.NaN, 2d, 3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина первой стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не -бесконечность на вход 1
            figureInputData = new[] { Double.NegativeInfinity, 2d, 3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина первой стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не +бесконечность на вход 1
            figureInputData = new[] { Double.PositiveInfinity, 2d, 3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина первой стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не  число на вход 2
            figureInputData = new[] { 1d, double.NaN, 3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина второй стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не -бесконечность на вход 2
            figureInputData = new[] { 1d, Double.NegativeInfinity,  3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина второй стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не +бесконечность на вход 2
            figureInputData = new[] { 1d, Double.PositiveInfinity,  3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина второй стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не  число на вход 3
            figureInputData = new[] { 1d, 2d, double.NaN};
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина третьей стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не -бесконечность на вход 3
            figureInputData = new[] { 1d, 2d, Double.NegativeInfinity };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина третьей стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не +бесконечность на вход 3
            figureInputData = new[] { 1d, 2d, Double.PositiveInfinity};
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина третьей стороны не может быть такой, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Отрицательное число на вход 1
            figureInputData = new[] { -1d, 2d, 3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина первой стороны не может быть отрицательной или = 0, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Отрицательное число на вход 2
            figureInputData = new[] { 1d, -2d, 3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина второй стороны не может быть отрицательной или = 0, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Отрицательное число на вход 3
            figureInputData = new[] { 1d, 2d, -3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, длина третьей стороны не может быть отрицательной или = 0, текущее значение", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Все три не корректные
            figureInputData = new[] { double.NaN, double.PositiveInfinity, -3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Equal(3, results.Count());

            var list = results.ToList();
            ValidationResult validationResult1 = list[0];
            Assert.Contains(@"Входная фигура треугольник, длина первой стороны не может быть такой, текущее значение", validationResult1.ErrorMessage);
            Assert.Single(validationResult1.MemberNames);
            Assert.Equal(@"figureInputData", validationResult1.MemberNames.First());

            
            ValidationResult validationResult2 = list[1];
            Assert.Contains(@"Входная фигура треугольник, длина второй стороны не может быть такой, текущее значение", validationResult2.ErrorMessage);
            Assert.Single(validationResult2.MemberNames);
            Assert.Equal(@"figureInputData", validationResult2.MemberNames.First());

            ValidationResult validationResult3 = list[2];
            Assert.Contains(@"Входная фигура треугольник, длина третьей стороны не может быть отрицательной или = 0, текущее значение", validationResult3.ErrorMessage);
            Assert.Single(validationResult3.MemberNames);
            Assert.Equal(@"figureInputData", validationResult3.MemberNames.First());

            // Не треугольник
            figureInputData = new[] { 1d, 1d, 3d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, сумма двух меньших сторон меньше максимальной стороны", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());

            // Не можем рассчитать площадь
            figureInputData = new[] { Double.MaxValue*0.9d, Double.MaxValue * 0.9d, Double.MaxValue * 0.9d };
            results = FigureTriangle.Validate(figureInputData);
            Assert.Single(results);
            validationResult = results.First();
            Assert.Contains(@"Входная фигура треугольник, сумма двух меньших сторон больше double.MaxValue =", validationResult.ErrorMessage);
            Assert.Single(validationResult.MemberNames);
            Assert.Equal(@"figureInputData", validationResult.MemberNames.First());
        }
    }
}
