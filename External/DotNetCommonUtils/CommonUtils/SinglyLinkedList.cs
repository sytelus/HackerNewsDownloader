using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    [DebuggerDisplay("{DebugCount}")]
    public class SinglyLinkedList<T> : IEnumerable<T>, ICollection<T>
    {
        public SinglyLinkedListNode<T> First { get; private set; }
        public SinglyLinkedListNode<T> Last { get; private set; }
        public int Count { get; private set; }

        public SinglyLinkedList()
        {
            //allow default constructor
        }

        public SinglyLinkedList(IEnumerable<T> items)
        {
            this.AddRange(items);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var nextNode = this.First;
            while (nextNode != null)
            {
                yield return nextNode.Value;
                nextNode = nextNode.Next;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerable<SinglyLinkedListNode<T>> Nodes()
        {
            var nextNode = this.First;
            while (nextNode != null)
            {
                yield return nextNode;
                nextNode = nextNode.Next;
            }            
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
                this.Add(item);
        }

        public SinglyLinkedListNode<T> AddLast(T item)
        {
            return this.Add(item);
        }

        public SinglyLinkedListNode<T> Add(T item)
        {
            var node = new SinglyLinkedListNode<T>(item);
            if (this.First == null)
            {
                this.First = node;
                this.Last = node;
            }
            else
            {
                this.Last.SetNext(node);
                this.Last = node;
            }

            this.Count++;

            return node;
        }

        public bool IsEmpty { get { return this.First == null; } }

        public void RemoveAfter(SinglyLinkedListNode<T> nodeToRemoveAfter)
        {
            if (nodeToRemoveAfter == null || nodeToRemoveAfter.Next == null)
                return;

            var nodeToRemove = nodeToRemoveAfter.Next;

            if (nodeToRemove == this.Last)
                this.Last = nodeToRemoveAfter;

            nodeToRemoveAfter.SetNext(nodeToRemove.Next);
            nodeToRemove.SetNext(null);   //Avoid zombi nodes pointing back

            this.Count--;
        }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public void Clear()
        {
            this.First = null;
            this.Last = null;
            this.Count = 0;
        }

        public bool Contains(T item)
        {
            throw new NotSupportedException("Contains operation in SinglyLinkedList is not supported");
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var i = 0;
            foreach (var item in this)
                array[arrayIndex + (i++)] = item;
        }

        public int DebugCount 
        { 
            get 
            {   var counter = 0;
                foreach (var item in this)
                    counter++;
                return counter;
            } 
        }

        public T[] DebugList
        {
            get 
            { 
                var items = new T[this.DebugCount]; 
                this.CopyTo(items, 0);
                return items;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("Remove operation in SinglyLinkedList is not supported");
        }
    }
}
