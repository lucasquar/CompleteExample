using CompleteExample.Entities;
using CompleteExample.Entities.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Managers
{
    public class CourseManager : ICourseManager
    {
        private readonly ILogger<CourseManager> _logger;
        private readonly ICompleteExampleRepository _completeExampleRepository;

        public CourseManager(ILogger<CourseManager> logger, ICompleteExampleRepository completeExampleRepository)
        {
            this._logger = logger;
            this._completeExampleRepository = completeExampleRepository;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            this._logger.LogInformation("Getting all courses");
            return await this._completeExampleRepository.GetAllCoursesAsync();
        }
    }
}
