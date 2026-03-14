using System;
using System.Numerics;
using OpenTabletDriver.Attributes;
using OpenTabletDriver.Platform.Pointer;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Desktop.Binding
{
    [PluginName(PLUGIN_NAME)]
    public class ScrollBinding : IStateBinding, IPositionBinding
    {
        private const string PLUGIN_NAME = "Scroll Binding";

        private readonly IMouseButtonHandler _mouseButtonHandler;

        public ScrollBinding(IMouseButtonHandler mouseButtonHandler, ISettingsProvider settingsProvider)
        {
            _mouseButtonHandler = mouseButtonHandler;

            Console.WriteLine("init");
            settingsProvider.Inject(this);
        }

        [Setting(nameof(Vertical))]
        public int Vertical { set; get; }

        [Setting(nameof(Horizontal), "The amount of ticks horizontally to scroll.")]
        public int Horizontal { set; get; }

        public void Press(IDeviceReport report)
        {
            Console.WriteLine("PRESS");
            _mouseButtonHandler.Scroll(new Vector2(Horizontal, Vertical));
        }

        public void Release(IDeviceReport report)
        {
            // This is a
        }

        public void Move(IDeviceReport report)
        {
            Console.WriteLine("MOVE");
        }
    }
}
