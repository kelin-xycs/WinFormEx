using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using System.Windows.Forms;

namespace WinFormEx
{
    public class ContentPanel : Panel
    {

        private Panel header;

        public Panel Header
        {
            get { return header; }
            set { header = value; }
        }

        private Panel body;

        public Panel Body
        {
            get { return body; }
            set { body = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {


            Rectangle b = this.Bounds;

            SolidBrush brush = new SolidBrush(Color.Gray);

            Pen pen = new Pen(brush);

            
            if (this.Header != null)
            {

                Point p1 = new Point(0, this.Header.Height);
                Point p2 = new Point(0, this.Height - 1);
                Point p3 = new Point(this.Width - 1, this.Height - 1);
                Point p4 = new Point(this.Width - 1, this.Header.Height);

                e.Graphics.DrawLines(pen, new Point[] { p1, p2, p3, p4 });
            }


            base.OnPaint(e);

        }
    }
}
