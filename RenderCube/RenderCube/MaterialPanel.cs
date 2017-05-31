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
        Material material;
        public MaterialPanel(Context c, Material m) : base(c)
        {
            material = m;
            IORSlider = this.CreateSlider(0, 0, 100, 20, "IOR");
            IORSlider.SliderChanged += (args => material.SetShaderParameter("RefractIndex", args.Value));
            IORSlider.Value = 0.66f;
            //IORSlider.SetStyleAuto(this.GetDefaultStyle(true));
        }
        public new bool SetStyleAuto(Urho.Resources.XmlFile file)
        {
            return IORSlider.SetStyleAuto(file) && base.SetStyleAuto(file);
        }
    }
}
