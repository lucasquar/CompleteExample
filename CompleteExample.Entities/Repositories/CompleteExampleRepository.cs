using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IEnumerable<Student>> GetAllStudentsAsync() => 
            await this._context.Students.ToListAsync();

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

        public async Task<IEnumerable<Enrollment>> GetTopGradesEnrollmentsForStudentsAndCoursesAsync()
        {
            return await this._context.Enrollments
                .Where(e => e.Grade.HasValue)
                .Include(e => e.Course)
                .Include(e => e.Student)
                .OrderBy(e => e.Course.Title)
                .ThenByDescending(e => e.Grade)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync() =>
            await this._context.Courses.ToListAsync();

        public async Task<Student> GetStudentByIdAsync(int studenId) =>
            await this._context.Students.FirstOrDefaultAsync(s => s.StudentId == studenId);

        public async Task<Course> GetCourseByIdAsync(int courseId) =>
            await this._context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

        public async Task<int?> CreateEnrollmentAsync(Enrollment enrollment)
        {
            await this._context.Enrollments.AddAsync(enrollment);
            var success = await this.SaveChangesAsync();
            if (!success)
                return null;

            return enrollment.EnrollmentId;
        }

        public async Task<Enrollment> GetEnrollmentByStudentIdCourseIdAsync(int studentId, int courseId) =>
            await this._context.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        public async Task<bool> UpdateEnrollmentGradeWithHistoricalAsync(int studentId, int courseId, decimal newGrade)
        {
            var enrollment = await this.GetEnrollmentByStudentIdCourseIdAsync(studentId, courseId);
            if (enrollment == null)
                return false;

            enrollment.Grade = newGrade;

            var historicalStudentGrade = new HistoricalStudentGrade()
            {
                StudentId = studentId,
                CourseId = courseId,
                Grade = newGrade,
                GradeDate = DateTime.Now
            };

            await this._context.HistoricalStudentGrades.AddAsync(historicalStudentGrade);

            return await this.SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync()
        {
            var result = await this._context.SaveChangesAsync();
            return result > 0;
        }
    }
}
