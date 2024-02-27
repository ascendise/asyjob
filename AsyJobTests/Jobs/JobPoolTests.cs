﻿using AsyJob;
using AsyJob.Jobs;
using AsyJobTests.Jobs.Test_Doubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public async Task StartJobPool_HasFinishedJobs_ShouldNotRestartJobs()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var job = new DummyJob("1");
            job.Run();
            await repo.SaveJob(job);
            //Act
            var sut = await JobPool.StartJobPool(repo);
            //Assert
            Assert.That(sut.RunningThreads, Is.EqualTo(0));
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
            Assert.That(spyJob.Output.RunCount, Is.EqualTo(1));
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

        [Test]
        public async Task FetchJob_JobExistsButWrongType_ShouldReturnNull()
        {
            //Arrange
            var repo = new FakeJobRepository([new DummyJob("J1")]);
            var sut = await JobPool.StartJobPool(repo);
            //Act
            var job = await sut.FetchJob<SpyJob>("J1");
            //Assert
            Assert.That(job, Is.Null);
        }

        [Test]
        public async Task FetchAll_HasJobs_ShouldReturnListOfJobs()
        {
            //Arrange
            List<Job> jobs = [new DummyJob("J1"), new DummyJob("J2")];
            var repo = new FakeJobRepository(jobs);
            var sut = await JobPool.StartJobPool(repo);
            //Act
            var foundJobs = await sut.FetchAll<Job>();
            //Assert
            CollectionAssert.AreEquivalent(jobs, foundJobs);
        }

        [Test]
        public async Task FetchAll_NoJobs_ShouldReturnEmptyList()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = await JobPool.StartJobPool(repo);
            //Act
            var foundJobs = await sut.FetchAll<Job>();
            //Assert
            Assert.That(foundJobs, Is.Empty);
        }


        [Test]
        public async Task FetchAll_SpecificJobType_ShouldOnlyReturnJobWithThisType()
        {
            //Arrange
            var dummyJobs = new List<DummyJob>() { new("DUMMY_1"), new("DUMMY_2") };
            var spyJobs = new List<SpyJob>() { new("SPY_1") };
            var jobs = new List<Job>(dummyJobs);
            jobs.AddRange(spyJobs);
            var repo = new FakeJobRepository(jobs);
            var sut = await JobPool.StartJobPool(repo);
            //Act
            var foundJobs = await sut.FetchAll<DummyJob>();
            //Assert
            CollectionAssert.AreEquivalent(dummyJobs, foundJobs);
        }


    }
}
