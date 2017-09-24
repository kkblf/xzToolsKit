using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace xzToolsKit.Handler
{
    class ExceptionHandler
    {
        public static void handle(Task<HttpResponseMessage> task,Int64 type)
        {
            switch (type)
            {
                case 1:
                    try
                    {
                        task.Result.EnsureSuccessStatusCode();
                    } catch(Exception e)
                    {
                        throw new Exception("登陆失败[" + task.Result.StatusCode + "]");
                    }
                    break;
            }
        }
    }
}
