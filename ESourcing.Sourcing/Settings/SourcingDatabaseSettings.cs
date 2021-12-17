using ESourcing.Sourcing.Settings.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Settings
{
    public class SourcingDatabaseSettings : ISourcingDatabaseSettings
    {
        public string ConnectionStrings { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
