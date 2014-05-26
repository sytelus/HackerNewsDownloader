using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public class SinglyLinkedListNode<T>
    {
        public T Value { get; set; }
        public SinglyLinkedListNode<T> Next { get; private set; }

        internal SinglyLinkedListNode(T value)
        {
            this.Value = value;
        }

        internal void SetNext(SinglyLinkedListNode<T> next)
        {
            this.Next = next;
        }
    }
}
