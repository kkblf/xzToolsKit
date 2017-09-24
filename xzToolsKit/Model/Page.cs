using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xzToolsKit.model
{
    public class Page<T>
    {
        public Int32 totalItemCount { get; set; }
        public Int32 pageItemCount { get; set; }
        public Int32 pageIndex { get; set; }
        public Int32 pageCount { get; set; }
        public T items { get; set; }
    }
}
