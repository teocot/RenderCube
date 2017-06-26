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
        private Button prevButton;
        private Button nextButton;
        private Text Label;
        private Font Font;
        public ControlBar(string title, Font font)
        {
            this.SetMinSize(0, 48);
            this.VerticalAlignment = VerticalAlignment.Top;

            this.SetLayout(LayoutMode.Horizontal,10, new IntRect(0, 0, 0, 0));
            this.SetStyleAuto(null);
            
            this.LayoutFlexScale = new Vector2(0, 0);
            this.Font = font;

            prevButton = CreateButton("<");
            var OneButton = CreateButton("One");
            OneButton.HorizontalAlignment = HorizontalAlignment.Center;
            OneButton.SetFixedWidth(50);
            nextButton = CreateButton(">");
            nextButton.HorizontalAlignment = HorizontalAlignment.Right;
            this.AddChild(prevButton);
            this.AddChild(new Urho.Gui.UIElement()); // spacer
            this.AddChild(OneButton);
            this.AddChild(new Urho.Gui.UIElement()); // spacer
            this.AddChild(nextButton);

            //prevButton.SetStyle("LeftButton", null);
        }
        public Action<ReleasedEventArgs> OnPrevious { set { prevButton.Released += value; } }
        public Action<ReleasedEventArgs> OnNext { set { nextButton.Released += value; } }
        Button CreateButton(string text)
        {
            var button = new Button();
            // Create the button and center the text onto it
            button.SetFixedSize(36, 36);

            Label = new Text()
            {
                Value = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Label.SetFont(Font, 20);

            button.AddChild(Label);


            button.SetStyleAuto(null);

            return button;
        }
        //public new bool SetStyleAuto(Urho.Resources.XmlFile file)
        //{
        //    bool ret = base.SetStyleAuto(file);
        //    Label.SetStyleAuto(file);
        //    prevButton.SetStyle("LeftButton", file);


        //    return ret;
        //}
    }
}
