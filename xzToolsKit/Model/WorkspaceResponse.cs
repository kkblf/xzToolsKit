using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xzToolsKit.model
{
    public class WorkspaceResponse<T>
    {
        public Int32 code { get; set; }
        public String message { get; set; }
        public T data { get; set; }
    }
}
