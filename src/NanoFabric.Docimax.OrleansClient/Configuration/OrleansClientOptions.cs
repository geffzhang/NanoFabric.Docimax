using System;
using System.Collections.Generic;

namespace NanoFabric.Docimax.OrleansClient
{
    public class OrleansClientOptions
    {
        public OrleansClientOptions()
        {
            this.StaticGatewayList = new List<Uri>();
        }

        /// <summary>
        /// Client Name
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// Cluster ID
        /// </summary>
        public string ClusterId { get; set; }

        /// <summary>
        /// whether is local host
        /// </summary>
        public bool IsLocalHost { get; set; } = false;

        /// <summary>
        /// client gateway list
        /// </summary>
        public List<Uri> StaticGatewayList { get; set; }
 

        /// <summary>
        /// Consul address
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}