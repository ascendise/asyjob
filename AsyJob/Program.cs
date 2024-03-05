using AsyJob.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Dependency Injection
builder.Services.AddTransient<IGuidProvider, GuidProvider>();
builder.Services.AddTransient<IJobWithInputFactory, TimerJobFactory>();
builder.Services.AddTransient<IJobWithInputFactory, DiceRollJobFactory>();
builder.Services.AddTransient<JobFactory>();
builder.Services.AddTransient<IJobRepository, JobMongoRepository>();
builder.Services.AddTransient<IJobRunner, JobRunnerService>();
builder.Services.AddSingleton<IJobPool, JobPool>(sp =>
{
    var repo = sp.GetService<IJobRepository>();
    return Task.Run(() => JobPool.StartJobPool(repo!)).Result;
});


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
