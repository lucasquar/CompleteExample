using System.ComponentModel.DataAnnotations;

namespace CompleteExample.Entities
{
    public  class Student : Person
    {
        [Key]
        public int StudentId { get; set; }
        public string TimeZone { get; set; }
    }
}
