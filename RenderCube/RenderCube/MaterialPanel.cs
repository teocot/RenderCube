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
        Vector4 CurrentDiffuse;
        Vector3 CurrentEnv;
        Vector3 CurrentRefract;
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
            string refractvalue = GetShaderParameter(this.material, "RefractColor");
            if (refractvalue != "")
            {
                RefractSlider = this.CreateSlider("Refract", 0.0f, 1.0f);
                CurrentRefract = new Vector3(1.0f, 1.0f, 1.0f);
                RefractSlider.SliderChanged +=
                    (args => material.SetShaderParameter("RefractColor", CurrentRefract * args.Value));
            }
            string diffvalue = GetShaderParameter(this.material, "MatDiffColor");
            if (diffvalue != "")
            {
                DiffSlider = this.CreateSlider("Texture", 0.0f, 1.0f);
                CurrentDiffuse = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                DiffSlider.SliderChanged +=
                    (args => material.SetShaderParameter("MatDiffColor", CurrentDiffuse * args.Value));
            }
            string envvalue = GetShaderParameter(this.material, "MatEnvMapColor");
            if (envvalue != "")
            {
                EnvSlider = this.CreateSlider("EnvMap", 0.0f, 1.0f);
                CurrentEnv = new Vector3(1.0f, 1.0f, 1.0f);
                EnvSlider.SliderChanged +=
                    (args => material.SetShaderParameter("MatEnvMapColor", CurrentEnv * args.Value));
            }
            //IORSlider.SetStyleAuto(this.GetDefaultStyle(true));

            //this.SetStyleAuto(null);
        }
        public void SetIORValue(float value)
        {
            IORSlider.Value = value;
        }
        public Vector4 ParseVector4(string value)
        {
            char[] delim = { ' ' };
            string[] values = value.Split(delim);
            Vector4 four = new Vector4(Single.Parse(values[0]), Single.Parse(values[1]), Single.Parse(values[2]), Single.Parse(values[3]));
            Urho.IO.Log.WriteRaw("parsed vector4:" + four.ToString(), true);
            return four;
        }
        public Vector3 ParseVector3(string value)
        {
            char[] delim = { ' ' };
            string[] values = value.Split(delim);
            Vector3 three = new Vector3 (Single.Parse(values[0]), Single.Parse(values[1]), Single.Parse(values[2]));
            Urho.IO.Log.WriteRaw("parsed vector3:" + three.ToString(), true);
            return three;
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

                    if (element.GetAttribute("name") == name)
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
