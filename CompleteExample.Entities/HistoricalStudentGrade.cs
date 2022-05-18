using System;
using System.ComponentModel.DataAnnotations;

namespace CompleteExample.Entities
{
    public class HistoricalStudentGrade
    {
        [Key]
        public int HistoricalId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal? Grade { get; set; }
        public DateTime GradeDate { get; set; }
    }
}
