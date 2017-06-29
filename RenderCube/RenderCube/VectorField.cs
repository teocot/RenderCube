using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Gui;
using Urho;

namespace RenderCube
{
    class VectorFieldEventArgs
    {
        public VectorFieldEventArgs(UIElement e, Vector3 v)
        {
            this.Element = e;
            this.Value = v;
        }
        public UIElement Element;
        public Vector3 Value;
    }
    class VectorField : UIElement
    {
        LineEdit lX, lY, lZ;
        public Action<VectorFieldEventArgs> OnTextFinished = (args) => { };
        private Vector3 initialvalue;
        public Vector3 Value
        {
            get
            {
                Vector3 ret;
                if (Single.TryParse(lX.Text, out ret.X) &&
                    Single.TryParse(lY.Text, out ret.Y) &&
                    Single.TryParse(lZ.Text, out ret.Z))
                    return ret;
                return initialvalue;
            }
            set
            {
                lX.Text = value.X.ToString();
                lY.Text = value.Y.ToString();
                lZ.Text = value.Z.ToString();
            }
        }
        //3 editor action delegates.
        public VectorField(Vector3 values, string xlabel = "X", string ylabel = "Y", string zlabel = "Z") : base()
        {
            this.initialvalue = values;
            this.SetLayout(LayoutMode.Horizontal, 6, new IntRect(0, 0, 0, 0));
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.SetStyleAuto(null);
            this.SetFixedHeight(20);
            this.AddChild(new Spacer());
            lX = CreateField(xlabel, values.X);
            this.AddChild(new Spacer());
            lY = CreateField(ylabel, values.Y);
            this.AddChild(new Spacer());
            lZ = CreateField(zlabel, values.Z);
            this.AddChild(new Spacer());
            lX.SetStyleAuto(null);
            lY.SetStyleAuto(null);
            lZ.SetStyleAuto(null);
            Action<TextFinishedEventArgs> DefaultTextAction = (args) => {
                var newArgs = new VectorFieldEventArgs(args.Element, this.Value);
                this.Value = Value;
                this.OnTextFinished(newArgs);
            };
            lX.TextFinished += DefaultTextAction;
            lY.TextFinished += DefaultTextAction;
            lZ.TextFinished += DefaultTextAction;
        }
        public LineEdit CreateField(string label, float initvalue)
        {
            Text labelText = new Text();
            labelText.Value = label;
            LineEdit ret = new LineEdit();
            ret.Text = initvalue.ToString();


            labelText.SetStyleAuto(null);
            ret.SetStyleAuto(null);
            this.AddChild(labelText);
            this.AddChild(ret);
            return ret;
        }
    }
}
