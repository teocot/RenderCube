using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Gui;
using Urho;
using Urho.Resources;
namespace RenderCube
{
    class Panel : Window
    {
        protected ResourceCache ResourceCache;
        public Panel(ResourceCache Cache) : base()
        {
            ResourceCache = Cache;
        }
        protected RangeSlider CreateSlider( string label, Single minRange = 0.0f, Single maxRange = 1.0f )
        {
            RangeSlider slider = new RangeSlider(label, minRange, maxRange, 30);
            this.AddChild(slider);

            return slider;
        }
        Button CreateButton(int x, int y, int xSize, int ySize, string text)
        {
            
            // Create the button and center the text onto it
            Button button = new Button();
            this.AddChild(button);
            button.SetStyleAuto(null);
            button.SetPosition(x, y);
            button.SetSize(xSize, ySize);

            Text buttonText = new Text();
            button.AddChild(buttonText);
            buttonText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            buttonText.SetFontSize(20);
            buttonText.Value = text;

            return button;
        }
    }
}
