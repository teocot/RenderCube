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
    public struct RangeSliderEventArgs {
        public RangeSliderEventArgs(UIElement e, Single v)
        {
            this.Element = e;
            this.Value = v;
        }

        public UIElement Element;
        public Single Value;
    };

    class RangeSlider : UIElement
    {
        public Text Label;
        public Slider Slider;
        public Text DisplayText;
        public Action<RangeSliderEventArgs> SliderChanged = (args => { });

        //map the Slider range [0.0,1.0] to the custom range [minRange, maxRange] 
        public float Value { get { return (Slider.Value * (maxRange - minRange)) + minRange; }
                             set { Slider.Value = ((value - minRange) / (maxRange - minRange)); } }

        Single minRange;
        Single maxRange;
        public RangeSlider(string label, Single minRange, Single maxRange, int height = 30) : base()
        {
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.SetLayout(LayoutMode.Horizontal, 10, new IntRect(0, 0, 0, 0));


            Label = new Text();
            Label.Value = label;
            this.AddChild(Label);

            //set the label to adjust size depending on the length of the text
            Label.LayoutFlexScale = new Vector2(0, 0);
            Label.SetMinSize(0, 0);

            Slider = new Slider();
            this.AddChild(Slider);

            //slider will respect height, but stretch horizontally
            Slider.SetMaxSize(100000, height);

            DisplayText = new Text();
            this.AddChild(DisplayText);
            DisplayText.SetMaxSize(80, 20);
            //when the slider changes, set the value display and call the action function;
            Slider.SliderChanged += (args => {
                DisplayText.Value = this.Value.ToString();
                var newargs = new RangeSliderEventArgs(args.Element,this.Value);
                this.SliderChanged(newargs);
                });

            this.SetStyleAuto(null);
            Label.SetStyleAuto(null);
            Slider.SetStyleAuto(null);
            DisplayText.SetStyleAuto(null);
        }
    }
}
