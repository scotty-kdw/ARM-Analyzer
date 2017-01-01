using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARMAnalyzer
{
    public class Chain
    {
        private LinkedList<Node> EndList = new LinkedList<Node>();
        public Node Curr = null;

        public void AddNode(Node next, Node child)
        {
            EndList.AddFirst(child);
            child.next = next;

            Curr = child;
        }

        public Node FindNode(int idx, string dst)
        {
            IEnumerator<Node> visit = EndList.GetEnumerator();
            while (visit.MoveNext())
            {
                if ((visit.Current.index != idx) && (visit.Current.src == dst))
                {
                    return visit.Current;
                }
            }
            return null;
        }

        public void RemoveValue(int idx, string target)
        {
            Node trash = FindNode(idx, target);
            if ( trash != null)
            {
                EndList.Remove(trash);
            }
        }
    }
}
