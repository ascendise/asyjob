using AsyJob.Lib.Jobs;
using AsyJob.Lib.Tests.TestDoubles;

namespace AsyJob.Lib.Tests.Jobs
{
    internal class JobTests
    {

        [Test]
        public void Construction_Creation_ShouldHaveStatusCreated()
        {
            //Arrange //Act
            var dummyJob = new DummyJob("JOB_1");
            //Assert
            Assert.That(dummyJob.Status, Is.EqualTo(ProgressStatus.Created));
        }


        [Test]
        public void Run_SuccessfulExecution_ShouldSetStatusCorrectlyThroughRun()
        {
            //Arrange
            var spyJob = new SpyJob("JOB_1");
            //Act
            spyJob.Run();
            //Assert
            Assert.That(spyJob.Output.RunCount, Is.EqualTo(1));
            var run = spyJob.Output.Runs.First();
            Assert.Multiple(() =>
            {
                Assert.That(run.OnCreation.Status, Is.EqualTo(ProgressStatus.Created));
                Assert.That(run.OnPreRun.Status, Is.EqualTo(ProgressStatus.Waiting));
                Assert.That(run.OnRun.Status, Is.EqualTo(ProgressStatus.Running));
                Assert.That(run.OnPostRun.Status, Is.EqualTo(ProgressStatus.Running));
                Assert.That(spyJob.Status, Is.EqualTo(ProgressStatus.Done));
            });
        }

        [Test]
        public void Run_OnUnhandledException_ShouldSetErrorOnJob()
        {
            //Arrange
            var errorJob = new ErrorJob("JOB_1");
            //Act
            errorJob.Run();
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(errorJob.Status, Is.EqualTo(ProgressStatus.Error));
                Assert.That(errorJob.Error, Is.Not.Null);
            });
        }
    }
}
