using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string? tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("OpenTelemetryExample.API"))
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();

        metrics.AddOtlpExporter();
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();

        if (!string.IsNullOrWhiteSpace(tracingOtlpEndpoint))
        {
            tracing.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
            });
        }
        else
        {
            tracing.AddConsoleExporter();
        }
    });

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

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
