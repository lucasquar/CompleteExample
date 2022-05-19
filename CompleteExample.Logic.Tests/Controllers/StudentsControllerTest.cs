using CompleteExample.API.Controllers;
using CompleteExample.Logic.DTOs;
using CompleteExample.Logic.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Tests.Controllers
{
    [TestFixture]
    public class StudentsControllerTest
    {
        private ILogger<StudentsController> mLogger;
        private IStudentManager mManager;
        private StudentsController sut;

        [SetUp]
        public void Setup()
        {
            this.mLogger = Substitute.For<ILogger<StudentsController>>();
            this.mManager = Substitute.For<IStudentManager>();
            this.sut = new StudentsController(this.mLogger, this.mManager);
        }

        [TearDown]
        public void Destroy()
        {
            this.mLogger = null;
            this.mManager = null;
            this.sut = null;
        }

        #region GetTopStudentsForEachCourse
        [Test]
        [Category("GetTopStudentsForEachCourse")]
        public async Task GetTopStudentsForEachCourse_Success()
        {
            // Arrange
            var expectedResult = new List<CourseStudentGradeDTO>() { Substitute.For<CourseStudentGradeDTO>() };
            this.mManager.GetTopStudentsForEachCourseAsync().Returns(expectedResult);

            // Act
            var result = await this.sut.GetTopStudentsForEachCourseAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var resultObject = result as OkObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.AssignableTo<IEnumerable<CourseStudentGradeDTO>>());
            Assert.That(resultObject.Value as IEnumerable<CourseStudentGradeDTO>, Is.EquivalentTo(expectedResult));
            await this.mManager.Received().GetTopStudentsForEachCourseAsync();
        }

        [Test]
        [Category("GetTopStudentsForEachCourse")]
        public async Task GetTopStudentsForEachCourse_NoContent()
        {
            // Arrange
            var expectedResult = Substitute.For<IEnumerable<CourseStudentGradeDTO>>();
            this.mManager.GetTopStudentsForEachCourseAsync().Returns(expectedResult);

            // Act
            var result = await this.sut.GetTopStudentsForEachCourseAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NoContentResult>());
            var resultObject = result as NoContentResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
            await this.mManager.Received().GetTopStudentsForEachCourseAsync();
        }

        [Test]
        [Category("GetTopStudentsForEachCourse")]
        public async Task GetTopStudentsForEachCourse_InternalServerError()
        {
            // Arrange
            var expectedException = new Exception("some_exception");
            this.mManager.When(x => x.GetTopStudentsForEachCourseAsync()).Do(x => throw expectedException);

            // Act
            var result = await this.sut.GetTopStudentsForEachCourseAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultObject = result as ObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.EqualTo("some_exception"));
            await this.mManager.Received().GetTopStudentsForEachCourseAsync();
        }
        #endregion

        #region EnrollStudent
        [Test]
        [Category("EnrollStudent")]
        public async Task EnrollStudent_Success()
        {
            // Arrange
            var request = new EnrollmentStudentCourseDTO() { StudentId = 1, CourseId = 1 };
            var expectedResult = 1;
            this.mManager.EnrollStudentInACourseAsync(Arg.Any<EnrollmentStudentCourseDTO>()).Returns(expectedResult);

            // Act
            var result = await this.sut.EnrollStudentAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<CreatedResult>());
            var resultObject = result as CreatedResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.EqualTo(expectedResult));
            await this.mManager.Received().EnrollStudentInACourseAsync(request);
        }

        [Test]
        [Category("EnrollStudent")]
        public async Task EnrollStudent_BadRequest()
        {
            // Arrange
            var request = new EnrollmentStudentCourseDTO() { StudentId = 1, CourseId = 1 };
            int? expectedResult = null;
            this.mManager.EnrollStudentInACourseAsync(Arg.Any<EnrollmentStudentCourseDTO>()).Returns(expectedResult);

            // Act
            var result = await this.sut.EnrollStudentAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<BadRequestResult>());
            var resultObject = result as BadRequestResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            await this.mManager.Received().EnrollStudentInACourseAsync(request);
        }

        [Test]
        [Category("EnrollStudent")]
        public async Task EnrollStudent_InternalServerError()
        {
            // Arrange
            var request = new EnrollmentStudentCourseDTO() { StudentId = 1, CourseId = 1 };
            var expectedException = new Exception("some_exception");
            this.mManager.When(x => x.EnrollStudentInACourseAsync(Arg.Any<EnrollmentStudentCourseDTO>())).Do(x => throw expectedException);

            // Act
            var result = await this.sut.EnrollStudentAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultObject = result as ObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.EqualTo("some_exception"));
            await this.mManager.Received().EnrollStudentInACourseAsync(request);
        }
        #endregion

        #region UpdateStudentGrade
        [Test]
        [Category("UpdateStudentGrade")]
        public async Task UpdateStudentGrade_Success()
        {
            // Arrange
            var request = new UpdateEnrollmentStudentCourseDTO() { StudentId = 1, CourseId = 1, Grade = 80 };
            var expectedResult = true;
            this.mManager.UpdateStudentCourseGradeAsync(Arg.Any<UpdateEnrollmentStudentCourseDTO>()).Returns(expectedResult);

            // Act
            var result = await this.sut.UpdateStudentGradeAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<OkResult>());
            var resultObject = result as OkResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            await this.mManager.Received().UpdateStudentCourseGradeAsync(request);
        }

        [Test]
        [Category("UpdateStudentGrade")]
        public async Task UpdateStudentGrade_NotFound()
        {
            // Arrange
            var request = new UpdateEnrollmentStudentCourseDTO() { StudentId = 1, CourseId = 1, Grade = 80 };
            var expectedResult = false;
            this.mManager.UpdateStudentCourseGradeAsync(Arg.Any<UpdateEnrollmentStudentCourseDTO>()).Returns(expectedResult);

            // Act
            var result = await this.sut.UpdateStudentGradeAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            var resultObject = result as NotFoundResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            await this.mManager.Received().UpdateStudentCourseGradeAsync(request);
        }

        [Test]
        [Category("UpdateStudentGrade")]
        public async Task UpdateStudentGrade_InternalServerError()
        {
            // Arrange
            var request = new UpdateEnrollmentStudentCourseDTO() { StudentId = 1, CourseId = 1, Grade = 80 }; 
            var expectedException = new Exception("some_exception");
            this.mManager.When(x => x.UpdateStudentCourseGradeAsync(Arg.Any<UpdateEnrollmentStudentCourseDTO>())).Do(x => throw expectedException);

            // Act
            var result = await this.sut.UpdateStudentGradeAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultObject = result as ObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.EqualTo("some_exception"));
            await this.mManager.Received().UpdateStudentCourseGradeAsync(request);
        }
        #endregion
    }
}
