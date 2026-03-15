using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.DependencyInjection;
using OpenTabletDriver.Plugin.Platform.Pointer;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Timers;

namespace OpenTabletDriver.Desktop.Binding
{
    [PluginName(PLUGIN_NAME)]
    public class MouseScrollBinding : IStatePositionBinding
    {
        private const string PLUGIN_NAME = "Mouse Scroll Binding";

        private ScrollDirection _direction;

        private ITimer timer;
        private float _interval = 1000f / 60;
        private int _refresh_rate = 60;

        private Vector2 initial_position;
        private int scroll_amount_horizontal = 0;
        private int scroll_amount_vertical = 0;

        [Resolved]
        public IMouseScrollHandler Pointer { set; get; }

        [Resolved]
        public ITimer Timer
        {
            get => timer;
            set
            {
                if (timer != null)
                {
                    timer.Elapsed -= Scroll;
                    if (timer.Enabled) timer.Stop();
                }
                timer = value;
                timer.Elapsed += Scroll;
            }
        }

        [Property("Direction"), DefaultPropertyValue("Both"), PropertyValidated(nameof(ValidDirections))]
        public string Direction
        {
            get => _direction.ToString();
            set
            {
                if (Enum.TryParse(value, out ScrollDirection direction))
                    _direction = direction;
                else
                {
                    Log.Write("MouseScrollBinding", $"Invalid scroll direction '{value}', defaulting to 'Both'", LogLevel.Warning);
                    _direction = ScrollDirection.Both;
                }
            }
        }

        [Property("Sensitivity"),
         DefaultPropertyValue(50f),
         ToolTip("The sensitivity of scrolling with pen movement.")]
        public float Sensitivity { get; set; }

        [SliderProperty("Refresh rate", 1f, 320f, 60f),
         DefaultPropertyValue(60f),
         ToolTip("How often scrolling event gets sent (lower for better performance, higher for smoother scrolling).")]
        public float RefreshRate
        {
            get => (float) _refresh_rate;
            set
            {
                _refresh_rate = (int) Math.Round(value);
                _interval = 1000f / _refresh_rate;
            }
        }

        [BooleanPropertyAttribute("Invert horizontal", "Invert horizontal direction"), DefaultPropertyValue(false)]
        public bool InvertHorizontal { get; set; }

        [BooleanPropertyAttribute("Invert vertical", "Invert vertical direction"), DefaultPropertyValue(false)]
        public bool InvertVertical { get; set; }

        public void Press(TabletReference tablet, IDeviceReport report)
        {
            if (this.timer == null)
                throw new InvalidOperationException($"{nameof(this.Timer)} was not injected by daemon");

            if (report is IAbsolutePositionReport absolutePositionReport)
            {
                initial_position = absolutePositionReport.Position;
                scroll_amount_horizontal = 0;
                scroll_amount_vertical = 0;
                timer.Interval = this._interval;
                timer.Start();
            }
            else
            {
                throw new InvalidOperationException("MouseScrollBinding not supported on this device");
            }
        }

        public void Release(TabletReference tablet, IDeviceReport report) => timer?.Stop();

        public void SetPosition(Vector2 pos)
        {
            if (!timer.Enabled) return;
            scroll_amount_horizontal = (int) Math.Ceiling((pos.X - initial_position.X) * (Sensitivity / 100));
            scroll_amount_vertical = (int) Math.Ceiling((initial_position.Y - pos.Y) * (Sensitivity / 100));
        }

        public void Scroll()
        {
            if (_direction == ScrollDirection.Horizontal || _direction == ScrollDirection.Both)
                Pointer.ScrollHorizontally(InvertHorizontal ? -scroll_amount_horizontal : scroll_amount_horizontal);

            if (_direction == ScrollDirection.Vertical || _direction == ScrollDirection.Both)
                Pointer.ScrollVertically(InvertVertical ? -scroll_amount_vertical : scroll_amount_vertical);

            if (Pointer is ISynchronousPointer synchronousPointer)
                synchronousPointer.Flush();
        }

        private static IEnumerable<string> validDirections;
        public static IEnumerable<string> ValidDirections =>
            validDirections ??= Enum.GetValues<ScrollDirection>().Select(Enum.GetName);

        public override string ToString()
        {
            if (_direction != ScrollDirection.Both)
                return $"{PLUGIN_NAME}";
            return $"{PLUGIN_NAME} ({Direction})";
        }
    }
}
