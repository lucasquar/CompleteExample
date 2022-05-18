using CompleteExample.Logic.DTOs;
using CompleteExample.Logic.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompleteExample.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly ILogger<InstructorsController> _logger;
        private readonly IInstructorManager _instructorManager;

        public InstructorsController(ILogger<InstructorsController> logger, IInstructorManager instructorManager)
        {
            this._logger = logger;
            this._instructorManager = instructorManager;
        }

        [HttpGet]
        [Route("[action]/{id}", Name = "GetInstructorStudentGrades")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CourseStudentGradeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInstructorStudentGradesAsync([FromRoute] int id)
        {
            try
            {
                var result = await this._instructorManager.GetStudentGradesAsync(id);
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
