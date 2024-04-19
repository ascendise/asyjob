using AsyJob.Web.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using AsyJob.Lib.Tests.TestDoubles;
using System.Dynamic;
using AsyJob.Lib.Client.Jobs;
using AsyJob.Lib.Client.Abstract.Jobs;

namespace AsyJob.Web.Tests.Jobs
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
            var jobRunner = new JobRunner(new FakeJobPool(), new StubAuthorizationManager());
            var jobApi = new JobApi(jobFactory, jobRunner);
            var sut = new JobController(jobApi);
            var jobRequest = new JobRequest("FakeJob", "MyJob1", "LoremIpsum");
            //Act
            var response = await sut.RunJob(jobRequest);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Id, Is.EqualTo(guid));
                Assert.That(response.Name, Is.EqualTo("MyJob1"));
                Assert.That(response.Description, Is.EqualTo("LoremIpsum"));
            });
        }

        [Test]
        public async Task RunJob_WithInput_ShouldRunJobInCorrectState()
        {
            //Arrange
            var fakeJobFactory = new FakeJobWithInputFactory("FakeJobWithInputFactory", "FakeJob");
            var guid = "19D5D496-6046-485B-8E22-92E3923A4DCB";
            var jobFactory = new JobFactory(null, [fakeJobFactory], new FakeGuidProvider([new(guid)]));
            var jobRunner = new JobRunner(new FakeJobPool(), new StubAuthorizationManager());
            var jobApi = new JobApi(jobFactory, jobRunner);
            var sut = new JobController(jobApi);
            var input = new Dictionary<string, object?>
            {
                [nameof(FakeFactoryJobInput.CheckNum)] = 14
            };
            var jobRequest = new JobRequest("FakeJob", "MyJob1", "LoremIpsum", input);
            //Act
            var response = await sut.RunJob(jobRequest);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Id, Is.EqualTo(guid));
                Assert.That(response.Name, Is.EqualTo("MyJob1"));
                Assert.That(response.Description, Is.EqualTo("LoremIpsum"));
                Assert.That(input[nameof(FakeFactoryJobInput.CheckNum)], Is.EqualTo(14));

            });
        }

        [Test]
        public void RunJob_WithWrongInput_ShouldThrowException()
        {
            //Arrange
            var fakeJobFactory = new FakeJobWithInputFactory("FakeJobWithInputFactory", "FakeJob");
            var jobFactory = new JobFactory(null, [fakeJobFactory], new GuidProvider());
            var runner = new JobRunner(new FakeJobPool(), new StubAuthorizationManager());
            var jobApi = new JobApi(jobFactory, runner);
            var sut = new JobController(jobApi);
            dynamic wrongInput = new ExpandoObject();
            wrongInput.Wrong = "1!1";
            var jobRequest = new JobRequest("FakeJob", "Job", "", wrongInput);
            //Act //Assert
            Assert.ThrowsAsync<JobInputMismatchException>(() => sut.RunJob(jobRequest));
        }

        [Test]
        public void RunJob_UnknowJobType_ShouldThrowException()
        {
            //Arrange
            var jobFactory = new JobFactory(null, null, new GuidProvider());
            var runner = new JobRunner(new FakeJobPool(), new StubAuthorizationManager());
            var jobApi = new JobApi(jobFactory, runner);
            var sut = new JobController(jobApi);
            var jobRequest = new JobRequest("UnknownJob", "Job");
            //Act //Assert
            Assert.ThrowsAsync<NoMatchingJobFactoryException>(() => sut.RunJob(jobRequest));
        }


        [Test]
        public async Task FetchJob_ValidId_ShouldReturnCorrectJob()
        {
            //Arrange
            var pool = FakeJobPool.InitializePool([new DummyJob("DUMMY1")]);
            var runner = new JobRunner(pool, new StubAuthorizationManager());
            var jobApi = new JobApi(null!, runner);
            var sut = new JobController(jobApi);
            //Act
            var response = await sut.FetchJob("DUMMY1");
            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.EqualTo("DUMMY1"));
        }

        [Test]
        public void FetchJob_InvalidId_ShouldThrowException()
        {
            //Arrange
            var pool = FakeJobPool.InitializePool([new DummyJob("DUMMY1")]);
            var runner = new JobRunner(pool, new StubAuthorizationManager());
            var jobApi = new JobApi(null!, runner);
            var sut = new JobController(jobApi);
            //Act //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => sut.FetchJob("SOME_OTHER_ID"));
        }

    }
}
