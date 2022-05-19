using CompleteExample.Entities;
using CompleteExample.Entities.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace CompleteExample.Logic.Tests.Repositories
{
    [TestFixture]
    public class CompleteExampleRepositoryTest
    {
        private CompleteExampleDBContext mContext;
        private CompleteExampleRepository sut;

        [SetUp]
        public void Setup()
        {
            this.mContext = Substitute.For<CompleteExampleDBContext>();
            this.sut = new CompleteExampleRepository(this.mContext);
        }

        [TearDown]
        public void Destroy()
        {
            this.mContext = null;
            this.sut = null;
        }
    }
}
