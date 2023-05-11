using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCase_WPF_RSS.Settings
{
    internal class ConnectionStringSettings
    {
        public ConnectionStringSettings(string connectionString) { }
        public ConnectionStringSettings() { }
        public string? ConnectionString { get; set; }
    }
}
