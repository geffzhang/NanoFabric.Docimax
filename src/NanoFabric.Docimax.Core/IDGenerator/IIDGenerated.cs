using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Core.IDGenerator
{
    public interface IIDGenerated
    {
        /// <summary>
        /// Generat ID
        /// </summary>
        /// <returns></returns>
        Task<long> NextIdAsync();

        /// <summary>
        /// Generat ID
        /// </summary>
        /// <returns></returns>
        long NextId();
    }
}
