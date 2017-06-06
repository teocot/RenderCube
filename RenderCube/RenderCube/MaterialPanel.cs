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

        MaterialHelper material;
        ResourceCache ResourceCache;
        //public MaterialPanel(UIElement parent, Material m, ResourceCache r) : base()
        //{
           
        //    this.material = new MaterialHelper(m);
            
        //    this.material.SetAll();
        //    this.ResourceCache = r;
        //    parent.AddChild(this);
            
        //    this.SetupPanel();
            

        //}
        public MaterialPanel(UIElement parent, MaterialHelper m, ResourceCache r) : base()
        {
            this.material = m;
            this.ResourceCache = r;
            parent.AddChild(this);
            this.material.SetAll();
            this.SetupPanel();
        }
        public void SetupPanel()
        {
            
            this.SetStyleAuto(null);

            this.SetMinSize(this.Parent.Size.X, 100);
            this.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            this.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
            this.Name = "MaterialPanel";

            if (material.isSet("RefractIndex"))
            {
                IORSlider = this.CreateSlider("Ior Ratio", 0.0f, 1.0f);
                                IORSlider.Value = 0.666f;
                IORSlider.SliderChanged +=
                    (args => material.RefractIndex = args.Value);

            }
            
            if (material.isSet("MatRefractColor"))
            {
                RefractSlider = this.CreateSlider("Refract", 0.0f, 1.5f);

                RefractSlider.SliderChanged +=
                    (args => material.MatRefractColor = Vector3.One * args.Value);

            }

            if (material.isSet("MatDiffColor"))
            {
                DiffSlider = this.CreateSlider("Texture", 0.0f, 1.5f);

                DiffSlider.SliderChanged +=
                    (args => material.MatDiffColor = Vector4.One * args.Value);
            }
            if (material.isSet("MatEnvMapColor"))
            {
                EnvSlider = this.CreateSlider("EnvMap", 0.0f, 1.5f);
                EnvSlider.SliderChanged +=
                    (args => material.MatEnvMapColor = Vector3.One * args.Value);
            }

        }

    }
}
