using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xzToolsKit
{
    public class Main
    {
        public static void showUI()
        {
            try
            {
                FrmMain frmMain = new FrmMain();
                frmMain.init();
            } catch(Exception ept)
            {
                MessageBox.Show("启动出错："+ept.Message);
            }
        }
    }
}
