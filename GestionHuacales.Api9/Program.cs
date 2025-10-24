
using GestionHuacales.Api9.DATA;
using GestionHuacales.Api9.Models;
//using GestionHuacales.Api9.Services;
using Microsoft.EntityFrameworkCore;

namespace GestionHuacales.Api9;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var ConnectionString = builder.Configuration.GetConnectionString("ConStr");

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "AllowFrontend",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        builder.Services.AddDbContextFactory<Contexto>(option => option.UseSqlite(ConnectionString));
       //builder.Services.AddScoped<EntradasGuacalesService>();
        //builder.Services.AddScoped<TiposHuacalesService>();

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionHuacales API V1");
                c.RoutePrefix = string.Empty;
            });
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionHuacales API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}