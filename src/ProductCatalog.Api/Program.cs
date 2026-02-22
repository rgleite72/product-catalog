using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Extensions;
using ProductCatalog.Api.Middlewares;
using ProductCatalog.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
});

// MVC + comportamento custom
builder.Services.AddControllers();
builder.Services.AddCustomApiBehavior();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não foi configurada.");

// DbContext (único)
builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
    options.UseNpgsql(connectionString));

// Health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Routing primeiro (padrão)
app.UseRouting();

// Correlation cedo (para TraceId existir em tudo)
app.UseMiddleware<RequestCorrelationMiddleware>();

// Exception handling cedo (pega tudo que vem depois)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Request logging (envolve o resto e garante log final com finally)
app.Use(async (ctx, next) =>
{
    var logger = ctx.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("Request");

    var traceId = RequestCorrelationMiddleware.GetRequestId(ctx);

    logger.LogInformation("HTTP {Method} {Path} TraceId={TraceId}",
        ctx.Request.Method, ctx.Request.Path, traceId);

    try
    {
        await next();
    }
    finally
    {
        logger.LogInformation("HTTP {StatusCode} {Method} {Path} TraceId={TraceId}",
            ctx.Response.StatusCode, ctx.Request.Method, ctx.Request.Path, traceId);
    }
});

// Auth (se não tiver auth agora, pode deixar; não atrapalha)
app.UseAuthorization();

// Endpoints
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();