using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Hubs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.SignalR;

namespace api.Services
{
    public class DataGeneratorService: BackgroundService
    {
        private readonly IHubContext<MonitorHub> _hubContext;
        private readonly IDataService _dataService;
        private readonly Random _random = new();
        private readonly string[] _metricNames = new[] { "CPU", "Memory", "Network", "Disk" };

        public DataGeneratorService(IHubContext<MonitorHub> hubContext, IDataService dataService)
        {
            _hubContext = hubContext;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var metricName = _metricNames[_random.Next(_metricNames.Length)];
                    var value = Math.Round(_random.NextDouble() * 100, 2);
                    var status = value > 80 ? "Critical" : value > 60 ? "Warning" : "Normal";

                    var data = new MonitorData
                    {
                        Timestamp = DateTime.Now,
                        Value = value,
                        MetricName = metricName,
                        Status = status
                    };

                    await _dataService.AddDataPointAsync(data);
                    await _hubContext.Clients.All.SendAsync("ReceiveUpdate", data, cancellationToken: stoppingToken);

                    // Generate data at random intervals between 1-3 seconds
                    await Task.Delay(_random.Next(1000, 3000), stoppingToken);
                }
                catch (Exception) when (stoppingToken.IsCancellationRequested)
                {
                    // Suppress exceptions when cancellation is requested
                    break;
                }
                catch (Exception ex)
                {
                    // Log error but continue
                    Console.WriteLine($"Error generating data: {ex.Message}");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

    }
}