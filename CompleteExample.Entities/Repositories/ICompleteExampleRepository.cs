using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Entities.Repositories
{
    public interface ICompleteExampleRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<IEnumerable<Course>> GetAllCoursesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Enrollment>> GetTopEnrollmentsForStudentsAndCourseAsync();
    }
}