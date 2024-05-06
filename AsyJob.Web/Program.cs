using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AsyJob.Lib.Auth;
using AsyJob.Lib.Auth.Users;
using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Lib.Client.Abstract.Users;
using AsyJob.Lib.Client.Jobs;
using AsyJob.Lib.Client.Users;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;
using AsyJob.Web;
using AsyJob.Web.Auth;
using AsyJob.Web.Auth.Rights;
using AsyJob.Web.Errors;
using AsyJob.Web.Errors.NotFound;
using AsyJob.Web.HAL.Json;
using AsyJob.Web.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Security.Claims;
using User = AsyJob.Web.Auth.User;

internal partial class Program
{
    private static int i = 2;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllers().AddNewtonsoftJson(o =>
        {
            o.SerializerSettings.Converters = JsonHal.Converters;
        });
        //Mapping
        builder.Services.AddTransient<IMapper<UserResponse, HalUserResponse>, UserResponseToHalMapper>();
        builder.Services.AddTransient<IMapper<User, UserResponse>, UserToUserResponseMapper>();

        //Error Handling
        builder.Services.AddTransient<ErrorResponseFactory>();
        builder.Services.AddTransient<IErrorResponseFactory, NotFoundErrorFactory>();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<ExceptionHandler>();

        //Jobs
        builder.Services.AddTransient<IJobRepository, JobMongoRepository>();
        builder.Services.AddSingleton<IJobPool, JobPool>(sp =>
        {
            var repo = sp.GetRequiredService<IJobRepository>();
            return JobPool.StartJobPool(repo).Result!;
        });
        builder.Services.AddTransient<IJobApi, JobApi>(sp =>
        {
            using var scope = sp.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var jobFactory = new JobFactory(
                [],
                [new TimerJobFactory(), new DiceRollJobFactory(), new RNGJobFactory(), new CounterJobFactory()],
                new GuidProvider());
            var pool = serviceProvider.GetRequiredService<IJobPool>();
            var authManager = new AuthorizationManager();
            var user = serviceProvider.GetService<AsyJob.Lib.Auth.User?>();
            var jobRunner = new JobRunner(pool, authManager, user);
            return new(jobFactory, jobRunner);
        });
        builder.Services.AddTransient<IUsersApi, UsersApi>(sp =>
        {
            using var scope = sp.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var aspUserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var userRepo = new UserRepository(aspUserManager);
            var user = serviceProvider.GetRequiredService<User>();
            var userManager = new UserManager(new AuthorizationManager(), userRepo, user?.ToDomainUser());
            return new(userManager);
        });
        builder.Services.AddHostedService<JobPoolBackgroundService>();

        //MongoDB
        builder.Services.AddTransient(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            return client.GetDatabase(config["DatabaseName"]);
        });
        //Tell BsonMapper which Subtypes for Job exist for deserialization
        //https://stackoverflow.com/a/24344620/10281237
        //TODO: Load all jobs dynamically
        var map = BsonClassMap.GetRegisteredClassMaps();
        //FIXME: Hack for getting integration tests to work.
        //Find way to add this information by injecting it into the MongoClient or something
        if(!map.Any())
        {
            BsonClassMap.RegisterClassMap<Job>();
            BsonClassMap.RegisterClassMap<DiceRollJob>();
            BsonClassMap.RegisterClassMap<TimerJob>();
            BsonClassMap.RegisterClassMap<RNGJob>();
            BsonClassMap.RegisterClassMap<CounterJob>();
        }

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Authorization
        var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration()
        {
            MongoDbSettings = new MongoDbSettings
            {
                ConnectionString = builder.Configuration.GetConnectionString("MongoDB")!,
                DatabaseName = builder.Configuration["DatabaseName"] ?? "asyJob"
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

                //Signin Settings
                options.SignIn.RequireConfirmedAccount = true;
            }
        };
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, HasRightsPolicyProvider>();
        builder.Services.AddTransient<IAuthorizationHandler, HasRightsAuthorizationHandler>();
        builder.Services.AddTransient<IAspUserManager, UserManagerWrapper>();
        builder.Services.ConfigureMongoDbIdentity<User, Role, Guid>(mongoDbIdentityConfiguration)
            .AddUserConfirmation<UserConfirmationService>();
        builder.Services.AddIdentityApiEndpoints<User>();
        //Add Domain user to DI.
        //Converts the Identity Framework User to a Domain User
        builder.Services.AddScoped(sp =>
        {
            var context = sp.GetService<IHttpContextAccessor>();
            var user = context?.HttpContext?.User;
            var id = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var anonymous = new User();
            if (id is null)
                return anonymous;
            var userManager = sp.GetRequiredService<UserManager<User>>();
            return userManager.FindByIdAsync(id).Result ?? anonymous;

        });
        builder.Services.AddScoped(sp =>
        {
            var mongoUser = sp.GetRequiredService<User>();
            return mongoUser!.ToDomainUser();
        });

        var app = builder.Build();
        app.UseExceptionHandler();
        app.AddAdminUser().Wait();

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
    }
}