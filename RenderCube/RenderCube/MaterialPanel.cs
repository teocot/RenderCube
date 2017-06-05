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
        RangeSlider TestSlider;
        Material material;
        ResourceCache ResourceCache;
        public MaterialPanel( UIElement parent, Material m, ResourceCache r) : base()
        {
            material = m;
            this.ResourceCache = r;
            parent.AddChild(this);
            this.SetStyleAuto(null);

            this.SetMinSize(parent.Size.X, 100);
            this.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            this.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
            this.Name = "MaterialPanel";

            string iorvalue = GetShaderParameter(this.material, "RefractIndex");
            if (iorvalue != "")
            {
                IORSlider = this.CreateSlider("Ior Ratio", 0.0f, 1.0f);
                IORSlider.SliderChanged +=
                    (args => material.SetShaderParameter("RefractIndex", args.Value));
                IORSlider.Value = Single.Parse(iorvalue);
            }
            //TestSlider = this.CreateSlider(0, 20, 100, 20, "TEST", (args => { }));
 
            //IORSlider.SetStyleAuto(this.GetDefaultStyle(true));
            
            //this.SetStyleAuto(null);
        }
        public void SetIORValue(float value)
        {
            IORSlider.Value = value;
        }

        //Unfortunately urhosharp does not support getting the shader parameters directly yet, so we do it the hard way.
        public string GetShaderParameter(Material m, string name)
        {
            //Urho.IO.Log.WriteRaw("name:" + m.Name +"\n", true);
            XmlFile file = ResourceCache.GetXmlFile(m.Name);

            string value = "";
            if (file != null)
            {
                XmlElement e = file.GetRoot("material");

                int i = 0;
                for (XmlElement element = e.GetChild("parameter"); element != null; element = element.GetNext("parameter"))
                {
                    Urho.IO.Log.WriteRaw("parameter " + element.GetAttribute("name"), true);

                    if (element.GetAttribute("name") == "RefractIndex")
                    {
                        value = element.GetAttribute("value");
                        break;
                    }
                }
            }
            return value;
        }

    }
}
