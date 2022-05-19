using CompleteExample.Entities;
using CompleteExample.Entities.Repositories;
using CompleteExample.Logic.DTOs;
using CompleteExample.Logic.Managers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Tests.Managers
{
    [TestFixture]
    public class InstructorManagerTest
    {
        private ILogger<InstructorManager> mLogger;
        private ICompleteExampleRepository mRepository;
        private InstructorManager sut;

        [SetUp]
        public void Setup()
        {
            this.mLogger = Substitute.For<ILogger<InstructorManager>>();
            this.mRepository = Substitute.For<ICompleteExampleRepository>();
            this.sut = new InstructorManager(this.mLogger, this.mRepository);
        }

        [TearDown]
        public void Destroy()
        {
            this.mLogger = null;
            this.mRepository = null;
            this.sut = null;
        }

        [Test]
        public async Task GetStudentGrades_Success()
        {
            // Arrange
            var instructorId = 1;
            var expectedResult = new List<CourseStudentGradeDTO>()
            {
                new CourseStudentGradeDTO()
                {
                    CourseId = 1,
                    CourseTitle = "some_title",
                    Students = new List<StudentGradeDTO>()
                    {
                        new StudentGradeDTO()
                        {
                            StudentId = 1,
                            StudentName = "some_last_name, some_first_name",
                            StudentGrade = 70
                        }
                    }
                }
            };
            var expectedEnrollments = new List<Enrollment>()
            {
                new Enrollment()
                {
                    CourseId = 1,
                    Course = new Course() { CourseId = 1, Title = "some_title" },
                    Student = new Student() { StudentId = 1, LastName = "some_last_name", FirstName = "some_first_name" },
                    Grade = 70
                }
            };
            this.mRepository.GetAllEnrollmentsByInstructorIdAsync(Arg.Any<int>()).Returns(expectedEnrollments);

            // Act
            var result = await this.sut.GetStudentGradesAsync(instructorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(expectedResult.Count()));
            Assert.That(result.First().CourseId, Is.EqualTo(1));
            Assert.That(result.First().CourseTitle, Is.EqualTo(expectedResult.First().CourseTitle));
            Assert.IsNotEmpty(result.First().Students);
            Assert.That(result.First().Students.Count(), Is.EqualTo(expectedResult.First().Students.Count()));
            Assert.That(result.First().Students.First().StudentName, Is.EqualTo(expectedResult.First().Students.First().StudentName));
            await this.mRepository.Received().GetAllEnrollmentsByInstructorIdAsync(instructorId);
        }
    }
}
