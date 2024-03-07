using AsyJob.Jobs;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using MongoDB.Bson.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

//Dependency Injection
builder.Services.AddTransient<IGuidProvider, GuidProvider>();
builder.Services.AddTransient<IJobWithInputFactory, TimerJobFactory>();
builder.Services.AddTransient<IJobWithInputFactory, DiceRollJobFactory>();
builder.Services.AddTransient<JobFactory>();
builder.Services.AddTransient<IJobRepository, JobMongoRepository>();
builder.Services.AddTransient<IJobRunner, JobRunner>();
builder.Services.AddSingleton<IJobPool, JobPool>(sp =>
{
    var repo = sp.GetService<IJobRepository>();
    return Task.Run(() => JobPool.StartJobPool(repo!)).Result;
});

//MongoDB
//Tell BsonMapper which Subtypes for Job exist for deserialization
//https://stackoverflow.com/a/24344620/10281237
//TODO: Load all jobs dynamically
BsonClassMap.RegisterClassMap<Job>();
BsonClassMap.RegisterClassMap<DiceRollJob>();
BsonClassMap.RegisterClassMap<TimerJob>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
