using AsyJob;
using AsyJob.Jobs;
using AsyJobTests.Jobs.Test_Doubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs
{
    internal class JobPoolTests
    {

        [Test]
        public async Task StartJobPool_NoJobs_ShouldCreateEmptyJobPool()
        {
            //Arrange 
            var repo = new FakeJobRepository();
            //Act
            var sut = await JobPool.StartJobPool(repo);
            //Assert
            Assert.That(sut.RunningThreads, Is.EqualTo(0));
        }

        [Test]
        public async Task StartJobPool_HasExistingJobs_ShouldStartExistingJobs()
        {
            //Arrange
            var repo = new FakeJobRepository([
                new DummyJob("1"),
                new DummyJob("2")
            ]);
            //Act
            var sut = await JobPool.StartJobPool(repo);
            //Assert
            Assert.That(sut.RunningThreads, Is.EqualTo(2));
        }

        [Test]
        public async Task RunJob_Persistance_ShouldStoreJob()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = await JobPool.StartJobPool(repo);
            var job = new DummyJob("JOB_1");
            //Act
            sut.RunJob(job);
            //Assert
            Assert.That(repo.Jobs, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task RunJob_Job_ShouldRunJob()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = await JobPool.StartJobPool(repo);
            var spyJob = new SpyJob("SPY_1");
            //Act   
            sut.RunJob(spyJob);
            JobTestUtils.WaitForJobCompletion(spyJob);
            //Assert
            Assert.That(spyJob.Result.RunCount, Is.EqualTo(1));
        }


        [Test]
        public async Task RunJob_Async_ShouldRunJobInNewThread()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = await JobPool.StartJobPool(repo);
            var job = new SleepJob("SLEEP_1", new SleepInput(int.MaxValue));
            //Act
            sut.RunJob(job);
            //Assert
            Assert.That(job.Status, Is.Not.EqualTo(ProgressStatus.Done),
                "Job was completed, which means RunJob didn't return until job was finished.");
        }

        [Test]
        public async Task RunJob_DuplicateJob_ShouldThrowDuplicateKeyException()
        {
            //Arrange
            var job = new DummyJob("J1");
            var repo = new FakeJobRepository([job]);
            var sut = await JobPool.StartJobPool(repo);
            //Act //Assert
            Assert.Throws<DuplicateKeyException>(() => sut.RunJob(job));
            Assert.That(sut.RunningThreads, Is.EqualTo(1));
        }

        [Test]
        public async Task FetchJob_JobExists_ShouldReturnJob()
        {
            //Arrange
            var repo = new FakeJobRepository([new DummyJob("DUMMY_1")]);
            var sut = await JobPool.StartJobPool(repo);
            //Act
            var job = await sut.FetchJob<Job>("DUMMY_1");
            //Assert
            Assert.That(job, Is.Not.Null);
            Assert.That(job.Id, Is.EqualTo("DUMMY_1"));
        }

        [Test]
        public async Task FetchJob_JobDoesNotExist_ShouldReturnNull()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = await JobPool.StartJobPool(repo);
            //Act
            var job = await sut.FetchJob<Job>("DOES_NOT_EXIST_1");
            //Assert
            Assert.That(job, Is.Null);
        }

    }
}
