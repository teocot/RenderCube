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
            UIElement labelslider = new UIElement();
            labelslider.LayoutMode = LayoutMode.Horizontal;
            labelslider.SetStyleAuto(null);
            Text sliderText = new Text();
            labelslider.AddChild(sliderText);
            sliderText.SetPosition(x, y);
            sliderText.LayoutFlexScale = new Vector2(0,0);
                //sliderText.SetFont(font, 12);
            sliderText.Value = text;

            //int width = 0;
            //for(uint i = 0; i < text.Length; i++)
            //    width += sliderText.GetCharSize(i).X;
            sliderText.SetMinSize(0, 0);
            //sliderText.SetFixedWidth(width);

            sliderText.SetStyleAuto(null);
            Slider slider = new Slider();
            labelslider.AddChild(slider);
            
            slider.SetPosition(x, y + 20);
            slider.SetSize(xSize, ySize);
            // Use 0-1 range for controlling sound/music master volume
            slider.Range = 1.0f;
            slider.SetMaxSize(500, ySize);
            slider.SetStyleAuto(null);

            //labelslider.SetEnabledRecursive(true);
            labelslider.SetStyleAuto(null);
            this.AddChild(labelslider);
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
