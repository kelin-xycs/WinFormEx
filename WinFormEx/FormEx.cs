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


        public FormEx()
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


        private bool isAddedFormActivatedEvent;

        public virtual bool OnMouseEnterEx(EventArgs e)
        {
            //  这段注册 Form.Activated 事件的代码是为了解决 窗口 弹出 模态对话框 时不能捕获到 MouseLeave 消息的问题
            //  当 模态对话框 关闭 ， 焦点回到 主窗口 时 ， 会触发 Form.Activated 事件 ，
            //  此时 ， 可以理解为 窗口 在 模态对话框 的 阻塞 结束后 ， 获取到了 MouseLeave 消息
            //  于是 ， 可以在 Form.Activated 做一些 MouseLeave 时要做的事
            //  但同时 ， 需要调用 MouseEventUtil.SetLeave(this); 方法 告诉 MessageFilter 鼠标已经 Leave ，
            //  否则 ， 下次 MouseEnter 事件将不能触发
            //  可以在下面 Form_Activated() 方法中看到 调用 MouseEventUtil.SetLeave(this);
            //  大家可能会问 ， 如果 Form.Activated 时， 鼠标刚好在 控件 上呢， 
            //  这样岂不是会出现 鼠标在控件上，但控件效果却显示为鼠标已经离开（Leave）的状态
            //  是这样的 ，
            //  在 Form.Activated 事件后 ，如果鼠标在控件上，会 接着收到 WM_MOUSELEAVE 消息 ， 触发 MouseMoveEx 事件，
            //  在 MessageFilter 中会根据 MouseMoveEx 来触发 MouseEnter 事件
            //  于是 ， 就相当于紧接着 鼠标 又 Enter 了， 相当于一直保持着 Enter 的状态
            Form form;

            if (!isAddedFormActivatedEvent)
            {
                form = this.FindForm();

                if (form != null)
                {
                    form.Activated += Form_Activated;

                    isAddedFormActivatedEvent = true;
                }
            }


            if (this.MouseEnterEx != null)
            {
                this.MouseEnterEx(this, e);
            }

            return false;
        }

        void Form_Activated(object sender, EventArgs e)
        {
            //  通知 MessageFilter 鼠标已离开 ， 参考上面 OnMouseEnterEx() 方法里的注释
            MouseEventUtil.SetLeave(this);

            OnMouseLeaveEx(e);
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
