using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Settings.Interface
{
    public interface ISourcingDatabaseSettings
    {
        public string ConnectionStrings { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
