using CompleteExample.Entities;
using CompleteExample.Entities.Repositories;
using CompleteExample.Logic.DTOs;
using CompleteExample.Logic.Managers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Tests.Managers
{
    [TestFixture]
    public class StudentManagerTest
    {
        private ILogger<StudentManager> mLogger;
        private ICompleteExampleRepository mRepository;
        private StudentManager sut;

        [SetUp]
        public void Setup()
        {
            this.mLogger = Substitute.For<ILogger<StudentManager>>();
            this.mRepository = Substitute.For<ICompleteExampleRepository>();
            this.sut = new StudentManager(this.mLogger, this.mRepository);
        }

        [TearDown]
        public void Destroy()
        {
            this.mLogger = null;
            this.mRepository = null;
            this.sut = null;
        }

        [Test]
        public async Task GetTopStudentsForEachCourse_Success()
        {
            // Arrange
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
                            StudentId = 3,
                            StudentName = "some_last_name, some_first_name",
                            StudentGrade = 90
                        },
                        new StudentGradeDTO()
                        {
                            StudentId = 2,
                            StudentName = "some_last_name, some_first_name",
                            StudentGrade = 80
                        },
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
                    Student = new Student() { StudentId = 3, LastName = "some_last_name", FirstName = "some_first_name" },
                    Grade = 90
                },
                new Enrollment()
                {
                    CourseId = 1,
                    Course = new Course() { CourseId = 1, Title = "some_title" },
                    Student = new Student() { StudentId = 2, LastName = "some_last_name", FirstName = "some_first_name" },
                    Grade = 80
                },
                new Enrollment()
                {
                    CourseId = 1,
                    Course = new Course() { CourseId = 1, Title = "some_title" },
                    Student = new Student() { StudentId = 1, LastName = "some_last_name", FirstName = "some_first_name" },
                    Grade = 70
                },
                new Enrollment()
                {
                    CourseId = 1,
                    Course = new Course() { CourseId = 1, Title = "some_title" },
                    Student = new Student() { StudentId = 4, LastName = "some_last_name", FirstName = "some_first_name" },
                    Grade = 50
                }
            };
            this.mRepository.GetTopGradesEnrollmentsForStudentsAndCoursesAsync().Returns(expectedEnrollments);

            // Act
            var result = await this.sut.GetTopStudentsForEachCourseAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(expectedResult.Count()));
            Assert.That(result.First().CourseId, Is.EqualTo(1));
            Assert.That(result.First().CourseTitle, Is.EqualTo(expectedResult.First().CourseTitle));
            Assert.IsNotEmpty(result.First().Students);
            Assert.That(result.First().Students.Count(), Is.EqualTo(3));
            Assert.That(result.First().Students.First().StudentGrade, Is.EqualTo(expectedResult.First().Students.First().StudentGrade));
            await this.mRepository.Received().GetTopGradesEnrollmentsForStudentsAndCoursesAsync();
        }

        #region EnrollStudentInACourse
        [Test]
        public async Task EnrollStudentInACourse_Success()
        {
            // Arrange
            var expectedResult = 1;
            var mockRequest = new EnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1 };
            this.mRepository.GetStudentByIdAsync(1).Returns(new Student());
            this.mRepository.GetCourseByIdAsync(1).Returns(new Course());
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(1, 1).Returns(null as Enrollment);
            this.mRepository.CreateEnrollmentAsync(Arg.Any<Enrollment>()).Returns(expectedResult);

            // Act
            var result = await this.sut.EnrollStudentInACourseAsync(mockRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expectedResult));
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.Received().GetCourseByIdAsync(1);
            await this.mRepository.Received().GetEnrollmentByStudentIdCourseIdAsync(1, 1);
            await this.mRepository.Received().CreateEnrollmentAsync(Arg.Any<Enrollment>());
        }

        [Test]
        public async Task EnrollStudentInACourse_FailNoExistingStudent()
        {
            // Arrange
            var expectedResult = 1;
            var mockRequest = new EnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1 };
            this.mRepository.GetStudentByIdAsync(1).Returns(null as Student);
            this.mRepository.GetCourseByIdAsync(Arg.Any<int>()).Returns(new Course());
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(null as Enrollment);
            this.mRepository.CreateEnrollmentAsync(Arg.Any<Enrollment>()).Returns(expectedResult);

            // Act
            var result = await this.sut.EnrollStudentInACourseAsync(mockRequest);

            // Assert
            Assert.IsNull(result);
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.DidNotReceive().GetCourseByIdAsync(Arg.Any<int>());
            await this.mRepository.DidNotReceive().GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>());
            await this.mRepository.DidNotReceive().CreateEnrollmentAsync(Arg.Any<Enrollment>());
        }

        [Test]
        public async Task EnrollStudentInACourse_FailNoExistingCourse()
        {
            // Arrange
            var expectedResult = 1;
            var mockRequest = new EnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1 };
            this.mRepository.GetStudentByIdAsync(1).Returns(new Student());
            this.mRepository.GetCourseByIdAsync(1).Returns(null as Course);
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(null as Enrollment);
            this.mRepository.CreateEnrollmentAsync(Arg.Any<Enrollment>()).Returns(expectedResult);

            // Act
            var result = await this.sut.EnrollStudentInACourseAsync(mockRequest);

            // Assert
            Assert.IsNull(result);
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.Received().GetCourseByIdAsync(1);
            await this.mRepository.DidNotReceive().GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>());
            await this.mRepository.DidNotReceive().CreateEnrollmentAsync(Arg.Any<Enrollment>());
        }

        [Test]
        public async Task EnrollStudentInACourse_FailExistingEnrollmentForStudentAndCourse()
        {
            // Arrange
            var expectedResult = 1;
            var mockRequest = new EnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1 };
            this.mRepository.GetStudentByIdAsync(1).Returns(new Student());
            this.mRepository.GetCourseByIdAsync(1).Returns(new Course());
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(1, 1).Returns(new Enrollment());
            this.mRepository.CreateEnrollmentAsync(Arg.Any<Enrollment>()).Returns(expectedResult);

            // Act
            var result = await this.sut.EnrollStudentInACourseAsync(mockRequest);

            // Assert
            Assert.IsNull(result);
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.Received().GetCourseByIdAsync(1);
            await this.mRepository.Received().GetEnrollmentByStudentIdCourseIdAsync(1, 1);
            await this.mRepository.DidNotReceive().CreateEnrollmentAsync(Arg.Any<Enrollment>());
        }
        #endregion

        #region UpdateStudentCourseGrade
        [Test]
        public async Task UpdateStudentCourseGrade_Success()
        {
            // Arrange
            var expectedResult = true;
            var mockRequest = new UpdateEnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1, Grade = 70 };
            this.mRepository.GetStudentByIdAsync(1).Returns(new Student());
            this.mRepository.GetCourseByIdAsync(1).Returns(new Course());
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(1, 1).Returns(new Enrollment());
            this.mRepository.UpdateEnrollmentGradeWithHistoricalAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<decimal>()).Returns(expectedResult);

            // Act
            var result = await this.sut.UpdateStudentCourseGradeAsync(mockRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expectedResult));
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.Received().GetCourseByIdAsync(1);
            await this.mRepository.Received().GetEnrollmentByStudentIdCourseIdAsync(1, 1);
            await this.mRepository.Received().UpdateEnrollmentGradeWithHistoricalAsync(1, 1, 70);
        }

        [Test]
        public async Task UpdateStudentCourseGrade_FailNoExistingStudent()
        {
            // Arrange
            var expectedResult = true;
            var mockRequest = new UpdateEnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1, Grade = 70 };
            this.mRepository.GetStudentByIdAsync(1).Returns(null as Student);
            this.mRepository.GetCourseByIdAsync(Arg.Any<int>()).Returns(new Course());
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(new Enrollment());
            this.mRepository.UpdateEnrollmentGradeWithHistoricalAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<decimal>()).Returns(expectedResult);

            // Act
            var result = await this.sut.UpdateStudentCourseGradeAsync(mockRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.DidNotReceive().GetCourseByIdAsync(Arg.Any<int>());
            await this.mRepository.DidNotReceive().GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>());
            await this.mRepository.DidNotReceive().UpdateEnrollmentGradeWithHistoricalAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<decimal>());
        }

        [Test]
        public async Task UpdateStudentCourseGrade_FailNoExistingCourse()
        {
            // Arrange
            var expectedResult = true;
            var mockRequest = new UpdateEnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1, Grade = 70 };
            this.mRepository.GetStudentByIdAsync(1).Returns(new Student());
            this.mRepository.GetCourseByIdAsync(1).Returns(null as Course);
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(new Enrollment());
            this.mRepository.UpdateEnrollmentGradeWithHistoricalAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<decimal>()).Returns(expectedResult);

            // Act
            var result = await this.sut.UpdateStudentCourseGradeAsync(mockRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.Received().GetCourseByIdAsync(1);
            await this.mRepository.DidNotReceive().GetEnrollmentByStudentIdCourseIdAsync(Arg.Any<int>(), Arg.Any<int>());
            await this.mRepository.DidNotReceive().UpdateEnrollmentGradeWithHistoricalAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<decimal>());
        }

        [Test]
        public async Task UpdateStudentCourseGrade_FailNoExistingEnrollmentForStudentAndCourse()
        {
            // Arrange
            var expectedResult = true;
            var mockRequest = new UpdateEnrollmentStudentCourseDTO() { CourseId = 1, StudentId = 1, Grade = 70 };
            this.mRepository.GetStudentByIdAsync(1).Returns(new Student());
            this.mRepository.GetCourseByIdAsync(1).Returns(new Course());
            this.mRepository.GetEnrollmentByStudentIdCourseIdAsync(1, 1).Returns(null as Enrollment);
            this.mRepository.UpdateEnrollmentGradeWithHistoricalAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<decimal>()).Returns(expectedResult);

            // Act
            var result = await this.sut.UpdateStudentCourseGradeAsync(mockRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
            await this.mRepository.Received().GetStudentByIdAsync(1);
            await this.mRepository.Received().GetCourseByIdAsync(1);
            await this.mRepository.Received().GetEnrollmentByStudentIdCourseIdAsync(1, 1);
            await this.mRepository.DidNotReceive().UpdateEnrollmentGradeWithHistoricalAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<decimal>());
        }
        #endregion
    }
}
