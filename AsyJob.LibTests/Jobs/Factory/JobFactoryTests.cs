using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Tests.TestDoubles;

namespace AsyJob.Lib.Tests.Jobs.Factory
{
    internal class JobFactoryTests
    {

        [Test]
        public void CreateJob_MultipleFactoriesNoInput_ShouldCreateCorrectJob()
        {
            //Arrange
            var job1Factory = new FakeJobFactory("Job1Factory", "Job1");
            var job2Factory = new FakeJobFactory("Job2Factory", "Job2");
            var guidProvider = new FakeGuidProvider([Guid.NewGuid()]);
            var sut = new JobFactory([job1Factory, job2Factory], null, guidProvider);
            //Act
            var createdJob = sut.CreateJob("Job1");
            //Assert
            Assert.That(createdJob, Is.Not.Null);
            Assert.That(createdJob, Is.InstanceOf<FakeJobFactoryOutputJob>());
            var fakeJob = (createdJob as FakeJobFactoryOutputJob)!;
            Assert.Multiple(() =>
            {
                Assert.That(fakeJob.FactoryName, Is.EqualTo("Job1Factory"));
                Assert.That(fakeJob.JobType, Is.EqualTo("Job1"));
            });
        }

        [Test]
        public void CreateJob_NoMatchingFactoryNoInput_ShouldThrowException()
        {
            //Arrange
            var jobFactory = new FakeJobFactory("JobFactory", "NoJob");
            var guidProvider = new FakeGuidProvider([Guid.NewGuid()]);
            var sut = new JobFactory([jobFactory], null, guidProvider);
            //Act //Assert
            Assert.Throws<NoMatchingJobFactoryException>(() => sut.CreateJob("SomeJob"));
        }

        [Test]
        public void CreateJob_MultipleFactoriesWithInput_ShouldCreateCorreectJob()
        {
            //Arrange
            var job1Factory = new FakeJobWithInputFactory("Job1Factory", "Job1");
            var job2Factory = new FakeJobWithInputFactory("Job2Factory", "Job2");
            var guidProvider = new FakeGuidProvider([Guid.NewGuid()]);
            var sut = new JobFactory(null, [job1Factory, job2Factory], guidProvider);
            //Act
            var createdJob = sut.CreateJobWithInput("Job1", new FakeFactoryJobInput(14));
            //Assert
            Assert.That(createdJob, Is.Not.Null);
            Assert.That(createdJob, Is.InstanceOf<FakeJobFactoryOutputExtendedJob>());
            var fakeJob = (createdJob as FakeJobFactoryOutputExtendedJob)!;
            Assert.Multiple(() =>
            {
                Assert.That(fakeJob.FactoryName, Is.EqualTo("Job1Factory"));
                Assert.That(fakeJob.JobType, Is.EqualTo("Job1"));
                Assert.That(fakeJob.Input.CheckNum, Is.EqualTo(14));
            });
        }

        [Test]
        public void CreateJob_NoFactoriesWithInput_ShouldThrowException()
        {
            //Arrange
            var jobFactory = new FakeJobWithInputFactory("JobFactory", "NoJob");
            var guidProvider = new FakeGuidProvider([Guid.NewGuid()]);
            var sut = new JobFactory(null, null, guidProvider);
            //Act //Assert
            Assert.Throws<NoMatchingJobFactoryException>(() => sut.CreateJobWithInput("SomeJob", new FakeFactoryJobInput(14)));
        }

        [Test]
        public void CreateJob_FactoryWithWrongInput_ShouldThrowException()
        {
            //Arrange
            var jobFactory = new FakeJobWithInputFactory("JobFactory", "Job");
            var guidProvider = new FakeGuidProvider([Guid.NewGuid()]);
            var sut = new JobFactory(null, [jobFactory], guidProvider);
            //Act //Assert
            Assert.Throws<JobInputMismatchException>(() => sut.CreateJobWithInput("Job", "1"));
        }

        [Test]
        public void CreateJob_UniqueId_ShouldGenerateUniqueId()
        {
            //Arrange
            var jobFactory = new FakeJobFactory("JobFactory", "Job");
            var guid = "9E5C66CE-8419-4DB6-AD03-B4EF4568FA83";
            var guidProvider = new FakeGuidProvider([Guid.Parse(guid)]);
            var sut = new JobFactory([jobFactory], null, guidProvider);
            //Act
            var createdJob = sut.CreateJob("Job");
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(createdJob.Id, Is.EqualTo(guid));
                Assert.That(createdJob.Name, Is.EqualTo($"JOB-{guid}"));
            });
        }

        [Test]
        public void CreateJob_WithInputUniqueId_ShouldGenerateUniqueId()
        {
            //Arrange
            var jobFactory = new FakeJobWithInputFactory("JobFactory", "Job");
            var guid = "9E5C66CE-8419-4DB6-AD03-B4EF4568FA83";
            var guidProvider = new FakeGuidProvider([Guid.Parse(guid)]);
            var sut = new JobFactory(null, [jobFactory], guidProvider);
            //Act
            var createdJob = sut.CreateJobWithInput("Job", new FakeFactoryJobInput(12));
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(createdJob.Id, Is.EqualTo(guid));
                Assert.That(createdJob.Name, Is.EqualTo($"JOB-{guid}"));
            });
        }
    }
}
