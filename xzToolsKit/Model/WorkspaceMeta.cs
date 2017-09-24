using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xzToolsKit.model
{
    public class WorkspaceMeta:Node
    {
        public String id { get; set; }
        public String name { get; set; }
        public Boolean permanent { get; set; }
        public String description { get; set; }
        public long creatorId { get; set; }
        public String creatorName { get; set; }
        public Int32 memberStatus { get; set; }
        public Int32 memberRole { get; set; }
        public Int32 status { get; set; }
        public long createTime { get; set; }
        public WorkspaceMeta()
        {
            type = 1;
        }
    }
}
