using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace WinFormEx
{
    public class MouseEventUtil
    {

        private static Dictionary<Control, bool> enterDic = new Dictionary<Control,bool>();
        private static Dictionary<Control, bool> clickDic = new Dictionary<Control, bool>();


        internal static bool SetEnter(Control ctrl)
        {

            if (!enterDic.ContainsKey(ctrl))
            {
                enterDic.Add(ctrl, true);
                return true;
            }

            return false;
        }

        public static void SetLeave(Control ctrl)
        {
            enterDic.Remove(ctrl);
        }

        internal static void SetClickDown(Control ctrl)
        {
            if (!clickDic.ContainsKey(ctrl))
            {
                clickDic.Add(ctrl, true);
            }
        }

        internal static bool SetClickUp(Control ctrl)
        {
            if (clickDic.ContainsKey(ctrl))
            {
                clickDic.Remove(ctrl);
                return true;
            }

            return false;
        }

        internal static bool CheckClick(Control ctrl, int x, int y)
        {

            Point p = ctrl.PointToClient(new Point(x, y));

            if (p.X >= 0 && p.Y >= 0 && p.X <= ctrl.Width - 1 && p.Y <= ctrl.Height - 1)
                return true;

            return false;

        }

    }
}
