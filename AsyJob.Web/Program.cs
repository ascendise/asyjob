using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AsyJob.Lib.Auth;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using AsyJob.Web.Auth;
using AsyJob.Web.HAL.Json;
using AsyJob.Web.Jobs;
using AsyJob.Web.Mapping;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson.Serialization;

using User = AsyJob.Web.Auth.User;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.Converters = JsonHal.Converters;
});

//Dependency Injection
builder.Services.AddTransient<IGuidProvider, GuidProvider>();
builder.Services.AddTransient<IJobWithInputFactory, TimerJobFactory>();
builder.Services.AddTransient<IJobWithInputFactory, DiceRollJobFactory>();
builder.Services.AddTransient<IJobWithInputFactory, RNGJobFactory>();
builder.Services.AddTransient<JobFactory>();
builder.Services.AddTransient<IJobRepository, JobMongoRepository>();
builder.Services.AddTransient<IJobRunner, JobRunner>();
builder.Services.AddScoped<IJobPool, JobPool>(sp =>
{
    var repo = sp.GetService<IJobRepository>();
    return Task.Run(() => JobPool.StartJobPool(repo!)).Result;
});
builder.Services.AddTransient<IAuthorizationManager, AuthorizationManager>();
//Mapping
builder.Services.AddTransient<IMapper<Job, JobResponseDto>, JobResponseDtoMapper>();

//MongoDB
//Tell BsonMapper which Subtypes for Job exist for deserialization
//https://stackoverflow.com/a/24344620/10281237
//TODO: Load all jobs dynamically
BsonClassMap.RegisterClassMap<Job>();
BsonClassMap.RegisterClassMap<DiceRollJob>();
BsonClassMap.RegisterClassMap<TimerJob>();
BsonClassMap.RegisterClassMap<RNGJob>();

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