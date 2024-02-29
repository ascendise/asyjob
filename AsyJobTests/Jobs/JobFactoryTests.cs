using AsyJob.Jobs;
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
            var sut = new JobFactory([job1Factory, job2Factory]);
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
            var sut = new JobFactory([jobFactory]);
            //Act //Assert
            Assert.Throws<NoMatchingJobFactoryException>(() => sut.CreateJob("SomeJob", "1"));
        }
    }
}
