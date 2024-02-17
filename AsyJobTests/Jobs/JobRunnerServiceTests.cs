using AsyJob.Jobs;
using AsyJobTests.Jobs.Test_Doubles;
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
            var sut = new JobRunnerService();
            var spyJob = new SpyJob("TEST_1");
            //Act
            sut.RunJob(spyJob);
            JobTestUtils.WaitForJobCompletion(spyJob);
            //Assert
            Assert.That(spyJob.Result?.RunCount, Is.EqualTo(1));
        }
    }
}
