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

            ////Font font = cache.GetFont("Fonts/Font.ttf");
            ////// Create text and slider below it
            //UIElement labelslider = new UIElement();
            //labelslider.SetLayout(LayoutMode.Horizontal, 6, new IntRect ( 0,0,0,0));
            //labelslider.SetStyleAuto(null);
            //Text sliderLabel = new Text();
            //labelslider.AddChild(sliderLabel);
            //sliderLabel.SetPosition(x, y);
            //sliderLabel.LayoutFlexScale = new Vector2(0,0);
            //sliderLabel.Value = text;
            //sliderLabel.SetMinSize(0, 0);
            ////sliderText.SetFixedWidth(width);

            //sliderLabel.SetStyleAuto(null);
            //Slider slider = new Slider();
            //labelslider.AddChild(slider);

            //slider.SetPosition(x, y + 20);
            //slider.SetSize(xSize, ySize);
            //// Use 0-1 range for controlling sound/music master volume
            //slider.Range = 1.0f;
            //slider.SetMaxSize(500, ySize);
            //slider.SetStyleAuto(null);

            //Text slidertext = new Text();
            //slidertext.Value = slider.Value.ToString();
            //labelslider.AddChild(slidertext);
            //slidertext.SetStyleAuto(null);

            //slider.SliderChanged += (args => { slidertext.Value = args.Value.ToString(); action(args); });

            ////labelslider.SetEnabledRecursive(true);
            //labelslider.SetStyleAuto(null);
            //this.AddChild(labelslider);
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
