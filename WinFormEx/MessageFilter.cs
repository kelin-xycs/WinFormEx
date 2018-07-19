using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

using System.Security;
using System.Security.Permissions;

namespace WinFormEx
{
    internal class MessageFilter : IMessageFilter
    {


        [ThreadStatic]
        private static bool isAddedMessageFilter;


        public static void Init()
        {
            if (isAddedMessageFilter == false)
            {
                Application.AddMessageFilter(new MessageFilter());

                isAddedMessageFilter = true;
            }
        }

        public bool PreFilterMessage(ref Message m)
        {


            if (m.Msg == Win32.WM_MOUSEMOVE || m.Msg == Win32.WM_LBUTTONDOWN || m.Msg == Win32.WM_LBUTTONUP)
            {
                return RaiseMouseEvent(ref m);
            }


            return RaiseCommonEvent(ref m);

                
        }

        private bool RaiseCommonEvent(ref Message m)
        {

            Control ctrl;
            Control parent;

            ctrl = Control.FromHandle(m.HWnd);

            if (ctrl == null)
                return false;


            bool r;

            while (true)
            {

                r = RaiseCommonEvent(ref m, ctrl);

                // r == true 表示 终止冒泡 ， 于是 返回 true 终止冒泡
                // 这里 的 终止 冒泡 包含 2 层 意思 ，
                // 1 return 跳出 while 循环 ， 结束 在  PreFilterMessage()  里 的 事件冒泡，
                //   也就是 结束  WinFormEx  的 事件冒泡
                // 2 return true ， 也就是  PreFilterMessage()  方法 返回 true ， 则 当前 消息 不再继续传递 ，
                //   也就是 结束  WinForm 本身 的 事件 ， WinForm 本身 的 事件 接下来 也 不会被 触发
                if (r)
                    return true;

                parent = ctrl.Parent;

                if (parent == null)
                    break;


                ctrl = parent;

            }

            return false;
        }

        private bool RaiseCommonEvent(ref Message m, Control c)
        {
            
            FormEx form = c as FormEx;
            ControlEx ctrl = c as ControlEx;

            if (form == null && ctrl == null)
                return false;


            EventArgs e;

            bool r = false;

            switch (m.Msg)
            {

                case Win32.WM_MOUSEHOVER:
                    {
                        
                        e = new EventArgs();
                        
                        if (form != null)
                        {
                            r = form.OnMouseHoverEx(e);
                        }
                        else if (ctrl != null)
                        {
                            r = ctrl.OnMouseHoverEx(e);
                        }

                        // 如果 r == true ， 则返回 true 终止事件冒泡
                        if (r)
                            return true;

                        break;
                    }

                case Win32.WM_MOUSELEAVE:
                    {

                        //  用于实现 MouseEnter 事件， 参考 MouseMove 消息处理中使用  MouseEventUtil.SetEnter(c) 的部分
                        MouseEventUtil.SetLeave(c);

                        e = new EventArgs();

                        if (form != null)
                        {
                            r = form.OnMouseLeaveEx(e);
                        }
                        else if (ctrl != null)
                        {
                            r = ctrl.OnMouseLeaveEx(e);
                        }

                        // 如果 r == true ， 则返回 true 终止事件冒泡
                        if (r)
                            return true;

                        break;
                    }


            }

            return false;
        }

        private bool RaiseMouseEvent(ref Message m)
        {

            int x, y;
            Point p;

            Control ctrl;
            Control parent;

            ctrl = Control.FromHandle(m.HWnd);

            if (ctrl == null)
                return false;



            GetMouseCoordinates(ref m, out x, out y);


            p = ctrl.PointToScreen(new Point(x, y));


            bool r;

            while (true)
            {


                r = RaiseMouseEvent(ref m, p.X, p.Y, ctrl);

                // r == true 表示 终止冒泡 ， 于是 返回 true 终止冒泡
                // 这里 的 终止 冒泡 包含 2 层 意思 ，
                // 1 return 跳出 while 循环 ， 结束 在  PreFilterMessage()  里 的 事件冒泡，
                //   也就是 结束  WinFormEx  的 事件冒泡
                // 2 return true ， 也就是  PreFilterMessage()  方法 返回 true ， 则 当前 消息 不再继续传递 ，
                //   也就是 结束  WinForm 本身 的 事件 ， WinForm 本身 的 事件 接下来 也 不会被 触发
                if (r)
                    return true;



                parent = ctrl.Parent;

                if (parent == null)
                    break;



                ctrl = parent;

            }

            return false;
        }

