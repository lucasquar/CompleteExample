using CompleteExample.Logic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Managers
{
    public interface IInstructorManager
    {
        Task<IEnumerable<CourseStudentGradeDTO>> GetStudentGradesAsync(int instructorId);
    }
}
