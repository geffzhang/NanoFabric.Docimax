using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Core.IDGenerator
{
    public interface IWorkerOpation
    {
        /// <summary>
        /// Get worker machine
        /// </summary>
        /// <returns></returns>
        long GetWorkerId();

        /// <summary>
        /// Get datacenter ID
        /// </summary>
        /// <returns></returns>
        long GetDatacenterId();
    }
}
