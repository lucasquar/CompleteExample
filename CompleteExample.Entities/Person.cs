using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteExample.Entities
{
    public class Person
    {
        [MaxLength(150)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(150)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; }
        [NotMapped]
        public string FullName => $"{this.LastName}, {this.FirstName}";
    }
}
