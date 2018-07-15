using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Core.IDGenerator
{
    public class WorkerOpation : IWorkerOpation
    {
        public long GetDatacenterId()
        {
            return 1;
        }

        public long GetWorkerId()
        {
            return 1;
        }
    }
}
