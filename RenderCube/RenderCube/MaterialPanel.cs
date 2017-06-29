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
    class MaterialPanel : Panel
    {
        RangeSlider IORSlider;
        RangeSlider DiffSlider;
        RangeSlider EnvSlider;
        RangeSlider RefractSlider;
        public ControlBar ControlBar;
        MaterialHelper material;



        public MaterialPanel(UIElement parent, MaterialHelper mh, ResourceCache r) : base(r)
        {
            this.material = mh;
            parent.AddChild(this);
            this.material.SetAll();
            this.Name = "MaterialPanel";
            this.SetupPanel();
        }

        public void SetMaterial(MaterialHelper m)
        {
            this.material = m;

            this.material.SetAll(); //this will eventually use the xml parser;
            this.RemoveAllChildren();
            this.SetupPanel();

        }
        public void SetupPanel()
        {
            
            this.SetStyleAuto(null);

            this.SetMinSize(this.Parent.Size.X, 100);
            this.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            this.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
            this.ControlBar = new ControlBar("Material Properties", ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"));
            

            this.AddChild(this.ControlBar);
            
            this.ControlBar.SetStyleAuto(null);
            //this.ControlBar.SetFixedWidth(this.Width);
            if (material.isSet("RefractIndex"))
            {
                IORSlider = this.CreateSlider("Ior Ratio", 0.0f, 1.0f);
                                IORSlider.Value = material.RefractIndex;
                IORSlider.SliderChanged +=
                    (args => material.RefractIndex = args.Value);

            }
            
            if (material.isSet("MatRefractColor"))
            {
                RefractSlider = this.CreateSlider("Refract", 0.0f, 1.5f);
                RefractSlider.Value = material.MatRefractColor.X;
                RefractSlider.SliderChanged +=
                    (args => material.MatRefractColor = Vector3.One * args.Value);

            }

            if (material.isSet("MatDiffColor"))
            {
                DiffSlider = this.CreateSlider("Texture", 0.0f, 1.5f);
                DiffSlider.Value = material.MatDiffColor.X;
                DiffSlider.SliderChanged +=
                    (args => material.MatDiffColor = Vector4.One * args.Value);
            }
            if (material.isSet("MatEnvMapColor"))
            {
                EnvSlider = this.CreateSlider("EnvMap", 0.0f, 1.5f);
                EnvSlider.Value = material.MatEnvMapColor.X;
                EnvSlider.SliderChanged +=
                    (args => material.MatEnvMapColor = Vector3.One * args.Value);
            }

        }

    }
}
