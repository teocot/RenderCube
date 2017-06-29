using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Resources;

namespace RenderCube
{

    class MaterialHelper : Material
    {


        private Vector4 _MatDiffColor;
        public Vector4 MatDiffColor {
            get { return this._MatDiffColor; }
            set { this.SetShaderParameter("MatDiffColor", value); _MatDiffColor = value; SetParameters["MatDiffuseColor"] = true; }
        }
        private Vector3 _MatEnvMapColor;
        public Vector3 MatEnvMapColor
        {
            get { return this._MatEnvMapColor; }
            set { this.SetShaderParameter("MatEnvMapColor", value); _MatEnvMapColor = value; SetParameters["MatEnvMapColor"] = true; }
        }
        private Vector3 _MatRefractColor;
        public Vector3 MatRefractColor
        {
            get { return this._MatRefractColor; }
            set { this.SetShaderParameter("MatRefractColor", value); _MatRefractColor = value; SetParameters["MatRefractColor"] = true; }
        }
        private Single _RefractIndex;
        public Single RefractIndex
        {
            get { return this._RefractIndex; }
            set { this.SetShaderParameter("RefractIndex", value); _RefractIndex = value; SetParameters["RefractIndex"] = true; }
        }

        Dictionary<string, bool> SetParameters = new Dictionary<string, bool>{
            { "MatDiffColor", false },
            { "MatEnvMapColor", false },
            {"MatRefractColor", false},
            {"RefractIndex", false }
            };

        public static readonly Vector4 Vector4NaN = new Vector4(Single.NaN, Single.NaN, Single.NaN, Single.NaN);


        public static readonly Vector3 Vector3NaN = new Vector3(Single.NaN, Single.NaN, Single.NaN);

        

        public MaterialHelper(XmlFile file)
        {
            this.Load(file.GetRoot("material"));
        }

        public bool isSet(string key)
        {
            return SetParameters[key];
        }
        public void SetAll(bool value = true)
        {
            foreach(var key in SetParameters.Keys.ToList()) {
                SetParameters[key] = value;
            }
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
            Vector3 three = new Vector3(Single.Parse(values[0]), Single.Parse(values[1]), Single.Parse(values[2]));
            Urho.IO.Log.WriteRaw("parsed vector3:" + three.ToString(), true);
            return three;
        }




        //Unfortunately urhosharp does not support getting the shader parameters directly yet, so we do it the hard way.
        //public string GetShaderParameterFromXML(Material m, string name)
        //{
        //    //Urho.IO.Log.WriteRaw("name:" + m.Name +"\n", true);
        //    XmlFile file = ResourceCache.GetXmlFile(m.Name);

        //    string value = "";
        //    if (file != null)
        //    {
        //        XmlElement e = file.GetRoot("material");

        //        int i = 0;
        //        for (XmlElement element = e.GetChild("parameter"); element != null; element = element.GetNext("parameter"))
        //        {
        //            Urho.IO.Log.WriteRaw("parameter " + element.GetAttribute("name"), true);

        //            if (element.GetAttribute("name") == name)
        //            {
        //                value = element.GetAttribute("value");
        //                break;
        //            }
        //        }
        //    }
        //    return value;
        //}
    }
}
