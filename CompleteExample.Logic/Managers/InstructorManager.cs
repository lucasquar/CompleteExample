using CompleteExample.Entities.Repositories;
using CompleteExample.Logic.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Managers
{
    public class InstructorManager : IInstructorManager
    {
        private readonly ICompleteExampleRepository _completeExampleRepository;

        public InstructorManager(ICompleteExampleRepository completeExampleRepository)
        {
            this._completeExampleRepository = completeExampleRepository;
        }

        public async Task<IEnumerable<CourseStudentGradeDTO>> GetStudentGradesAsync(int instructorId)
        {
            var enrollments = await this._completeExampleRepository.GetAllEnrollmentsByInstructorIdAsync(instructorId);
            return enrollments.GroupBy(e => e.Course).Select(e => new CourseStudentGradeDTO()
            {
                CourseId = e.Key.CourseId,
                CourseTitle = e.Key.Title,
                Students = e.Select(s => new StudentGradeDTO()
                {
                    StudentId = s.StudentId,
                    StudentName = $"{s.Student.LastName}, {s.Student.FirstName}",
                    StudentGrade = s.Grade
                })
            });
        }
    }
}
