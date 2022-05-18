using CompleteExample.Entities;
using CompleteExample.Logic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Managers
{
    public interface IStudentManager
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<IEnumerable<CourseStudentGradeDTO>> GetTopStudentsForEachCourseAsync();
        Task<int?> EnrollStudentInACourseAsync(EnrollmentStudentCourseDTO enrollmentRequest);
        Task<bool> UpdateStudentCourseGradeAsync(UpdateEnrollmentStudentCourseDTO request);
    }
}