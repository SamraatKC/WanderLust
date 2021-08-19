using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WanderLust.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Common.HostgatorEmailHelper heh;
        private readonly Common.EmailHelper emailHelper;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, Common.HostgatorEmailHelper _heh, Common.EmailHelper _emailHelper)
        {
            _logger = logger;
            heh = _heh;
            emailHelper = _emailHelper;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

       [HttpGet]
       [Route("SendEmail_Thru_Hostgator")]
       public void SendEmailViaHostgator()
        {
            heh.SendEmail();
        }
        [HttpGet]
        [Route("SendEmail_Thru_Google")]
        public void SendEmail_Thru_Google()
        {
            string subject = "TEST TEST TEST";
            string receiver = "ramkesharthapa15@gmail.com";
            string body = "<html><head></head><body><h1>Hello World!<br/> Message from google.</h1></body></html>";
            emailHelper.SendEmail(subject,receiver, body);
        }
    }
}
