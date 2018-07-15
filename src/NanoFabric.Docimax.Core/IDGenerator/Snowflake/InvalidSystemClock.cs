using NanoFabric.Docimax.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Core.IDGenerator.Snowflake
{
    public class InvalidSystemClock : DociException
    {
        public InvalidSystemClock(string message) 
            : base(message)
        {
        }
    }
}
