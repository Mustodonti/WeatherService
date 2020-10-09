using Microsoft.AspNetCore.Http;
using OpenWeatherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeatherService
{
    public class CityTimestampMiddleware
    {
        private readonly RequestDelegate _next;//ссылка на тот делегат запроса, который стоит следующим в конвейере обработки запроса.

        public CityTimestampMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();
            if (path == "/v1/forecast/")
            {

                var city = context.Request.Query["city"];
                var timestamp = Convert.ToInt32(context.Request.Query["timestamp"]);
                if ((!string.IsNullOrWhiteSpace(city)&timestamp>0&timestamp<97))
                {
                    var keyapi = new KeyAPI("2d6d053b6a777cd0fe236dd2c0d9aa22");
                    var result = keyapi.DetectedForecastWeather(city,timestamp.ToString());

                    await context.Response.WriteAsync(result);
                }
            }
            else if (path == "/v1/current/")
            {
                var city = context.Request.Query["city"];
                if (!string.IsNullOrWhiteSpace(city))
                {
                    var keyapi = new KeyAPI("2d6d053b6a777cd0fe236dd2c0d9aa22");
                    var result = keyapi.DetectedCurrentWeather(city);

                    await context.Response.WriteAsync(result);
                }
            }
            else
            {
                context.Response.StatusCode = 404;
            }
            await _next.Invoke(context);
        }
    }
}
