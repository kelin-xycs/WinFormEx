using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class StartTip : Form
    {
        public StartTip()
        {
            InitializeComponent();
        }

        private void StartTip_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }
    }
}
