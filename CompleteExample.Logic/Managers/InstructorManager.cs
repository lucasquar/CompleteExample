using CompleteExample.Entities.Repositories;
using CompleteExample.Logic.DTOs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Managers
{
    public class InstructorManager : IInstructorManager
    {
        private readonly ILogger<InstructorManager> _logger;
        private readonly ICompleteExampleRepository _completeExampleRepository;

        public InstructorManager(ILogger<InstructorManager> logger, ICompleteExampleRepository completeExampleRepository)
        {
            this._logger = logger;
            this._completeExampleRepository = completeExampleRepository;
        }

        public async Task<IEnumerable<CourseStudentGradeDTO>> GetStudentGradesAsync(int instructorId)
        {
            this._logger.LogInformation("Getting the list of students' grades the instructor has given out");
            var enrollments = await this._completeExampleRepository.GetAllEnrollmentsByInstructorIdAsync(instructorId);
            return enrollments.GroupBy(e => new { e.CourseId, e.Course.Title }).Select(e => new CourseStudentGradeDTO()
            {
                CourseId = e.Key.CourseId,
                CourseTitle = e.Key.Title,
                Students = e.Select(s => new StudentGradeDTO()
                {
                    StudentId = s.Student.StudentId,
                    StudentName = $"{s.Student.LastName}, {s.Student.FirstName}",
                    StudentGrade = s.Grade
                })
            });
        }
    }
}
