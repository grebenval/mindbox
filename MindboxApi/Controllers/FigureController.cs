using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Mindbox.Bl.Bl;
using Mindbox.Bl.DataModels;

namespace MindboxApi.Controllers
{
    /// <summary>
    /// Операции с фигурами
    /// </summary>
    [Route("")]
    [ApiController]
    public class FigureController : ControllerBase
    {
        /// <summary>
        /// Интерфейс взаимодействия с БД хранящих фигуры
        /// </summary>
        private readonly IFigureManipulation _iFigureManipulation;

        /// <summary>
        /// Контролер
        /// </summary>
        /// <param name="iFigureManipulation">Интерфейс взаимодействия с БД хранящих фигуры</param>
        public FigureController(IFigureManipulation iFigureManipulation)
        {
            _iFigureManipulation = iFigureManipulation;
            // Можно добавить лог, но этого в Т.З не было
        }


        /// <summary>
        /// Добавляет фигуру в БД
        /// </summary>
        /// <param name="figureCreate">Запрос на создание фигуры</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/figure/")]
        [ProducesResponseType(200, Type = typeof(long))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Insert([FromBody][Required] FigureCreate figureCreate)
        {
            var figure = figureCreate.Create();
            double area;
            try
            {
                area = figure.GetArea();
                // Не корректное значение площади на выходе
                if (double.IsNegative(area) || double.IsNaN(area) || double.IsInfinity(area))
                    return new ObjectResult($"Не корректные входные данные. Ошибка при расчете площади фигуры, площадь = {area}")
                        { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (Exception e)
            {
               return new ObjectResult($"Не корректные входные данные. Ошибка при расчете площади фигуры: {e.Message}" ) 
                    { StatusCode = StatusCodes.Status400BadRequest };
            }

            var result = await _iFigureManipulation.FigureInsert(figureCreate.FigureType, area, figureCreate.FigureInputData);
            // Если успех result.Item1, результат в Item2
            if (result.Item1)
                return (IActionResult) new JsonResult(result.Item2);
            
            // Не удача ошибка в result.Item3
            return new ObjectResult(result.Item3) { StatusCode = StatusCodes.Status500InternalServerError };

        }

        /// <summary>
        /// Возвращает площадь фигуры из БД по ID
        /// </summary>
        /// <param name="id">id фигуры</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/figure/{id:long}")]
        [ProducesResponseType(200, Type = typeof(double))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetArea(long id)
        {
            if (id <= 0 )
                return new ObjectResult("Не корректные входные данные. id не может быть меньше или равен нулю.")
                { StatusCode = StatusCodes.Status400BadRequest };

            var result = await _iFigureManipulation.FigureGet(id);
            // Если успех result.Item1, результат в Item2
            if (result.Item1)
                return (IActionResult)new JsonResult(result.Item2);

            // Не удача ошибка в result.Item3
            return new ObjectResult(result.Item3) { StatusCode = StatusCodes.Status500InternalServerError };

        }
    }
}
