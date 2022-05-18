using System.ComponentModel.DataAnnotations;

namespace CompleteExample.Logic.DTOs
{
    public class EnrollmentStudentCourseDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? StudentId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int? CourseId { get; set; }
    }
}
