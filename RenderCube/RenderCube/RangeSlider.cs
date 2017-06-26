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
    public struct RangeSliderEventArgs
    {
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
        public LineEdit ValueEditor;
        public Action<RangeSliderEventArgs> SliderChanged = (args => { });

        //map the Slider range [0.0,1.0] to the custom range [minRange, maxRange] 
        public Single Value
        {
            get { return (Slider.Value * (maxRange - minRange)) + minRange; }
            set { Slider.Value = ((value - minRange) / (maxRange - minRange)); }
        }

        Single minRange;
        Single maxRange;
        public Single Clamp(Single value)
        {
            return MathHelper.Clamp(value, this.minRange, this.maxRange);
        }
        public RangeSlider(string label, Single minRange, Single maxRange, int height = 35) : base()
        {
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.SetLayout(LayoutMode.Horizontal, 10, new IntRect(0, 0, 0, 0));
            this.SetStyleAuto(null);
            this.SetFixedHeight(height);
            Label = new Text();
            Label.Value = label;
            this.AddChild(Label);

            //set the label to adjust size depending on the length of the text
            Label.LayoutFlexScale = new Vector2(0, 0);
            Label.SetMinSize(0, 0);
            Label.SetFixedWidth(10 * label.Length);
            Label.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Center);
            Slider = new Slider();
            this.AddChild(Slider);

            //slider will respect height, but stretch horizontally
            Slider.SetMaxSize(100000, height);

            ValueEditor = new LineEdit();
            ValueEditor.SetStyleAuto(null);
            this.AddChild(ValueEditor);
            ValueEditor.SetMaxSize(100, height);
            ValueEditor.SetFixedWidth(10 * 10);
            ValueEditor.VerticalAlignment = VerticalAlignment.Center;
            //ValueEditor. = new IntVector2(20,height - ValueEditor.TextElement.Height/2);
            //
            ValueEditor.TextElement.VerticalAlignment = VerticalAlignment.Center;
            ValueEditor.TextElement.SetStyleAuto(null);
            //when the slider changes, set the value display and call the action function;
            Slider.SliderChanged += (args =>
            {
                ValueEditor.Text = " "+this.Value.ToString();
                var newargs = new RangeSliderEventArgs(args.Element, this.Value);
                this.SliderChanged(newargs);
            });

            ValueEditor.TextFinished += (args =>
            {
                Single value;
                if (Single.TryParse(args.Text, out value))
                {

                        this.Value = this.Clamp(value);
                        //ValueEditor.Text = Slider.Value.ToString();
                    
                    //ValueEditor.Text = value.ToString();
                }
                //ValueEditor.Text = Slider.Value.ToString();
            });
            //ValueEditor.Focused += (args => ValueEditor.Selected=true);
            
            Label.SetStyleAuto(null);
            Slider.SetStyleAuto(null);
            
        }
    }
}
