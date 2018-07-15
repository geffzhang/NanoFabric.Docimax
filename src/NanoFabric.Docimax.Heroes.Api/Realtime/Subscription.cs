using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Heroes.Api.Realtime
{
    public class Subscription<T>
    {
        public StreamSubscriptionHandle<T> Stream { get; set; }
        public Subject<T> Subject { get; set; }
    }
}
