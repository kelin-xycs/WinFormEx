using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinFormEx;

namespace Demo
{
    public partial class Form1 : Form360
    {

        ImageButton imgButton1;

        ImageButton imgButton2;

        Label lblTip;


        public Form1()
        {

            imgButton1 = new ImageButton();
            imgButton2 = new ImageButton();
            lblTip = new Label();

            this.SuspendLayout();

            imgButton1.Width = 80;
            imgButton1.Height = 80;
            imgButton1.ImageResourceNameEx = "Demo.Resources.b1.png";
            imgButton1.Location = new Point(25, 20);

            imgButton1.MouseClickEx += imgButton1_MouseClickEx;


            imgButton2.Width = 80;
            imgButton2.Height = 80;
            imgButton2.ImageResourceNameEx = "Demo.Resources.b10.png";
            imgButton2.Location = new Point(120, 20);

            imgButton2.MouseClickEx += imgButton2_MouseClickEx;



            this.Content.Header.Controls.Add(imgButton1);
            this.Content.Header.Controls.Add(imgButton2);



            lblTip.Text = "来一个 Git Desktop 那样的 深邃 的开始画面 ？";
            lblTip.Left = 20;
            lblTip.Top = 20;
            lblTip.Width = 400;
            lblTip.Font = new Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            lblTip.MouseEnter += lblTip_MouseEnter;
            lblTip.MouseClick += lblTip_MouseClick;

            this.Content.Body.Controls.Add(lblTip);




            
            this.ResumeLayout();




            InitializeComponent();

        }

        void lblTip_MouseClick(object sender, MouseEventArgs e)
        {
            StartTip startTip = new StartTip();
            startTip.Show();
        }

        void lblTip_MouseEnter(object sender, EventArgs e)
        {
            lblTip.Cursor = Cursors.Hand;
        }

        void imgButton1_MouseClickEx(object sender, MouseEventArgsEx e)
        {
            MessageBox.Show("imgButton1 Click .");
        }

        void imgButton2_MouseClickEx(object sender, MouseEventArgsEx e)
        {
            MessageBox.Show("imgButton2 Click .");
        }

    }
}
