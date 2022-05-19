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
    public class InstructorsControllerTest
    {
        private ILogger<InstructorsController> mLogger;
        private IInstructorManager mManager;
        private InstructorsController sut;

        [SetUp]
        public void Setup()
        {
            this.mLogger = Substitute.For<ILogger<InstructorsController>>();
            this.mManager = Substitute.For<IInstructorManager>();
            this.sut = new InstructorsController(this.mLogger, this.mManager);
        }

        [TearDown]
        public void Destroy()
        {
            this.mLogger = null;
            this.mManager = null;
            this.sut = null;
        }

        [Test]
        public async Task GetInstructorStudentGrades_Success()
        {
            // Arrange
            var instructorId = 1;
            var expectedResult = new List<CourseStudentGradeDTO>() { Substitute.For<CourseStudentGradeDTO>() };
            this.mManager.GetStudentGradesAsync(Arg.Any<int>()).Returns(expectedResult);

            // Act
            var result = await this.sut.GetInstructorStudentGradesAsync(instructorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var resultObject = result as OkObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.AssignableTo<IEnumerable<CourseStudentGradeDTO>>());
            Assert.That(resultObject.Value as IEnumerable<CourseStudentGradeDTO>, Is.EquivalentTo(expectedResult));
            await this.mManager.Received().GetStudentGradesAsync(instructorId);
        }

        [Test]
        public async Task GetInstructorStudentGrades_NoContent()
        {
            // Arrange
            var instructorId = 1;
            var expectedResult = Substitute.For<IEnumerable<CourseStudentGradeDTO>>();
            this.mManager.GetStudentGradesAsync(Arg.Any<int>()).Returns(expectedResult);

            // Act
            var result = await this.sut.GetInstructorStudentGradesAsync(instructorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NoContentResult>());
            var resultObject = result as NoContentResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
            await this.mManager.Received().GetStudentGradesAsync(instructorId);
        }

        [Test]
        public async Task GetInstructorStudentGrades_InternalServerError()
        {
            // Arrange
            var instructorId = 1;
            var expectedException = new Exception("some_exception");
            this.mManager.When(x => x.GetStudentGradesAsync(Arg.Any<int>())).Do(x => throw expectedException);

            // Act
            var result = await this.sut.GetInstructorStudentGradesAsync(instructorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultObject = result as ObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.EqualTo("some_exception"));
            await this.mManager.Received().GetStudentGradesAsync(instructorId);
        }
    }
}
