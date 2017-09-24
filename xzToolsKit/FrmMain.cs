using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xzToolsKit.Service;

namespace xzToolsKit
{
    public partial class FrmMain : Form
    {
        /**
         * 登录服务 
         **/
        private LoginService loginService = null;

        public FrmMain()
        {
            InitializeComponent();
        }

        public void init()
        {
            loginService = new LoginService();
            if (loginService.needLogin())
            {
                FrmLogin frmLogin = new FrmLogin();
                DialogResult dialogResult = frmLogin.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    this.Show();
                }
            } else
            {
                this.Show();
            }
        }

        public void showProgress(Int32 value)
        {
            //toolStripProgressBar1.Value = value;
            Application.DoEvents();
        }
    }
}
