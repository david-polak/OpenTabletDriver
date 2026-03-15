
using System.Numerics;
using OpenTabletDriver.Plugin.Tablet;

namespace OpenTabletDriver.Plugin
{
    public interface IStatePositionBinding : IStateBinding
    {
        public void SetPosition(Vector2 pos);
    }
}
