using ConfigureManager;
using ConfigureManager.ServiceManager;
using Coravel;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json.Converters;

List<string> waitList = new List<string>()
    {
        "ReturningBook",
        "BorrowingBook"
    };

var builder = WebApplication.CreateBuilder(args);

// get valiable from appsettings.json
int hour = builder.Configuration.GetValue<int>("SchedulerConfig:DailyAtHour");
int sec = builder.Configuration.GetValue<int>("SchedulerConfig:DailyAtSecound");

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add
    (
        new StringEnumConverter(namingStrategy: null, allowIntegerValues: false) // use this to show enum value as string in json response
    );
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.VersioningServices();
builder.Services.SwaggerSecurity();
builder.Services.RepoConfigure();
builder.Services.DBConfigure(builder.Configuration);
builder.Services.JwtConfig(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                "Mini Library Management System- "+description.GroupName.ToUpperInvariant());
        // sets the swagger ui at apps root namespace
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Use(async (context, next) =>
{    
    RouteData data = context.GetRouteData();
    if (waitList.Contains(data.Values["controller"]?.ToString()?.ToLower()))
    {
        await Task.Delay(2000);
    }
    await next();
});

app.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<DailyDueEmailScheduleJob>()
        .DailyAt(hour,sec); // we use 24-hour format
});


app.Run();
