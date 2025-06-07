using api.Hubs;
using api.Interfaces;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// // Configure CORS to allow requests from the frontend
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("CorsPolicy", builder =>
//         builder
//             // .WithOrigins("http://localhost:5002") // Frontend URL
//             .WithOrigins("http://localhost:5002", "http://163.50.57.112:8082/monitor_realtime_front", "http://localhost:8081/monitor_realtime_front")
//             .AllowAnyMethod()
//             .AllowAnyHeader()
//             .AllowCredentials());
// });

// Register custom services
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddHostedService<DataGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
//     app.UseDeveloperExceptionPage();
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        // // IIS with sub domain/sub folder
        // c.SwaggerEndpoint("/monitor_realtime_api/swagger/v1/swagger.json", "Api monitor realtime V1");

        // Without sub domain/sub folder
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Factory verification V1");

        c.RoutePrefix = string.Empty; // Set this to an empty string if you want it at root URL (http://localhost:5000/)
    });
}

app.UseHttpsRedirection();
app.UseRouting();
// app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();
app.MapHub<MonitorHub>("/monitorHub");

app.Run();
