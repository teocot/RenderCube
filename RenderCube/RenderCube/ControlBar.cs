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
        private Button prevButton = new Button();
        private Text Label;
        private Font Font;
        public ControlBar(string title, Font font)
        {
            this.SetMinSize(0, 48);
            this.VerticalAlignment = VerticalAlignment.Top;
            this.LayoutMode = LayoutMode.Horizontal;
            this.Font = font;
            prevButton.SetFixedSize(36, 36);

            Label = new Text()
            {
                Value = "<",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Label.SetFont(font, 20);

            prevButton.AddChild(Label);
            this.AddChild(prevButton);
            this.SetStyleAuto(null);

            prevButton.SetStyleAuto(null);
            //prevButton.SetStyle("LeftButton", null);
        }
        public Action OnNext;
        public Action<ReleasedEventArgs> OnPrevious { set { prevButton.Released += value; } }
        //Button CreateButton(int x, int y, int xSize, int ySize, string text)
        //{
        //    UIElement root = UI.Root;
        //    var cache = ResourceCache;
        //    Font font = cache.GetFont("Fonts/Font.ttf");

        //    // Create the button and center the text onto it
        //    Button button = new Button();
        //    root.AddChild(button);
        //    button.SetStyleAuto(null);
        //    button.SetPosition(x, y);
        //    button.SetSize(xSize, ySize);

        //    Text buttonText = new Text();
        //    button.AddChild(buttonText);
        //    buttonText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
        //    buttonText.SetFont(font, 20);
        //    buttonText.Value = text;

        //    return button;
        //}
        //public new bool SetStyleAuto(Urho.Resources.XmlFile file)
        //{
        //    bool ret = base.SetStyleAuto(file);
        //    Label.SetStyleAuto(file);
        //    prevButton.SetStyle("LeftButton", file);


        //    return ret;
        //}
    }
}
