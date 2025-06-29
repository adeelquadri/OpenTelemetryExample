using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
//using OpenTelemetryExample.API.Configs;
using OpenTelemetryExample.API.Services;
using OpenTelemetryExample.API.Services.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMetricsService, MetricsService>();

//string? tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
//builder.Services.AddOpenTelemetry()
//    .ConfigureResource(resource => resource.AddService("OpenTelemetryExample.API"))
//    .WithMetrics(metrics =>
//    {
//        metrics
//            .AddAspNetCoreInstrumentation()
//            .AddHttpClientInstrumentation()
//            .AddConsoleExporter();

//        metrics.AddOtlpExporter();
//    })
//    .WithTracing(tracing =>
//    {
//        tracing
//            .AddAspNetCoreInstrumentation()
//            .AddHttpClientInstrumentation();

//        if (!string.IsNullOrWhiteSpace(tracingOtlpEndpoint))
//        {
//            tracing.AddOtlpExporter(otlpOptions =>
//            {
//                otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
//            });
//        }
//        else
//        {
//            tracing.AddConsoleExporter();
//        }
//    });

//builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

//string? tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.AddOtlpExporter();
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter(AppDomain.CurrentDomain.FriendlyName);

        metrics.AddOtlpExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.AddSource(builder.Environment.ApplicationName)
            .AddAspNetCoreInstrumentation()
            // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
            //.AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation();

        tracing.AddOtlpExporter();
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

app.MapControllers();

app.Run();
