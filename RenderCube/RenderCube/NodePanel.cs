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
    class NodePanel : Panel
    {

        Node SceneNode;
        public ControlBar ControlBar;
       
        public VectorField PositionField;
        public QuaternionField RotationField;
        public RangeSlider ScaleSlider;
        Node Node;

        public NodePanel(UIElement Parent, Node Node, ResourceCache Cache) : base(Cache)
        {
            Parent.AddChild(this);
            this.Node = Node;
            this.SetupPanel();
        }
        public void SetupPanel()
        {

            this.SetStyleAuto(null);
            this.ControlBar = new ControlBar("Model Properties", ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"));
            this.AddChild(this.ControlBar);
            this.ControlBar.SetStyleAuto(null);
            this.SetMinSize(this.Parent.Size.X, 150);
            this.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            this.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);


            PositionField = new VectorField(Node.Position, "PosX", "PosY", "PosZ");
            PositionField.VerticalAlignment = VerticalAlignment.Top;
            this.AddChild(PositionField);
            PositionField.SetStyleAuto(null);
            RotationField = new QuaternionField(Node.Rotation, "RotX", "RotY", "RotZ");
            RotationField.VerticalAlignment = VerticalAlignment.Top;
            this.AddChild(RotationField);
            RotationField.SetStyleAuto(null);

            ScaleSlider = new RangeSlider("Scale", 0.01f, 5.0f);
            ScaleSlider.SetStyleAuto(null);
            this.AddChild(ScaleSlider);
            this.Name = "ModelPanel";
        }
    }
}
