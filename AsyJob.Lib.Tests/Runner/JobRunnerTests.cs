using AsyJob.Lib.Runner;
using AsyJob.Lib.Tests.Jobs;
using AsyJob.Lib.Tests.TestDoubles;
using NUnit.Framework.Internal;

namespace AsyJob.Lib.Tests.Runner
{
    internal class JobRunnerTests
    {

        [Test]
        public void RunJob_DummyJob_ShouldRunJob()
        {
            //Arrange
            var stubAuthManager = new StubAuthorizationManager();
            var fakeJobPool = new FakeJobPool();
            var sut = new JobRunner(fakeJobPool, stubAuthManager);
            var spyJob = new SpyJob("TEST_1");
            //Act
            sut.RunJob(spyJob);
            JobTestUtils.WaitForJobCompletion(spyJob);
            //Assert
            Assert.That(spyJob.Output?.RunCount, Is.EqualTo(1));
        }

        [Test]
        public void RunJob_JobPool_ShouldRunTheJobThroughTheJobPool()
        {
            //Arrange
            var stubAuthManager = new StubAuthorizationManager();
            var fakeJobPool = new FakeJobPool();
            var sut = new JobRunner(fakeJobPool, stubAuthManager);
            var dummyJob = new DummyJob("JOB_1");
            //Act
            sut.RunJob(dummyJob);
            //Assert
            Assert.That(fakeJobPool.JobThreads, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task GetJobs_WithJobs_ShouldReturnAllJobs()
        {
            //Arrange
            var stubAuthManager = new StubAuthorizationManager();
            var fakeJobPool = new FakeJobPool();
            fakeJobPool.RunJob(new DummyJob("J1"));
            fakeJobPool.RunJob(new DummyJob("J2"));
            var sut = new JobRunner(fakeJobPool, stubAuthManager);
            //Act
            var jobs = await sut.GetJobs();
            //Assert
            Assert.That(jobs.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetJob_ExistingId_ShouldReturnJob()
        {
            //Arrange
            var stubAuthManager = new StubAuthorizationManager();
            var fakeJobPool = FakeJobPool.InitializePool([new DummyJob("J1")]);
            var sut = new JobRunner(fakeJobPool, stubAuthManager);
            //Act
            var job = await sut.GetJob("J1");
            //Assert
            Assert.That(job, Is.Not.Null);
        }

        [Test]
        public async Task GetJob_NoExistingId_ShouldReturnNull()
        {
            //Arrange
            var stubAuthManager = new StubAuthorizationManager();
            var fakeJobPool = FakeJobPool.InitializePool([new DummyJob("J1")]);
            var sut = new JobRunner(fakeJobPool, stubAuthManager);
            //Act
            var job = await sut.GetJob("Wabbajack");
            //Assert
            Assert.That(job, Is.Null);
        }
    }
}
