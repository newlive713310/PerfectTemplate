using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using PerfectTemplate.Host.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(options =>
{
}).AddJsonTranscoding();
builder.Services.AddGrpcReflection();
builder.Services.AddGrpcHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
});

//builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(opt =>
//        opt.UseNpgsql(builder.Configuration.GetConnectionString("EmailConnection")));

var app = builder.Build();

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardReleaseV1");
    });
    app.MapGrpcReflectionService();
}

//ApplyMigration();

app.MapGrpcHealthChecksService();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

//void ApplyMigration()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var _Db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
//        if (_Db != null)
//        {
//            if (_Db.Database.GetPendingMigrations().Any())
//            {
//                _Db.Database.Migrate();
//            }
//        }
//    }
//}