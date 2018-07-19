using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace WinFormEx
{
    public class ControlEx : Panel
    {


        public ControlEx()
        {
            MessageFilter.Init();
        }



        public event MouseEventHandlerEx MouseMoveEx;

        public event MouseEventHandlerEx MouseDownEx;

        public event MouseEventHandlerEx MouseUpEx;

        public event MouseEventHandlerEx MouseClickEx;

        public event EventHandler MouseHoverEx;

        public event EventHandler MouseEnterEx;

        public event EventHandler MouseLeaveEx;




        //  返回 true 则 终止冒泡    返回 false 则 冒泡正常继续
        public virtual bool OnMouseMoveEx(MouseEventArgsEx e)
        {
            if (this.MouseMoveEx != null)
            {
                this.MouseMoveEx(this, e);
            }

            return false;
        }

        public virtual bool OnMouseDownEx(MouseEventArgsEx e)
        {
            if (this.MouseDownEx != null)
            {
                this.MouseDownEx(this, e);
            }

            return false;
        }

        public virtual bool OnMouseUpEx(MouseEventArgsEx e)
        {
            if (this.MouseUpEx != null)
            {
                this.MouseUpEx(this, e);
            }

            return false;
        }

        public virtual bool OnMouseClickEx(MouseEventArgsEx e)
        {
            if (this.MouseClickEx != null)
            {
                this.MouseClickEx(this, e);
            }

            return false;
        }

        public virtual bool OnMouseHoverEx(EventArgs e)
        {
            if (this.MouseHoverEx != null)
            {
                this.MouseHoverEx(this, e);
            }

            return false;
        }

        public virtual bool OnMouseEnterEx(EventArgs e)
        {
            if (this.MouseEnterEx != null)
            {
                this.MouseEnterEx(this, e);
            }

            return false;
        }

        public virtual bool OnMouseLeaveEx(EventArgs e)
        {
            if (this.MouseLeaveEx != null)
            {
                this.MouseLeaveEx(this, e);
            }

            return false;
        }
    }
}
