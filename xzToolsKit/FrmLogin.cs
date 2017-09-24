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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                String grandType = "password";
                String userName = this.textBoxUsername.Text;
                String password = this.textBoxPassword.Text;
                String auth = "11-22-33-44";
                new LoginService().login(grandType,userName,password,auth);
                if(!new LoginService().isLoginSuccess())
                {
                    throw new Exception("登录失败");
                }
                DialogResult = DialogResult.OK;

            } catch (Exception ept)
            {
                labTip.Text = ept.Message;
            }
        }
    }
}
