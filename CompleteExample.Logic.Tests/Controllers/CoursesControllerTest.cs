using CompleteExample.API.Controllers;
using CompleteExample.Entities;
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
    public class CoursesControllerTest
    { 
        private ILogger<CoursesController> mLogger;
        private ICourseManager mManager;
        private CoursesController sut;

        [SetUp]
        public void Setup()
        {
            this.mLogger = Substitute.For<ILogger<CoursesController>>();
            this.mManager = Substitute.For<ICourseManager>();
            this.sut = new CoursesController(this.mLogger, this.mManager);
        }

        [TearDown]
        public void Destroy()
        {
            this.mLogger = null;
            this.mManager = null;
            this.sut = null;
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var expectedResult = new List<Course>() { Substitute.For<Course>() };
            this.mManager.GetAllAsync().Returns(expectedResult);

            // Act
            var result = await this.sut.GetAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var resultObject = result as OkObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.AssignableTo<IEnumerable<Course>>());
            Assert.That(resultObject.Value as IEnumerable<Course>, Is.EquivalentTo(expectedResult));
            await this.mManager.Received().GetAllAsync();
        }

        [Test]
        public async Task GetAll_NoContent()
        {
            // Arrange
            var expectedResult = Substitute.For<IEnumerable<Course>>();
            this.mManager.GetAllAsync().Returns(expectedResult);

            // Act
            var result = await this.sut.GetAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NoContentResult>());
            var resultObject = result as NoContentResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
            await this.mManager.Received().GetAllAsync();
        }

        [Test]
        public async Task GetAll_InternalServerError()
        {
            // Arrange
            var expectedException = new Exception("some_exception");
            this.mManager.When(x => x.GetAllAsync()).Do(x => throw expectedException);

            // Act
            var result = await this.sut.GetAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var resultObject = result as ObjectResult;
            Assert.That(resultObject.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.IsNotNull(resultObject.Value);
            Assert.That(resultObject.Value, Is.EqualTo("some_exception"));
            await this.mManager.Received().GetAllAsync();
        }
    }
}
