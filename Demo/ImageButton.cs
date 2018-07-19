using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using WinFormEx;

namespace Demo
{
    class ImageButton : ControlEx
    {

        private Image image;

        private string imageResourceName;

        public string ImageResourceNameEx
        {
            get { return imageResourceName; }
            set { imageResourceName = value; }
        }


        public ImageButton()
        {

            this.MouseEnterEx += ImageButton_MouseEnterEx;
            this.MouseLeaveEx += ImageButton_MouseLeaveEx;

        }

        void ImageButton_MouseLeaveEx(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        private bool isAddedFormActivatedEvent;

        void ImageButton_MouseEnterEx(object sender, EventArgs e)
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

            this.BackColor = Color.Aqua;
        }

        void Form_Activated(object sender, EventArgs e)
        {
            //  通知 MessageFilter 鼠标已离开 ， 参考上面 ImageButton_MouseEnterEx() 方法里的注释
            MouseEventUtil.SetLeave(this);

            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            if (this.image == null)
            {
                if (string.IsNullOrEmpty(this.imageResourceName))
                    throw new ImageButtonException("ImageResourceNameEx 属性不能为空 。 应设置 ImageResourceNameEx 属性来指明 ImageButton 的 图片资源 。");


                Assembly assm = Assembly.GetExecutingAssembly();

                using (Stream s = assm.GetManifestResourceStream(this.imageResourceName))
                {
                    if (s == null)
                        throw new ImageButtonException("找不到 名为 \"" + this.imageResourceName + "\" 的资源 。");

                    this.image = Image.FromStream(s);
                }
            } 
            

            e.Graphics.DrawImage(this.image, 5, 5, this.Width - 10, this.Height - 10);

            base.OnPaint(e);
        }
    }

    public class ImageButtonException : Exception
    {
        internal ImageButtonException(string message) : base(message)
        {

        }
    }
}
