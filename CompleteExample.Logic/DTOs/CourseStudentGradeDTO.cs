using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteExample.Logic.DTOs
{
    public class CourseStudentGradeDTO
    {
        public CourseStudentGradeDTO()
        {
            this.Students = new List<StudentGradeDTO>();
        }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public IEnumerable<StudentGradeDTO> Students { get; set; }
    }
}
