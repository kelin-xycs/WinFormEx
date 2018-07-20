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


            
            //  下面注释的 这 2 个 Panel 是测试用的 ， 
            //  测试 在这 2 个 Panel 上的 MouseEnter MouseLeave 事件 会不会 冒泡 到 ImageButton

            //Panel panel = new Panel();
            //panel.Width = 50;
            //panel.Height = 50;
            //panel.BackColor = Color.AliceBlue;

            //Panel panel2 = new Panel();
            //panel2.Width = 25;
            //panel2.Height = 25;
            //panel2.BackColor = Color.AntiqueWhite;

            //panel.Controls.Add(panel2);

            //this.Controls.Add(panel); 
        }

        void ImageButton_MouseLeaveEx(object sender, EventArgs e)
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
