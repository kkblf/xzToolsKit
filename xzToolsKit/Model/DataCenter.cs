using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xzToolsKit.model
{
    class DataCenter
    {
        public static DateTime lastLoginTime { get; set; }
        public static LoginResponse loginResponse { get; set; }

        public static List<WorkspaceMeta> workspaceList { get; set; }

        public static String currentWorkspaceId { get; set; }

        public static String  parentId { get; set; }
    }
}
