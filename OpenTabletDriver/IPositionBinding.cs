using JetBrains.Annotations;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver
{
    /// <summary>
    /// A binding with a boolean state.
    /// </summary>
    [PublicAPI]
    public interface IPositionBinding : IBinding
    {
        /// <summary>
        /// The method to perform when the binding is being activated.
        /// </summary>
        /// <param name="report">The report that triggered the press.</param>
        void Move(IDeviceReport report);
    }
}
