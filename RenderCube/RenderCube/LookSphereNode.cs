using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace RenderCube
{
    //this delegate lets us use a callback, so we can track an object even if it is moving.
    public delegate Vector3 CenterPositionCallBack();
    
    public class LookSphereNode : Node
    {
        public CenterPositionCallBack CenterPosition { get; set; }
        public LookSphereNode(String name, CenterPositionCallBack center, float radius = 1.0f, float theta=0.0f, float phi=0.0f)
        : base(){
            this.CenterPosition = center;
            this._Radius = radius;
            this._Theta = theta;
            this._Phi = phi;
            this.Name = name;
            this.UpdatePosition();
        }
        
        float _Radius;
        public float Radius
        {
            get { return _Radius; }
            set { _Radius = value; this.UpdatePosition(); }
        }
        private float WrapToZeroTwoPi(float value)
        {
            float ans = value % MathHelper.TwoPi;
            if (ans < 0) return ans + MathHelper.TwoPi;
            else return ans;
        }
        private float WrapToPi(float value)
        {
            float ans = value % MathHelper.TwoPi;
            if (ans > MathHelper.Pi) return ans - MathHelper.TwoPi;
            else return ans;
        }
        float _Theta;
        public float Theta {
            get { return _Theta; }
            set { _Theta = WrapToPi(value); this.UpdatePosition(); }
        }
        float _Phi;
        public float Phi
        {
            get { return _Phi; }
            set { _Phi = WrapToPi(value); this.UpdatePosition(); }
        }
        public void UpdatePosition()
        {
            //this computes the location on the sphere corresponding to the angles Theta and Phi
            var z = CenterPosition().Z + Radius * (float)Math.Sin(Theta) * (float)Math.Cos(Phi);
            var x = CenterPosition().X + Radius * (float)Math.Cos(Theta) * (float)Math.Cos(Phi);
            var y = CenterPosition().Y + Radius * (float)Math.Sin(Phi);
            Position = new Vector3(x, y, z);
            
            //make sure the camera is upside down when we are on the far side of the sphere.
            Vector3 up = (Math.Abs(Phi) < MathHelper.PiOver2) ? Vector3.UnitY : -Vector3.UnitY;

            //point the camera at the object's position.
            LookAt(CenterPosition(), up, TransformSpace.World);

        }
    }
}
