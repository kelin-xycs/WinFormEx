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

            //  MouseLeave 事件使用 WinForm 本身提供的 MouseLeave 事件，
            //  WinForm 提供的 MouseLeave 事件做的完善， 可以捕获到窗口弹出模态对话框时的 MouseLeave 事件
            //  而且刚好 MouseLeave 事件不需要冒泡
            this.MouseLeave += ImageButton_MouseLeave;

        }

        
        void ImageButton_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        void ImageButton_MouseEnterEx(object sender, EventArgs e)
        {
            this.BackColor = Color.Aqua;
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
