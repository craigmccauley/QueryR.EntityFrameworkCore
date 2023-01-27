using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.EntityFrameworkCore;
using QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database;
using QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Endpoints;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonOptions.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        jsonOptions.JsonSerializerOptions.WriteIndented = true;
    })
    .AddControllersAsServices(); //for DI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KerbalDbContext>(options =>
{
    options.UseSqlite("DataSource=example.db");
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    var configuration = MediatRConfigurationBuilder
        .Create(typeof(Program).Assembly)
        .WithAllOpenGenericHandlerTypesRegistered()
        .Build();
    builder.RegisterMediatR(configuration);

    // Mediator property injection into BaseController base class.
    var baseApiControllerType = typeof(BaseController);
    foreach (var controllerType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
        .Where(assemblyType => assemblyType != baseApiControllerType
            && baseApiControllerType.IsAssignableFrom(assemblyType)))
    {
        builder.RegisterType(controllerType).PropertiesAutowired();
    }

    builder.RegisterType<KerbalDbContext>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

    //register other type below here
    builder.RegisterType<QueryParametersMapper>()
        .AsImplementedInterfaces()
        .SingleInstance();
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
