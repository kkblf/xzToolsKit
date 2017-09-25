using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xzToolsKit.model;
using xzToolsKit.proxy;

namespace xzToolsKit.Service
{
    public class WorkspaceService
    {
        private WorkspaceProxy workspaceProxy { get; set; }

        public WorkspaceService(WorkspaceProxy workspaceProxy)
        {
            this.workspaceProxy = workspaceProxy;
            if (this.workspaceProxy == null)
            {
                this.workspaceProxy = new WorkspaceProxy();
            }
        }
        public void checkWorkspaceIsRight(String wsId)
        {
            if (DataCenter.workspaceList == null || DataCenter.workspaceList.Count==0)
            {
                getWorkspaceList();
            }

            if(DataCenter.workspaceList==null || DataCenter.workspaceList.Count == 0)
            {
                throw new Exception("空间不存在，请注销后重新登陆！");
            }

            Boolean finded = false;

            DataCenter.workspaceList.ForEach(e =>
            {
                if (e.id.Equals(wsId))
                {
                    finded = true;
                }
            });

            if (finded == false)
            {
                throw new Exception("空间不存在，请注销后重新登陆！");

            }
        }

        public void getWorkspaceList()
        {
            DataCenter.workspaceList = workspaceProxy.list();
        }
    }
}
