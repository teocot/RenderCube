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
        //This is a simple spacer UIElement that can be a variable width spacer, or fixed width.
        public Spacer(int width = -1) : base()
        {
            if(width > 0)
                this.SetFixedWidth(width);
        }
    }
}
