using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AsyJob.Lib.Auth;
using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Lib.Client.Jobs;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using AsyJob.Web;
using AsyJob.Web.Auth;
using AsyJob.Web.HAL.Json;
using AsyJob.Web.Jobs;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson.Serialization;

using User = AsyJob.Web.Auth.User;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.Converters = JsonHal.Converters;
});

//Jobs
builder.Services.AddTransient<IJobRepository, JobMongoRepository>();
builder.Services.AddSingleton<IJobPool, JobPool>(sp =>
{
    var repo = sp.GetRequiredService<IJobRepository>();
    return JobPool.StartJobPool(repo).Result!;
});
builder.Services.AddTransient<IJobApi, JobApi>(sp =>
{
    var jobFactory = new JobFactory(
        [],
        [new TimerJobFactory(), new DiceRollJobFactory(), new RNGJobFactory(), new CounterJobFactory()],
        new GuidProvider());
    var pool = sp.GetRequiredService<IJobPool>();
    var authManager = new AuthorizationManager();
    var user = sp.GetService<AsyJob.Lib.Auth.User>();
    var jobRunner = new JobRunner(pool, authManager, user);
    return new(jobFactory, jobRunner);
});
builder.Services.AddHostedService<JobPoolBackgroundService>();

//MongoDB
//Tell BsonMapper which Subtypes for Job exist for deserialization
//https://stackoverflow.com/a/24344620/10281237
//TODO: Load all jobs dynamically
BsonClassMap.RegisterClassMap<Job>();
BsonClassMap.RegisterClassMap<DiceRollJob>();
BsonClassMap.RegisterClassMap<TimerJob>();
BsonClassMap.RegisterClassMap<RNGJob>();
BsonClassMap.RegisterClassMap<CounterJob>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authorization
var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration()
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = builder.Configuration.GetConnectionString("MongoDB")!,
        DatabaseName = "asyJob"
    },
    IdentityOptionsAction = options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 10;

        // ApplicationUser settings
        options.User.RequireUniqueEmail = true;
    }
};
builder.Services.AddTransient<IAuthorizationHandler, HasRightsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, HasRightsPolicyProvider>();
builder.Services.ConfigureMongoDbIdentity<User, Role, Guid>(mongoDbIdentityConfiguration);
builder.Services.AddIdentityApiEndpoints<User>();
//Add Domain user to DI.
//Converts the Identity Framework User to a Domain User
builder.Services.AddScoped<User>();
builder.Services.AddScoped(sp =>
{
    var mongoUser = sp.GetService<User>();
    return mongoUser!.GetDomainUser();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapIdentityApi<User>();

app.MapControllers();

app.Run();
