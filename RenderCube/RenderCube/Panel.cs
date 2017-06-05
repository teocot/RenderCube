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
        //private ResourceCache cache;
        public Panel() : base()
        {
            //this.cache = r;
        }
        protected RangeSlider CreateSlider( string label, Single minRange = 0.0f, Single maxRange = 1.0f )
        {
            RangeSlider slider = new RangeSlider(label, minRange, maxRange);
            this.AddChild(slider);

            return slider;
        }
        Button CreateButton(int x, int y, int xSize, int ySize, string text)
        {
            
            //Font font = cache.GetFont("Fonts/Font.ttf");

            // Create the button and center the text onto it
            Button button = new Button();
            this.AddChild(button);
            button.SetStyleAuto(null);
            button.SetPosition(x, y);
            button.SetSize(xSize, ySize);

            Text buttonText = new Text();
            button.AddChild(buttonText);
            buttonText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            //buttonText.SetFont(font, 20);
            buttonText.Value = text;

            return button;
        }
        //public new bool SetStyleAuto(Urho.Resources.XmlFile file)
        //{
        //    bool ret = base.SetStyleAuto(file);
        //    foreach (UIElement e in this.Children)
        //    {
        //        ret = ret && e.SetStyleAuto(file);
        //    }
        //    return ret;
        //}
    }
}
