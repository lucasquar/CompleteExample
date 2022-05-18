using CompleteExample.Entities;
using CompleteExample.Logic.DTOs;
using CompleteExample.Logic.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CompleteExample.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IStudentManager _studentManager;

        public StudentsController(ILogger<StudentsController> logger, IStudentManager studentManager)
        {
            this._logger = logger;
            this._studentManager = studentManager;
        }

        /// <summary>
        /// Retrieves the collection of students
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var result = await this._studentManager.GetAllAsync();
                if (result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"{ex.Message} for {ex.Data}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("[action]", Name = "GetTopStudentsForEachCourse")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CourseStudentGradeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopStudentsForEachCourseAsync()
        {
            try
            {
                var result = await this._studentManager.GetTopStudentsForEachCourseAsync();
                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"{ex.Message} for {ex.Data}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
