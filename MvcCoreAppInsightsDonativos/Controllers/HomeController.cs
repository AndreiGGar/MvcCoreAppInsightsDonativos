using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAppInsightsDonativos.Models;
using System.Diagnostics;

namespace MvcCoreAppInsightsDonativos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TelemetryClient _telemetryClient;

        public HomeController(ILogger<HomeController> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string usuario, int cantidad)
        {
            ViewData["MENSAJE"] = "Su donativo de " + cantidad + " ha sido aceptado. Muchas gracias " + usuario;
            this._telemetryClient.TrackEvent("DonativosRequest");
            MetricTelemetry metric = new MetricTelemetry();
            metric.Name = "Donativos";
            metric.Sum = cantidad;
            this._telemetryClient.TrackMetric(metric);
            string mensaje = "";
            SeverityLevel level;
            if (cantidad == 0)
            {
                level = SeverityLevel.Error;
            }
            else if (cantidad <= 20)
            {
                level = SeverityLevel.Warning;
            } else
            {
                level = SeverityLevel.Information;
            }
            mensaje = usuario + " - " + cantidad + "€";
            TraceTelemetry traza = new TraceTelemetry(mensaje, level);
            this._telemetryClient.TrackTrace(traza);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}