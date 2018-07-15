using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NanoFabric.Docimax.Core.Exceptions
{
    public class DociException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="DociException"/> object.
        /// </summary>
        public DociException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="DociException"/> object.
        /// </summary>
        public DociException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="DociException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public DociException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="DociException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DociException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
