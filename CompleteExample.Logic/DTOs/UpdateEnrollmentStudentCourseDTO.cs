using System;
using System.ComponentModel.DataAnnotations;

namespace CompleteExample.Logic.DTOs
{
    public class UpdateEnrollmentStudentCourseDTO : EnrollmentStudentCourseDTO
    {
        [Required]
        [Range(0, 100)]
        [RegularExpression(@"^\d*(\.\d{0,2})?$", ErrorMessage = "The Grade must have at most two decimal points")]
        public decimal? Grade { get; set; }
    }
}
