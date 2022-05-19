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
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await this._studentManager.GetAllAsync();
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

        /// <summary>
        /// Retrieves the list of all students that have the top 3 grades for each course
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]", Name = "GetTopStudentsForEachCourse")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CourseStudentGradeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Enroll a student in a course
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]", Name = "EnrollStudent")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CreatedResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EnrollStudentAsync([FromBody] EnrollmentStudentCourseDTO request)
        {
            try
            {
                var enrollmentId = await this._studentManager.EnrollStudentInACourseAsync(request);
                if (!enrollmentId.HasValue)
                    return BadRequest();

                return Created(string.Empty, enrollmentId);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"{ex.Message} for {ex.Data}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update a grade(number) for a student for a course
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]", Name = "UpdateStudentGrade")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudentGradeAsync([FromBody] UpdateEnrollmentStudentCourseDTO request)
        {
            try
            {
                var success = await this._studentManager.UpdateStudentCourseGradeAsync(request);
                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"{ex.Message} for {ex.Data}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
