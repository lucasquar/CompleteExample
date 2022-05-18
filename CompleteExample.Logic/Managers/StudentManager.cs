using CompleteExample.Entities;
using CompleteExample.Entities.Repositories;
using CompleteExample.Logic.DTOs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Managers
{
    public class StudentManager : IStudentManager
    {
        private readonly ILogger<StudentManager> _logger;
        private readonly ICompleteExampleRepository _completeExampleRepository;

        public StudentManager(ILogger<StudentManager> logger, ICompleteExampleRepository completeExampleRepository)
        {
            this._logger = logger;
            this._completeExampleRepository = completeExampleRepository;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            this._logger.LogInformation("Getting all students");
            return await this._completeExampleRepository.GetAllStudentsAsync();
        }

        public async Task<IEnumerable<CourseStudentGradeDTO>> GetTopStudentsForEachCourseAsync()
        {
            var enrollments = await this._completeExampleRepository.GetTopEnrollmentsForStudentsAndCourseAsync();
            return enrollments.GroupBy(e => e.Course).Select(e => new CourseStudentGradeDTO()
            {
                CourseId = e.Key.CourseId,
                CourseTitle = e.Key.Title,
                Students = e.Take(3).Select(s => new StudentGradeDTO()
                {
                    StudentId = s.StudentId,
                    StudentName = $"{s.Student.LastName}, {s.Student.FirstName}",
                    StudentGrade = s.Grade
                })
            });
        }
    }
}
