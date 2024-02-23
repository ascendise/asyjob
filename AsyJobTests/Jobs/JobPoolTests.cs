﻿using AsyJob.Jobs;
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
        public void RunJob_Persistance_ShouldStoreJob()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = new JobPool(repo);
            var job = new DummyJob("JOB_1");
            //Act
            sut.RunJob(job);
            //Assert
            Assert.That(repo.Jobs, Has.Count.EqualTo(1));
        }

        [Test]
        public void RunJob_Job_ShouldRunJob()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = new JobPool(repo);
            var spyJob = new SpyJob("SPY_1");
            //Act   
            sut.RunJob(spyJob);
            JobTestUtils.WaitForJobCompletion(spyJob);
            //Assert
            Assert.That(spyJob.Result.RunCount, Is.EqualTo(1));
        }


        [Test]
        public void RunJob_Async_ShouldRunJobInNewThread()
        {
            //Arrange
            var repo = new FakeJobRepository();
            var sut = new JobPool(repo);
            var job = new SleepJob("SLEEP_1", new SleepInput(int.MaxValue));
            //Act
            sut.RunJob(job);
            //Assert
            Assert.That(job.Status, Is.Not.EqualTo(ProgressStatus.Done),
                "Job was completed, which means RunJob didn't return until job was finished.");
        }

        [Test]
        public async Task FetchJob_JobExists_ShouldReturnJob()
        {
            //Arrange
            var repo = new FakeJobRepository([new DummyJob("DUMMY_1")]);
            var sut = new JobPool(repo);
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
            var sut = new JobPool(repo);
            //Act
            var job = await sut.FetchJob<Job>("DOES_NOT_EXIST_1");
            //Assert
            Assert.That(job, Is.Null);
        }

    }
}
