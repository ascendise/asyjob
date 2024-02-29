using AsyJob.Jobs;
using AsyJobTests.Jobs.Test_Doubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs
{
    internal class JobFactoryTests
    {

        [Test]
        public void CreateJob_MultipleFactoriesNoInput_ShouldCreateCorrectJob()
        {
            //Arrange
            var job1Factory = new FakeJobFactory("Job1Factory", "Job1");
            var job2Factory = new FakeJobFactory("Job2Factory", "Job2");
            var sut = new JobFactory([job1Factory, job2Factory], null);
            //Act
            var createdJob = sut.CreateJob("Job1", "1");
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
            var sut = new JobFactory([jobFactory], null);
            //Act //Assert
            Assert.Throws<NoMatchingJobFactoryException>(() => sut.CreateJob("SomeJob", "1"));
        }

        [Test]
        public void CreateJob_MultipleFactoriesWithInput_ShouldCreateCorreectJob()
        {
            //Arrange
            var job1Factory = new FakeJobWithInputFactory("Job1Factory", "Job1");
            var job2Factory = new FakeJobWithInputFactory("Job2Factory", "Job2");
            var sut = new JobFactory(null, [job1Factory, job2Factory]);
            //Act
            var createdJob = sut.CreateJob<DummyInput>("Job1", "1", new(14));
            //Assert
            Assert.That(createdJob, Is.Not.Null);
            Assert.That(createdJob, Is.InstanceOf<FakeJobFactoryOutputExtendedJob<DummyInput>>());
            var fakeJob = (createdJob as FakeJobFactoryOutputExtendedJob<DummyInput>)!;
            Assert.Multiple(() =>
            {
                Assert.That(fakeJob.FactoryName, Is.EqualTo("Job1Factory"));
                Assert.That(fakeJob.JobType, Is.EqualTo("Job1"));
                Assert.That(fakeJob.Input.CheckNum, Is.EqualTo(14));
            });
        }

        [Test]
        public void CreateJob_NoFactoriesWithInput_ShouldCreateCorrectJob()
        {
            //Arrange
            var jobFactory = new FakeJobWithInputFactory("JobFactory", "NoJob");
            var sut = new JobFactory(null, null);
            //Act //Assert
            Assert.Throws<NoMatchingJobFactoryException>(() => sut.CreateJob<DummyInput>("SomeJob", "1", new(14)));
        }
    }
}
