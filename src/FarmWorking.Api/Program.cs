using FarmWorking.Application.Abstractions;
using FarmWorking.Application.Services;
using FarmWorking.Infrastructure.Persistence;
using FarmWorking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var connectionString = builder.Configuration.GetConnectionString("FarmDatabase")
    ?? throw new InvalidOperationException("Connection string 'FarmDatabase' was not configured.");
builder.Services.AddDbContext<FarmDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IProcessRepository, ProcessRepository>();
builder.Services.AddScoped<ISupplyRepository, SupplyRepository>();
builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFarmService, FarmService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();

var app = builder.Build();

// Keep the local database in sync automatically. Without this, a fresh checkout
// starts successfully but every data request fails until migrations are run by hand.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FarmDbContext>();
    db.Database.Migrate();
}

app.UseCors();
app.UseStaticFiles();
app.MapControllers();
app.Run();
