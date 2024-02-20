using AsyJob.Jobs;
using AsyJobTests.Jobs.Test_Doubles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs
{
    internal class JobRunnerServiceTests
    {

        [Test]
        public void RunJob_DummyJob_ShouldRunJob()
        {
            //Arrange
            var fakeJobPool = new FakeJobPool();
            var sut = new JobRunnerService(fakeJobPool);
            var spyJob = new SpyJob("TEST_1");
            //Act
            sut.RunJob(spyJob);
            JobTestUtils.WaitForJobCompletion(spyJob);
            //Assert
            Assert.That(spyJob.Result?.RunCount, Is.EqualTo(1));
        }

        [Test]
        public void RunJob_JobPool_ShouldRunTheJobThroughTheJobPool()
        {
            //Arrange
            var fakeJobPool = new FakeJobPool();
            var sut = new JobRunnerService(fakeJobPool);
            var dummyJob = new DummyJob("JOB_1");
            //Act
            sut.RunJob(dummyJob);
            //Assert
            Assert.That(fakeJobPool.JobThreads, Has.Count.EqualTo(1)); 
        }
    }
}
