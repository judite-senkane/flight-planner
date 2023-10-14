
using FlightPlanner.Core.Interfaces;
using FlightPlanner.Data;
using FlightPlanner.Handlers;
using FlightPlanner.Services.Extensions;
using FlightPlanner.Validations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<FlightPlannerDbContext>(options => 
                options.UseSqlServer(builder.Configuration
                    .GetConnectionString("flight-planner")));
            builder.Services.RegisterServices();
            builder.Services.AddTransient<IValidate, AirportValuesValidator>();
            builder.Services.AddTransient<IValidate, FlightDatesValidator>();
            builder.Services.AddTransient<IValidate, FlightValuesValidator>();
            builder.Services.AddTransient<IValidate, SameAirportValidator>();
            var mapper = AutoMapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}