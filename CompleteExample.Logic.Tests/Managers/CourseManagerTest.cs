using CompleteExample.Entities;
using CompleteExample.Entities.Repositories;
using CompleteExample.Logic.Managers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Tests.Managers
{
    [TestFixture]
    public class CourseManagerTest
    {
        private ILogger<CourseManager> mLogger;
        private ICompleteExampleRepository mRepository;
        private CourseManager sut;

        [SetUp]
        public void Setup()
        {
            this.mLogger = Substitute.For<ILogger<CourseManager>>();
            this.mRepository = Substitute.For<ICompleteExampleRepository>();
            this.sut = new CourseManager(this.mLogger, this.mRepository);
        }

        [TearDown]
        public void Destroy()
        {
            this.mLogger = null;
            this.mRepository = null;
            this.sut = null;
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var expectedResult = new List<Course>() { Substitute.For<Course>() };
            this.mRepository.GetAllCoursesAsync().Returns(expectedResult);

            // Act
            var result = await this.sut.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result, Is.EqualTo(expectedResult));
            await this.mRepository.Received().GetAllCoursesAsync();
        }
    }
}
