using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;

using System.Windows.Forms;

namespace WinFormEx
{
    public class Form360 : FormEx
    {


        private ContentPanel pnlContent;


        private bool dragMouseDown = false;

        private int dragOriginX;
        private int dragOriginY;

        private int dragOriginMouseX;
        private int dragOriginMouseY;


        protected int resizeCornerSize = 10;
        protected int resizeCornerOffset = 6;

        protected bool isMouseInResizeArea = false;
        private bool isResizing = false;

        private ResizePosition resizePosition = ResizePosition.None;

        private int resizeOriginWidth;
        private int resizeOriginHeight;

        private int resizeMouseOriginX;
        private int resizeMouseOriginY;

        private int resizeOriginTop;
        private int resizeOriginLeft;

        private enum ResizePosition
        {
            None,
            RightBottom,
            LeftTop,
            LeftBottom,
            RightTop,
            Bottom,
            Top,
            Left,
            Right
        }


        private ShadowEx shadowEx = new ShadowEx();

        public ShadowEx ShadowEx
        {
            get { return shadowEx; }
            set { shadowEx = value; }
        }


        protected ContentPanel Content
        {
            get { return pnlContent; }
        }

        public Form360()
        {
            
            Panel pnlHeader = new Panel();
            Panel pnlBody = new Panel();
            pnlContent = new ContentPanel();


            pnlContent.SuspendLayout();
            this.SuspendLayout();


            this.FormBorderStyle = FormBorderStyle.None;
            this.ShadowEx.Width = 8;

            this.Resize += Form360_Resize;
            this.MouseMoveEx += Form360_MouseMoveEx;
            this.MouseDownEx += Form360_MouseDownEx;
            this.MouseUpEx += Form360_MouseUpEx;


            pnlHeader.Height = 120;
            pnlHeader.BackColor = Color.LightGreen;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.MouseDown += pnlHeader_MouseDown;
            pnlHeader.MouseMove += pnlHeader_MouseMove;
            pnlHeader.MouseUp += pnlHeader_MouseUp;


            pnlContent.Header = pnlHeader;
            pnlContent.Controls.Add(pnlHeader);


            int c = this.resizeCornerOffset;
            int s = this.ShadowEx.Width;
            int w = s > c ? s : c;

            pnlContent.Location = new Point(w, w);
            pnlContent.BackColor = Color.White;


            Rectangle b = this.Bounds;

            pnlContent.Width = b.Width - w * 2;
            pnlContent.Height = b.Height - w * 2;



            pnlBody.Location = new Point(1, pnlHeader.Height);
            pnlBody.Width = pnlContent.Width - 2;
            pnlBody.Height = pnlContent.Height - pnlHeader.Height - 1;



            pnlContent.Body = pnlBody;
            pnlContent.Controls.Add(pnlBody);



            this.Controls.Add(pnlContent);



            pnlContent.ResumeLayout();
            this.ResumeLayout();

        }

        private void Form360_MouseMoveEx(object sender, MouseEventArgsEx e)
        {
            if (this.isResizing)
            {
                MouseResize(e);
            }
            else
            {
                ShowResizeMouseCursor(e);
            }
        }

        private void ShowResizeMouseCursor(MouseEventArgsEx e)
        {

            Point p = PointToClient(new Point(e.ScreenX, e.ScreenY));


            int w = this.Content.Width;
            int h = this.Content.Height;

            int x = this.Content.Left;
            int y = this.Content.Top;

            int offset = this.resizeCornerOffset;

            //  x1 y1 x2 y2 x3 y3 x4 y4  的  1 2 3 4  的 顺序 是 左上角 左下角 右下角 右上角
            int x1 = x - offset;
            int y1 = y - offset;
            int x2 = x - offset;
            int y2 = y + h - 1 + offset;
            int x3 = x + w - 1 + offset;
            int y3 = y + h - 1 + offset;
            int x4 = x + w - 1 + offset;
            int y4 = y - offset;


            int c = this.resizeCornerSize;


            if (p.X >= x3 - c && p.X <= x3 && p.Y >= y3 - c && p.Y <= y3)
            {
                this.Cursor = Cursors.SizeNWSE;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.RightBottom;
            }
            else if (p.X >= x1 && p.X <= x1 + c && p.Y >= y1 && p.Y <= y1 + c)
            {
                this.Cursor = Cursors.SizeNWSE;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.LeftTop;
            }
            else if (p.X >= x2 && p.X <= x2 + c && p.Y >= y2 - c && p.Y <= y2)
            {
                this.Cursor = Cursors.SizeNESW;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.LeftBottom;
            }
            else if (p.X >= x4 - c && p.X <= x4 && p.Y >= y4 && p.Y <= y4 + c)
            {
                this.Cursor = Cursors.SizeNESW;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.RightTop;
            }
            else if (p.X >= x2 && p.X <= x3 && p.Y >= y2 - c && p.Y <= y2)
            {
                this.Cursor = Cursors.SizeNS;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.Bottom;
            }
            else if (p.X >= x1 && p.X <= x4 && p.Y >= y1 && p.Y <= y1 + c)
            {
                this.Cursor = Cursors.SizeNS;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.Top;
            }
            else if (p.X >= x1 && p.X <= x1 + c && p.Y >= y1 && p.Y <= y2)
            {
                this.Cursor = Cursors.SizeWE;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.Left;
            }
            else if (p.X >= x4 - c && p.X <= x4 && p.Y >= y4 && p.Y <= y3)
            {
                this.Cursor = Cursors.SizeWE;
                this.isMouseInResizeArea = true;
                this.resizePosition = ResizePosition.Right;
            }
            else
            {
                if (this.isMouseInResizeArea)
                {
                    this.Cursor = Cursors.Default;
                    this.isMouseInResizeArea = false;
                }
            }
        }

