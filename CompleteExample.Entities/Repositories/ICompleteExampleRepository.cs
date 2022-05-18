using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Entities.Repositories
{
    public interface ICompleteExampleRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<IEnumerable<Course>> GetAllCoursesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Enrollment>> GetTopGradesEnrollmentsForStudentsAndCoursesAsync();
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Student> GetStudentByIdAsync(int studenId);
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<int?> CreateEnrollmentAsync(Enrollment enrollment);
        Task<Enrollment> GetEnrollmentByStudentIdCourseIdAsync(int studentId, int courseId);
        Task<bool> UpdateEnrollmentGradeWithHistoricalAsync(int studentId, int courseId, decimal newGrade);
    }
}