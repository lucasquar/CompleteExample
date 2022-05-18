using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteExample.Entities.Repositories
{
    public class CompleteExampleRepository : ICompleteExampleRepository
    {
        private readonly CompleteExampleDBContext _context;

        public CompleteExampleRepository(CompleteExampleDBContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync() => await this._context.Students.ToListAsync();

        public async Task<IEnumerable<Course>> GetAllCoursesByInstructorIdAsync(int instructorId) =>
            await this._context.Courses.Where(c => c.InstructorId == instructorId).ToListAsync();

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsByInstructorIdAsync(int instructorId)
        {
            return await this._context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Join(
                    this._context.Enrollments,
                    c => c.CourseId,
                    e => e.CourseId,
                    (c, e) => e)
                .Include(e => e.Course)
                .Include(e => e.Student)
                .OrderBy(e => e.Course.Title)
                .ThenBy(e => e.Student.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetTopEnrollmentsForStudentsAndCourseAsync()
        {
            return await this._context.Enrollments
                .Where(e => e.Grade.HasValue)
                .Include(e => e.Course)
                .Include(e => e.Student)
                .OrderBy(e => e.Course.Title)
                .ThenByDescending(e => e.Grade)
                .ToListAsync();
        }
    }
}
