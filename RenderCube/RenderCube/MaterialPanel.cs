using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Gui;
using Urho;
namespace RenderCube
{
    class MaterialPanel : Panel
    {
        Slider IORSlider;
        Slider TestSlider;
        Material material;
        public MaterialPanel(Context c, UIElement parent, Material m) : base(c)
        {
            material = m;

            parent.AddChild(this);
            this.SetStyleAuto(null);

            this.SetMinSize(400, 200);
            this.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            this.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
            this.Name = "MaterialPanel";
            //// Create Window 'titlebar' container
            //UIElement titleBar = new UIElement();
            //titleBar.SetMinSize(0, 24);
            //titleBar.VerticalAlignment = VerticalAlignment.Top;
            //titleBar.LayoutMode = LayoutMode.Horizontal;

            //// Create the Window title Text
            //var windowTitle = new Text();
            //windowTitle.Name = "WindowTitle";
            //windowTitle.Value = "Hello GUI!";

            //// Create the Window's close button
            //Button buttonClose = new Button();
            //buttonClose.Name = "CloseButton";

            //// Add the controls to the title bar
            //titleBar.AddChild(windowTitle);
            //titleBar.AddChild(buttonClose);

            //this.AddChild(titleBar);
            //windowTitle.SetStyleAuto(null);
            //buttonClose.SetStyle("CloseButton", null);

            //buttonClose.SubscribeToReleased(_ => Exit());

            IORSlider = this.CreateSlider(10, 10, 100, 20, "Index of Refraction");
            TestSlider = this.CreateSlider(0, 20, 100, 20, "TEST");
            IORSlider.SliderChanged += (args => material.SetShaderParameter("RefractIndex", args.Value));
            //IORSlider.SetStyleAuto(this.GetDefaultStyle(true));
            
            //this.SetStyleAuto(null);
        }
        public void SetIORValue(float value)
        {
            IORSlider.Value = value;
        }

    }
}
