using System;
using System.ComponentModel.DataAnnotations;

namespace CompleteExample.Entities
{
    public class Instructor : Person
    {
        [Key]
        public int InstructorId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
