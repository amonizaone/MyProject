using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ReportViewer reportViewer;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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
    }
    class ReportViewerForm : Form
    {
        private readonly ReportViewer reportViewer;

        public ReportViewerForm()
        {
            Text = "Report viewer";
            WindowState = FormWindowState.Maximized;
            reportViewer = new ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            Controls.Add(reportViewer);
        }

        protected override void OnLoad(EventArgs e)
        {
          //  Report.Load(reportViewer.LocalReport);
            reportViewer.RefreshReport();
            base.OnLoad(e);
        }
    }
}
