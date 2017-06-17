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
    class ModelPanel : Panel
    {


        Node Node;
        ControlBar ControlBar;
        public Action<ReleasedEventArgs> OnPrevious { set { ControlBar.OnPrevious = value; } }
        ResourceCache ResourceCache;
        //public MaterialPanel(UIElement parent, Material m, ResourceCache r) : base()
        //{

        //    this.material = new MaterialHelper(m);

        //    this.material.SetAll();
        //    this.ResourceCache = r;
        //    parent.AddChild(this);

        //    this.SetupPanel();


        //}
        public ModelPanel(UIElement parent, Node n, ResourceCache r) : base()
        {

            this.ResourceCache = r;
            parent.AddChild(this);

            this.SetupPanel();
        }
        public void SetupPanel()
        {

            this.SetStyleAuto(null);
            this.ControlBar = new ControlBar("Model Properties");
            this.AddChild(this.ControlBar);
            this.ControlBar.SetStyleAuto(null);
            this.SetMinSize(this.Parent.Size.X, 100);
            this.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            this.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
            this.Name = "ModelPanel";
        }

    }
}
