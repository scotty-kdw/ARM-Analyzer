using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARMAnalyzer
{
    public class Node
    {
        public int index = 0;
        public string dst = null;
        public string src = null;
        //public Node prev = null;
        public Node next = null;

        public Node(int idx, string dst, string src)
        {
            this.index = idx;
            this.dst = dst;
            this.src = src;
        }
    }
}
