using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Core.Orleans
{
    /// <summary>
	/// Odin Grain implementation interface. e.g. Grain concrete should implement this.
	/// </summary>
	public interface IAppGrain : IAppGrainContract
    {
        /// <summary>
        /// Gets the primary key for the grain as string (independent of its original type).
        /// </summary>
        string PrimaryKey { get; }

        /// <summary>
        /// Gets the source type name e.g. 'AppConfigGrain'.
        /// </summary>
        string Source { get; }
    }
}
