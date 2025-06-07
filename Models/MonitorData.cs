using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class MonitorData
    {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public string MetricName { get; set; }
        public string Status { get; set; }
    }
}