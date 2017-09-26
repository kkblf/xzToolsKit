using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xzToolsKit.model
{
    class DataCenter
    {
        public static DateTime lastLoginTime { get; set; }
        public static Boolean isExiting { get; set; }
         public static String wsId = "eba0c2b007684465aeb0f7270bd5c12c";
        //public static String wsId = "ca0df842837e4fa4ac674793eac41c0f";
        public static LoginResponse loginResponse { get; set; }

        public static List<WorkspaceMeta> workspaceList { get; set; }

        public static String currentWorkspaceId { get; set; }

        public static String  parentId { get; set; }

        public static void clear()
        {
            loginResponse = null;
            workspaceList = null;
            currentWorkspaceId = null;
            parentId = null;
            isExiting = false;
        }
    }
}
