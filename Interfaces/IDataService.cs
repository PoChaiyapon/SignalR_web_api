using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<MonitorData>> GetLatestDataAsync();
        Task<MonitorData> AddDataPointAsync(MonitorData data);
    }
}