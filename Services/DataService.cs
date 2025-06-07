using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class DataService : IDataService
    {
        private readonly List<MonitorData> _dataPoints = new();
        private readonly object _lock = new();
        public Task<MonitorData> AddDataPointAsync(MonitorData data)
        {
            lock (_lock)
            {
                // Keep only the last 100 data points
                if (_dataPoints.Count >= 100)
                {
                    _dataPoints.RemoveAt(0);
                }
                
                _dataPoints.Add(data);
                return Task.FromResult(data);
            }
        }

        public Task<IEnumerable<MonitorData>> GetLatestDataAsync()
        {
            lock (_lock)
            {
                return Task.FromResult<IEnumerable<MonitorData>>(_dataPoints);
            }
        }
    }
}