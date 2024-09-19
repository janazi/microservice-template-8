using Asp.Versioning;
using MicroserviceTemplate.Api.IoC;
using MicroserviceTemplate.Infra.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using RulesEngine.Api.Common.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.ConfigureIoC();
const string AppEnvironmentConfigurationVariableName = "APP_ENVIRONMENT";
const string AppEnvironmentNameDev = "dev";
bool IsDevelopmentEnvironment = AppEnvironmentNameDev.Equals(builder.Configuration.GetValue(AppEnvironmentConfigurationVariableName, string.Empty));
// api versioning
builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.DefaultApiVersion = new ApiVersion(1.0);
})
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.SubstitutionFormat = "F";
        options.SubstituteApiVersionInUrl = true;
    });


if (builder.Environment.IsDevelopment() || IsDevelopmentEnvironment)
{
    // swagger
    builder.Services.AddSwaggerGen(options =>
    {
        // add a custom operation filter which sets default values
        options.OperationFilter<SwaggerDefaultValues>();
        options.CustomSchemaIds(type => type.ToString());
        options.CustomSchemaIds(s => s.FullName!.Replace("+", "."));

        // Enable Authorize button
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Add ONLY your JWT Bearer token below (Bearer will be added to the request programmatically",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
    });

    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
    options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//Necessary to run integration tests
public partial class Program { }