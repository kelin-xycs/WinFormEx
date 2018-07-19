using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace WinFormEx
{
    public class FormEx : Form
    {


        static FormEx()
        {
            MessageFilter.Init();
        }



        public event MouseEventHandlerEx MouseMoveEx;

        public event MouseEventHandlerEx MouseDownEx;

        public event MouseEventHandlerEx MouseUpEx;

        public event MouseEventHandlerEx MouseClickEx;
        
        public event EventHandler MouseHoverEx;

        public event EventHandler MouseEnterEx;



        protected override void OnMouseLeave(EventArgs e)
        {
            //  用于实现 MouseEnter 事件， 参考 MessageFilter 类消息处理中使用  MouseEventUtil.SetEnter(c) 的部分
            //  这里使用 WinForm 本身的 MouseLeave 事件，
            //  是因为 WinForm 本身的 MouseLeave 事件 可以捕获到窗口弹出模态对话框时的 MouseLeave 事件
            //  在 MessageFilter 类中单纯靠接收 Win32.WM_MOUSELEAVE 消息是不能捕获到弹出模态对话框时的 MouseLeave 事件的
            //  具体可参考 MessageFilter.RaiseCommonEvent(ref Message m, Control c) 方法内的注释
            MouseEventUtil.SetLeave(this);

            base.OnMouseLeave(e);
        }

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
    }
}
