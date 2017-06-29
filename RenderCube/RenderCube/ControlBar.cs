using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Gui;
using Urho;

namespace RenderCube
{
    class ControlBar : UIElement
    {
        private Button PrevButton;
        private Button NextButton;
        private Button SuzanneButton;
        private Button TeapotButton;
        private Button CubeButton;

        private Text Label;
        private Font Font;

        public Action<ReleasedEventArgs> OnPrevious { set { PrevButton.Released += value; } }
        public Action<ReleasedEventArgs> OnNext { set { NextButton.Released += value; } }
        public Action<ReleasedEventArgs> OnSuzanne { set { SuzanneButton.Released += value; } }
        public Action<ReleasedEventArgs> OnCube { set { CubeButton.Released += value; } }
        public Action<ReleasedEventArgs> OnTeapot { set { TeapotButton.Released += value; } }
        public ControlBar(string title, Font font)
        {
            this.SetMinSize(0, 48);
            this.VerticalAlignment = VerticalAlignment.Top;

            this.SetLayout(LayoutMode.Horizontal,10, new IntRect(0, 0, 0, 0));
            this.SetStyleAuto(null);
            
            this.LayoutFlexScale = new Vector2(0, 0);
            this.Font = font;

            PrevButton = CreateButton("<", HorizontalAlignment.Left);
            SuzanneButton = CreateButton("Suzanne", HorizontalAlignment.Center);
            TeapotButton = CreateButton("Teapot", HorizontalAlignment.Center);
            CubeButton = CreateButton("Cube", HorizontalAlignment.Center);
            NextButton = CreateButton(">", HorizontalAlignment.Right);


            this.AddChild(PrevButton);
            this.AddChild(new Spacer()); 
            this.AddChild(SuzanneButton);
            this.AddChild(new Spacer(12));
            this.AddChild(CubeButton);
            this.AddChild(new Spacer(12));
            this.AddChild(TeapotButton);
            this.AddChild(new Spacer()); 
            this.AddChild(NextButton);

            //prevButton.SetStyle("LeftButton", null);  // Couldn't get this to work--Used "<" instead. This would be ideal.
        }

        Button CreateButton(string text, HorizontalAlignment align = HorizontalAlignment.Left)
        {
            var Button = new Button();

            // Create the button and center the text onto it
            Button.SetFixedSize(36, 36);
            Label = new Text()
            {
                Value = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Label.SetFont(Font, 20);

            Button.AddChild(Label);

            Button.SetFixedWidth(Button.GetChild(0).Width + 16);
            Button.SetStyleAuto(null);
            Button.HorizontalAlignment = align;
            return Button;
        }
    }
}
