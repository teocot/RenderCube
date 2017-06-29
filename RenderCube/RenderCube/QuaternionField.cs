using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Gui;
using Urho;

namespace RenderCube
{
    class QuaternionFieldEventArgs
    {
        public QuaternionFieldEventArgs(UIElement e, Quaternion v)
        {
            this.Element = e;
            this.Value = v;
        }
        public UIElement Element;
        public Quaternion Value;
    }
    class QuaternionField : UIElement
    {
        LineEdit lX, lY, lZ, lW;
        public Action<QuaternionFieldEventArgs> OnTextFinished = (args) => { };
        private Quaternion initialvalue;
        public Quaternion Value
        {
            get
            {
                
                Single X, Y, Z, W;
                if (Single.TryParse(lX.Text, out X) &&
                    Single.TryParse(lY.Text, out Y) &&
                    Single.TryParse(lZ.Text, out Z) &&
                    Single.TryParse(lW.Text, out W))
                {
                    Quaternion ret = new Quaternion(X, Y, Z, W);
                    return ret;
                }
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
        public QuaternionField(Quaternion values, string xlabel = "X", string ylabel = "Y", string zlabel = "Z", string wlabel = "W") : base()
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
            lW = CreateField(wlabel, values.W);
            this.AddChild(new Spacer());
            lX.SetStyleAuto(null);
            lY.SetStyleAuto(null);
            lZ.SetStyleAuto(null);
            lW.SetStyleAuto(null);
            Action<TextFinishedEventArgs> DefaultTextAction = (args) => {
                var newArgs = new QuaternionFieldEventArgs(args.Element, this.Value);
                this.Value = Value;
                this.OnTextFinished(newArgs);
            };
            lX.TextFinished += DefaultTextAction;
            lY.TextFinished += DefaultTextAction;
            lZ.TextFinished += DefaultTextAction;
            lW.TextFinished += DefaultTextAction;
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