        private bool RaiseMouseEvent(ref Message m, int x, int y, Control c)
        {

            FormEx form = c as FormEx;
            ControlEx ctrl = c as ControlEx;

            if (form == null && ctrl == null)
                return false;


            MouseEventArgsEx e;

            
            bool r = false;

            
            switch(m.Msg)
            {
                case Win32.WM_MOUSEMOVE :
                    {

                        e = new MouseEventArgsEx(MouseButtons.None, x, y);

                        //  这里是 实现 MouseEnter 事件 。 Windows 消息 中没有 MouseEnter 消息， 所以需要自己实现
                        //  .Net WinForm 也是自己实现的， 不过 .Net WinForm 实现的方式是调用一些  Native 方法
                        //  比如 RegisterWindowMessage()  TrackMouseEvent()       
                        //  这对于我们太复杂，我们只需要用 MouseMove 消息 和 MouseEventUtil 配合 就可以实现 Enter 事件
                        if (MouseEventUtil.SetEnter(c))
                        {
                            if (form != null)
                            {
                                r = form.OnMouseEnterEx(new EventArgs());
                            }
                            else if (ctrl != null)
                            {
                                r = ctrl.OnMouseEnterEx(new EventArgs());
                            }
                        }

                        // 如果 r == true ， 则返回 true 终止事件冒泡
                        if (r)
                            return true;

                        if (form != null)
                        {
                            r = form.OnMouseMoveEx(e);
                        }
                        else if (ctrl != null)
                        {
                            r = ctrl.OnMouseMoveEx(e);
                        }

                        // 如果 r == true ， 则返回 true 终止事件冒泡
                        if (r)
                            return true;

                        break;
                    }
                case Win32.WM_LBUTTONDOWN :
                    {
                        //  实现 Click 事件，参考  MouseUp 消息处理部分
                        MouseEventUtil.SetClickDown(c);

                        e = new MouseEventArgsEx(MouseButtons.Left, x, y);

                        if (form != null)
                        {
                            r = form.OnMouseDownEx(e);
                        }
                        else if (ctrl != null)
                        {
                            r = ctrl.OnMouseDownEx(e);
                        }

                        // 如果 r == true ， 则返回 true 终止事件冒泡
                        if (r)
                            return true;

                        break;
                    }
                case Win32.WM_LBUTTONUP :
                    {

                        e = new MouseEventArgsEx(MouseButtons.Left, x, y);

                        //  这里是 实现  Click 事件 。 Windows 消息 中没有 MouseClick 消息 ， 所以需要自己实现
                        //  .Net WinForm 也是自己实现的。
                        if (MouseEventUtil.CheckClick(c, x, y))
                        {
                            MouseEventUtil.SetClickUp(c);

                            if (form != null)
                            {
                                r = form.OnMouseClickEx(e);
                            }
                            else if (ctrl != null)
                            {
                                r = ctrl.OnMouseClickEx(e);
                            }
                        }

                        // 如果 r == true ， 则返回 true 终止事件冒泡
                        if (r)
                            return true;

                        if (form != null)
                        {
                            r = form.OnMouseUpEx(e);
                        }
                        else if (ctrl != null)
                        {
                            r = ctrl.OnMouseUpEx(e);
                        }

                        // 如果 r == true ， 则返回 true 终止事件冒泡
                        if (r)
                            return true;

                        break;
                    }
            }


            return false;
        }

        private void GetMouseCoordinates(ref Message m, out int x, out int y)
        {
            x = (int)((short)(((int)((long)m.LParam)) & 65535));
            y = (int)((short)(((int)((long)m.LParam)) >> 16 & 65535));
        }
    }
}
