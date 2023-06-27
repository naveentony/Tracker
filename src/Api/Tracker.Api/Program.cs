using System.Reflection;
using Tracker.Api.Extensions;
using Tracker.Features;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();
// Add services to the container.


// Registers handlers and mediator types from the specified assemblies.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});
builder.Features();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();


app.RegisterEndpointDefinitions();
app.Run();


/*
using System.Reflection;
using Tracker.Api;
using Tracker.Api.Extensions;
using Tracker.Domain;
using Tracker.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.RegisterServices();
// Registers handlers and mediator types from the specified assemblies.
builder.Features();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

builder.Services.AddTransient<IStudentsRepository, StudentsRepository>();
builder.Services.AddScoped<IStudentsService, StudentsService>();

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();



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

app.RegisterEndpointDefinitions();
app.Run();
*/