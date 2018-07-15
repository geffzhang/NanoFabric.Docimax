using System.Threading.Tasks;

namespace NanoFabric.Docimax.Core.Orleans
{
    /// <summary>
	/// Odin Grain public contract interface. e.g. Grain interface should implement this.
	/// </summary>
	public interface IAppGrainContract
    {
        /// <summary>
        /// Cause force activation in order for grain to be warmed up/preloaded.
        /// </summary>
        /// <returns></returns>
        Task Activate();
    }

}
