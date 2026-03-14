using OpenTabletDriver.Desktop.Reflection;
using OpenTabletDriver.Platform.Pointer;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Desktop.Binding
{
    public class BindingPosition
    {
        private readonly IBinding _binding;

        public BindingPosition(IPluginFactory pluginFactory, InputDevice device, IMouseButtonHandler mouseButtonHandler, PluginSettings settings)
        {
            _binding = pluginFactory.Construct<IBinding>(settings, device, mouseButtonHandler)!;
        }

        public void Invoke(IDeviceReport report)
        {
            if (_binding is IPositionBinding positionBinding)
            {
                positionBinding.Move(report);
            }

        }
    }
}

