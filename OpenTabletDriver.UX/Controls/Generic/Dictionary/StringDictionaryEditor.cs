using System.Collections.Generic;
using Eto.Forms;

namespace OpenTabletDriver.UX.Controls.Generic.Dictionary
{
    public class StringDictionaryEditor : CustomItemList<KeyValuePair<string, string>>
    {
        protected override Control CreateControl(int index, DirectBinding<KeyValuePair<string, string>> itemBinding)
        {
            TextBox keyBox = new TextBox();
            keyBox.TextBinding.Bind(itemBinding.Child(i => i.Key));

            TextBox valueBox = new TextBox();
            valueBox.TextBinding.Bind(itemBinding.Child(i => i.Value));

            return new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Padding = 5,
                Items =
                {
                    keyBox,
                    new StackLayoutItem(valueBox, true)
                }
            };
        }
    }
}