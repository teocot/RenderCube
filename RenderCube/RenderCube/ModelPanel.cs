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


        Node SceneNode;
        ControlBar ControlBar;
        public Action<ReleasedEventArgs> OnPrevious { set { ControlBar.OnPrevious = value; } }
        ResourceCache ResourceCache;
        ListView ModelListView;
        Dictionary<string, Model> models;

        public ModelPanel(UIElement parent, Node scene, ResourceCache r, Dictionary<string,Model> models = null) : base()
        {

            this.ResourceCache = r;
            parent.AddChild(this);
            this.SceneNode = scene;
            this.models = models;
            this.SetupPanel();
        }
        public void SetupPanel()
        {

            this.SetStyleAuto(null);
            this.ControlBar = new ControlBar("Model Properties", ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"));
            this.AddChild(this.ControlBar);
            this.ControlBar.SetStyleAuto(null);
            this.SetMinSize(this.Parent.Size.X, 100);
            this.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            this.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
            this.ModelListView = new ListView();
            this.RefreshList();
            ModelListView.SetStyleAuto(null);
            this.AddChild(ModelListView);

            this.Name = "ModelPanel";
        }
        public void RefreshList()
        {
            //ModelListView.RemoveAllChildren();
            foreach (Node node in SceneNode.Children)
            {
                Text modelselect = new Text();
                modelselect.Value = node.Name;
                ModelListView.AddItem(modelselect);
            }
            Text empty = new Text();
            empty.SetStyleAuto(null);
            empty.Value = "Empty";
            ModelListView.AddItem(empty);
        }

    }
}