        private void MouseResize(MouseEventArgsEx e)
        {


            int oX;
            int oY;



            Point p = new Point(e.ScreenX, e.ScreenY);


            if (this.resizePosition == ResizePosition.RightBottom)
            {
                oX = p.X - this.resizeMouseOriginX;
                oY = p.Y - this.resizeMouseOriginY;

                this.Width = this.resizeOriginWidth + oX;
                this.Height = this.resizeOriginHeight + oY;
            }
            else if (this.resizePosition == ResizePosition.LeftTop)
            {
                oX = p.X - this.resizeMouseOriginX;
                oY = p.Y - this.resizeMouseOriginY;

                this.Width = this.resizeOriginWidth - oX;
                this.Height = this.resizeOriginHeight - oY;

                this.Left = this.resizeOriginLeft + oX;
                this.Top = this.resizeOriginTop + oY;
            }
            else if (this.resizePosition == ResizePosition.LeftBottom)
            {
                oX = p.X - this.resizeMouseOriginX;
                oY = p.Y - this.resizeMouseOriginY;

                this.Width = this.resizeOriginWidth - oX;
                this.Height = this.resizeOriginHeight + oY;

                this.Left = this.resizeOriginLeft + oX;
            }
            else if (this.resizePosition == ResizePosition.RightTop)
            {
                oX = p.X - this.resizeMouseOriginX;
                oY = p.Y - this.resizeMouseOriginY;

                this.Width = this.resizeOriginWidth + oX;
                this.Height = this.resizeOriginHeight - oY;

                this.Top = this.resizeOriginTop + oY;
            }
            else if (this.resizePosition == ResizePosition.Bottom)
            {
                oY = p.Y - this.resizeMouseOriginY;

                this.Height = this.resizeOriginHeight + oY;
            }
            else if (this.resizePosition == ResizePosition.Top)
            {
                oY = p.Y - this.resizeMouseOriginY;

                this.Height = this.resizeOriginHeight - oY;

                this.Top = this.resizeOriginTop + oY;
            }
            else if (this.resizePosition == ResizePosition.Right)
            {
                oX = p.X - this.resizeMouseOriginX;

                this.Width = this.resizeOriginWidth + oX;
            }
            else if (this.resizePosition == ResizePosition.Left)
            {
                oX = p.X - this.resizeMouseOriginX;
                
                this.Width = this.resizeOriginWidth - oX;

                this.Left = this.resizeOriginLeft + oX;
            }
        }

        private void Form360_MouseUpEx(object sender, MouseEventArgsEx e)
        {
            this.isResizing = false;
        }
        
        private void Form360_MouseDownEx(object sender, MouseEventArgsEx e)
        {


            if (!this.isMouseInResizeArea)
                return;


            this.isResizing = true;


            this.resizeOriginWidth = this.Width;
            this.resizeOriginHeight = this.Height;



            this.resizeMouseOriginX = e.ScreenX;
            this.resizeMouseOriginY = e.ScreenY;


            this.resizeOriginTop = this.Location.Y;
            this.resizeOriginLeft = this.Location.X;

        }

        private void Form360_Resize(object sender, EventArgs e)
        {

            Rectangle b = this.Bounds;

            int c = this.resizeCornerOffset;

            int s = this.ShadowEx.Width;

            int w = s > c ? s : c;

            pnlContent.Width = b.Width - w * 2;
            pnlContent.Height = b.Height - w * 2;

            pnlContent.Body.Width = pnlContent.Width - 2;
            pnlContent.Body.Height = pnlContent.Height - pnlContent.Header.Height - 1;

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {


            Rectangle b = this.Bounds;


            Rectangle rBody = new Rectangle(0, 0, b.Width - 6, b.Height - 6);

            SolidBrush brushBody = new SolidBrush(Color.White);



            SolidBrush brush1 = new SolidBrush(Color.Gray);

            Pen pen = new Pen(brush1);

            int x = this.Content.Left;
            int y = this.Content.Top;

            int w = this.Content.Width;
            int h = this.Content.Height;

            e.Graphics.DrawLine(pen, b.Width - 1, y + 8, b.Width - 1, b.Height - 1);
            e.Graphics.DrawLine(pen, b.Width - 1 - 5, y + 3, b.Width - 1 - 5, b.Height - 1 - 5);
            e.Graphics.DrawLine(pen, x + 8, b.Height - 1, b.Width - 1, b.Height - 1);
            e.Graphics.DrawLine(pen, x + 3, b.Height - 1 - 5, b.Width - 1 - 5, b.Height - 1 - 5);

        }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        {


            this.dragMouseDown = true;

            this.dragOriginX = this.Location.X;

            this.dragOriginY = this.Location.Y;


            Point p = PointToScreen(new Point(e.X, e.Y));


            this.dragOriginMouseX = p.X;

            this.dragOriginMouseY = p.Y;

        }

        private void pnlHeader_MouseMove(object sender, MouseEventArgs e)
        {

            if (!this.dragMouseDown)
                return;

            if (this.isMouseInResizeArea)
                return;

            Point p = PointToScreen(new Point(e.X, e.Y));

            this.Location = new Point(this.dragOriginX + p.X - this.dragOriginMouseX, this.dragOriginY + p.Y - this.dragOriginMouseY);

        }

        private void pnlHeader_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragMouseDown = false;
        }
    }
}
