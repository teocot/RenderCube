using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Gui;
namespace RenderCube
{
    class Spacer : UIElement
    {
        public Spacer(int width = -1) : base()
        {
            if(width > 0)
                this.SetFixedWidth(width);
        }
    }
}
