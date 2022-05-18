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
            this._logger.LogInformation("Getting top 3 students' grades for each course");
            var enrollments = await this._completeExampleRepository.GetTopGradesEnrollmentsForStudentsAndCoursesAsync();
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

        public async Task<int?> EnrollStudentInACourseAsync(EnrollmentStudentCourseDTO enrollmentRequest)
        {
            this._logger.LogInformation("Enrolling a student in a course");
            var isValidRequest = await this.ValidateEnrollmentAsync(enrollmentRequest);
            if (!isValidRequest)
                return null;

            var enrollment = new Enrollment() { StudentId = enrollmentRequest.StudentId.Value, CourseId = enrollmentRequest.CourseId.Value };
            return await this._completeExampleRepository.CreateEnrollmentAsync(enrollment);
        }

        public async Task<bool> UpdateStudentCourseGradeAsync(UpdateEnrollmentStudentCourseDTO enrollmentRequest)
        {
            this._logger.LogInformation("Updating a student' grade for a course");
            var isValidRequest = await this.ValidateEnrollmentAsync(enrollmentRequest, false);
            if (!isValidRequest)
                return false;

            return await this._completeExampleRepository.UpdateEnrollmentGradeWithHistoricalAsync(enrollmentRequest.StudentId.Value, enrollmentRequest.CourseId.Value, enrollmentRequest.Grade.Value);
        }

        private async Task<bool> ValidateEnrollmentAsync(EnrollmentStudentCourseDTO enrollmentToValidate, bool validateNew = true)
        {
            if (enrollmentToValidate == null)
                return false;

            var student = await this._completeExampleRepository.GetStudentByIdAsync(enrollmentToValidate.StudentId.GetValueOrDefault());
            var course = await this._completeExampleRepository.GetCourseByIdAsync(enrollmentToValidate.CourseId.GetValueOrDefault());
            if (student == null || course == null)
                return false;

            var existingEnrollment = await this._completeExampleRepository.GetEnrollmentByStudentIdCourseIdAsync(enrollmentToValidate.StudentId.GetValueOrDefault(), enrollmentToValidate.CourseId.GetValueOrDefault());
            if (validateNew && existingEnrollment != null)
                return false;

            if (!validateNew && existingEnrollment == null)
                return false;

            return true;
        }
    }
}
