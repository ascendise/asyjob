using AsyJob.Jobs;
using AsyJobTests.Jobs.Test_Doubles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs
{
    internal class JobControllerTests
    {

        [Test]
        public async Task RunJob_NoInput_ShouldRunJobInCorrectState()
        {
            //Arrange
            var guid = "19D5D496-6046-485B-8E22-92E3923A4DCB";
            var fakeJobFactory = new FakeJobFactory("FakeJobFactory", "FakeJob");
            var jobFactory = new JobFactory([fakeJobFactory], null, new FakeGuidProvider([new(guid)]));
            var jobRunner = new JobRunnerService(new FakeJobPool());
            var sut = new JobController(jobRunner, jobFactory);
            var jobRequest = new JobRequestDto("FakeJob", "MyJob1", "LoremIpsum");
            //Act
            var response = await sut.RunJob(jobRequest);
            //Assert
            var job = response.Job;
            Assert.That(job, Is.InstanceOf<FakeJobFactoryOutputJob>());
            Assert.Multiple(() =>
            {
                Assert.That(job.Id, Is.EqualTo(guid));
                Assert.That(job.Name, Is.EqualTo("MyJob1"));
                Assert.That(job.Description, Is.EqualTo("LoremIpsum"));
            });
        }

        [Test]
        public async Task RunJob_WithInput_ShouldRunJobInCorrectState()
        {
            //Arrange
            var fakeJobFactory = new FakeJobWithInputFactory("FakeJobWithInputFactory", "FakeJob");
            var guid = "19D5D496-6046-485B-8E22-92E3923A4DCB";
            var jobFactory = new JobFactory(null, [fakeJobFactory], new FakeGuidProvider([new(guid)]));
            var jobRunner = new JobRunnerService(new FakeJobPool());
            var sut = new JobController(jobRunner, jobFactory);
            var checkNum = 14;
            var jobRequest = new JobRequestDto("FakeJob", "MyJob1", "LoremIpsum", new FakeFactoryJobInput(checkNum));
            //Act
            var response = await sut.RunJob(jobRequest);
            //Assert
            var job = response.Job;
            Assert.That(job, Is.InstanceOf<FakeJobFactoryOutputExtendedJob>());
            var fakeJobWithInput = (job as FakeJobFactoryOutputExtendedJob)!;
            Assert.Multiple(() =>
            {
                Assert.That(fakeJobWithInput.Id, Is.EqualTo(guid));
                Assert.That(fakeJobWithInput.Name, Is.EqualTo("MyJob1"));
                Assert.That(fakeJobWithInput.Description, Is.EqualTo("LoremIpsum"));
                Assert.That(fakeJobWithInput.Input.CheckNum, Is.EqualTo(checkNum));

            });
        }

        [Test]
        public void RunJob_WithWrongInput_ShouldThrowException()
        {
            //Arrange
            var fakeJobFactory = new FakeJobWithInputFactory("FakeJobWithInputFactory", "FakeJob");
            var jobFactory = new JobFactory(null, [fakeJobFactory], new GuidProvider());
            var runner = new JobRunnerService(new FakeJobPool());
            var sut = new JobController(runner, jobFactory);
            var jobRequest = new JobRequestDto("FakeJob", "Job", "", "definetlyNotTheExpectedInput");
            //Act //Assert
            Assert.ThrowsAsync<JobInputMismatchException>(() => sut.RunJob(jobRequest));
        }

        [Test]
        public void RunJob_UnknowJobType_ShouldThrowException()
        {
            //Arrange
            var jobFactory = new JobFactory(null, null, new GuidProvider());
            var runner = new JobRunnerService(new FakeJobPool());
            var sut = new JobController(runner, jobFactory);
            var jobRequest = new JobRequestDto("UnknownJob", "Job");
            //Act //Assert
            Assert.ThrowsAsync<NoMatchingJobFactoryException>(() => sut.RunJob(jobRequest));
        }


        [Test]
        public async Task FetchJob_ValidId_ShouldReturnCorrectJob()
        {
            //Arrange
            var pool = FakeJobPool.InitializePool([new DummyJob("DUMMY1")]);
            var runner = new JobRunnerService(pool);
            var sut = new JobController(runner, null!);
            //Act
            var response = await sut.FetchJob("DUMMY1");
            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Job, Is.InstanceOf<DummyJob>());
            Assert.That(response.Job.Id, Is.EqualTo("DUMMY1"));
        }

        [Test]
        public void FetchJob_InvalidId_ShouldThrowException()
        {
            //Arrange
            var pool = FakeJobPool.InitializePool([new DummyJob("DUMMY1")]);
            var runner = new JobRunnerService(pool);
            var sut = new JobController(runner, null!);
            //Act //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => sut.FetchJob("SOME_OTHER_ID"));
        }

    }
}
