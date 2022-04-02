var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

List<WeatherForecast> listWeatherForecast = new List<WeatherForecast>();
var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
listWeatherForecast.AddRange(forecast);

app.MapGet("/weatherforecast", () =>
{
    return listWeatherForecast;
})
.WithName("GetWeatherForecast");

app.MapPut("/addWeatherForcast", (int howmanydaysfromtoday, int temperature) => {
    Console.WriteLine("add new weather information");
    var thisForecast =
       new WeatherForecast
       (
           DateTime.Now.AddDays(howmanydaysfromtoday),
           temperature,
           summaries[Random.Shared.Next(summaries.Length)]
       );
    listWeatherForecast.Add(thisForecast);
    return "Insert weather information is successed.";
});

app.MapPost("/getWeatheroftheDay", (int dayfromtoday) => {
    var forecastoftheday = listWeatherForecast.Where(forecast => forecast.Date.Date == DateTime.Now.AddDays(dayfromtoday).Date).Select(forecast => forecast).ToArray();
    return forecastoftheday;
});

app.MapDelete("/remWeatheroftheDay", (int dayfromtoday) => {
    List<WeatherForecast> removeforecast = new List<WeatherForecast>();
    listWeatherForecast.ForEach(forecast => {
        if (forecast.Date.Date == DateTime.Now.AddDays(dayfromtoday).Date)
            removeforecast.Add(forecast);
            //listWeatherForecast.Remove(forecast);
    });
    listWeatherForecast = listWeatherForecast.Except(removeforecast).ToList();

    return "Delete weather information of the day is successed.";
});

// app.MapGet("/", () => "Hello World!");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}