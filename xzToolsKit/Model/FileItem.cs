using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xzToolsKit.model
{
    public class FileItem:Node
    {
        public String fileId { get; set; }
        public String workspaceId { get; set; }
        public String name { get; set; }
        public String suffix { get; set; }
        public Boolean folder { get; set; }
        public int? length { get; set; }
        public String parentId { get; set; }
        public String appKey { get; set; }
        public String creatorId { get; set; }
        public String creatorName { get; set; }
        public String createTime { get; set; }
        public String digest { get; set; }
        public String thumbnail { get; set; }
        public int? versionIndex { get; set; }
        public FileItem()
        {
            type = 2;
        }
    }
}
