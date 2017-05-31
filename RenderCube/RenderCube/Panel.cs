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
        public Panel(Context c) : base(c)
        {
            //this.cache = r;
        }
        protected Slider CreateSlider(int x, int y, int xSize, int ySize, string text)
        {

            //Font font = cache.GetFont("Fonts/Font.ttf");
            //// Create text and slider below it
            Text sliderText = new Text();
            this.AddChild(sliderText);
            sliderText.SetPosition(x, y);
            //sliderText.SetFont(font, 12);
            sliderText.Value = text;

            Slider slider = new Slider();
            this.AddChild(slider);
            //slider.SetStyleAuto(null);
            slider.SetPosition(x, y + 20);
            slider.SetSize(xSize, ySize);
            // Use 0-1 range for controlling sound/music master volume
            slider.Range = 1.0f;

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
    }
}
