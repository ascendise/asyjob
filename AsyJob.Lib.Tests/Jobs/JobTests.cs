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

        [Test]
        public void Update_CorrectJobType_ShouldUpdateJobWithNewValues()
        {
            //Arrange
            var job = new ValueJob<int>("MyJob1", 31);
            var newJob = new ValueJob<int>("MyJob2", 42);
            //Act
            job.Update(newJob);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(job.Id, Is.EqualTo(newJob.Id));
                Assert.That(job.Name, Is.EqualTo(newJob.Name));
                Assert.That(job.Description, Is.EqualTo(newJob.Description));
                Assert.That(job.Status, Is.EqualTo(newJob.Status));
                Assert.That(job.Value, Is.EqualTo(newJob.Value));
            });
        }

        [Test]
        public void Update_IncorrectType_ShouldUpdateJobWithNewValues()
        {
            //Arrange
            Job dummyJob = new DummyJob("DJ1");
            Job valueJob = new ValueJob<int>("VJ1", 1337);
            //Act //Assert
            Assert.Throws<ArgumentException>(() => valueJob.Update(dummyJob));
        }
    }
}
