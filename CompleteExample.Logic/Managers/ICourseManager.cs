using CompleteExample.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Managers
{
    public interface ICourseManager
    {
        Task<IEnumerable<Course>> GetAllAsync();
    }
}