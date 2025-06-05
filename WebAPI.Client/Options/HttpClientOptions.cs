using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Client.Options
{
    public class HttpClientOptions
    {
        public string BaseAddress { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public int Timeout { get; set; } = 100;
        public bool EnableLogging { get; set; } = false;
        public bool EnableCaching { get; set; } = false;
        public bool EnableCompression { get; set; } = false;
        public bool EnableRetryPolicy { get; set; } = false;
        public int MaxRetryAttempts { get; set; } = 3;

    }
}
