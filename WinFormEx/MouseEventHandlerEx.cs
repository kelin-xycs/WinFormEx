using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace WinFormEx
{



    public delegate void MouseEventHandlerEx(object sender, MouseEventArgsEx e);



    public class MouseEventArgsEx
    {

        MouseButtons button;

        private int screenX;

        private int screenY;


        public MouseEventArgsEx(MouseButtons button, int screenX, int screenY)
        {
            this.button = button;

            this.screenX = screenX;

            this.screenY = screenY;
        }

        public MouseButtons Button
        {
            get { return this.button; }
        }

        public int ScreenX
        {
            get { return this.screenX; }
        }

        public int ScreenY
        {
            get { return this.screenY; }
        }
    }
   
}
