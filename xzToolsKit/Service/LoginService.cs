using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xzToolsKit.model;
using xzToolsKit.proxy;

namespace xzToolsKit.Service
{
    public class LoginService
    {
        public Boolean needLogin()
        {
            Boolean ret = true;

            ret = !isLoginSuccess();

            if (ret == false)
            {
                if ((DateTime.Now - DataCenter.lastLoginTime).TotalSeconds > DataCenter.loginResponse.expires_in / 3)
                    ret = true;
            }

            return ret; 
        }

        public Boolean isLoginSuccess()
        {
            return DataCenter.loginResponse != null && DataCenter.loginResponse.access_token != null;
        }

        public void login(String grandType, String userName, String password, String auth)
        {
            String trialKey = new DocumentProxy().getTrialInfo(auth).key;
            DataCenter.loginResponse = new AccountProxy().login(grandType, userName, password,trialKey);
            DataCenter.lastLoginTime = DateTime.Now;
        }
    }
}
