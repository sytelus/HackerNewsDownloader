using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// Data structure that allows to,
    ///  1. Enumerate elements as singly linked list.
    ///  2. Add new elements only if they are not already present in the list.
    ///  3. Truncate list after specified node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{DebugDescription}")]
    public class LinkedListSet<T> : IEnumerable<T>
    {
        private readonly HashSet<T> hashSet = new HashSet<T>();
        private readonly SinglyLinkedList<T> linkedList = new SinglyLinkedList<T>();

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
                this.Add(item);
        }

        public SinglyLinkedListNode<T> First
        {
            get { return this.linkedList.First; }
        }

        public int Count
        {
            get { return this.hashSet.Count; }
        }

        public bool Contains(T item)
        {
            return this.hashSet.Contains(item);
        }

        public virtual void Add(T item)
        {
            if (this.hashSet.Add(item))
                linkedList.Add(item);            
        }

        public IEnumerable<T> Items()
        {
            return this.hashSet;
        }

        public void RemoveAllAfter(SinglyLinkedListNode<T> nodeToRemoveAllAfter)
        {
            var nextNodeToRemoveAfter = nodeToRemoveAllAfter;
            while (nextNodeToRemoveAfter != null && nextNodeToRemoveAfter.Next != null)
                this.RemoveAfter(nextNodeToRemoveAfter);
        }

        public string DebugDescription
        {
            get
            {
                var description = new StringBuilder();
                description.Append(this.Count);
                foreach (var item in this)
                {
                    description.Append(",");
                    description.Append(item.ToString());
                }
                return description.ToString();
            }
        }

        public override string ToString()
        {
            return this.DebugDescription;
        }

        public virtual void RemoveAfter(SinglyLinkedListNode<T> nodeToRemoveAfter)
        {
            if (nodeToRemoveAfter == null || nodeToRemoveAfter.Next == null)
                return;

            this.hashSet.Remove(nodeToRemoveAfter.Next.Value);

            this.linkedList.RemoveAfter(nodeToRemoveAfter);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.hashSet.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.hashSet.GetEnumerator();
        }

    }
}
