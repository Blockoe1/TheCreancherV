/*****************************************************************************
// File Name : Enemy.cs
// Author : Arcadia Koederitz
// Creation Date : 5/25/2026
// Last Modified : 5/25/2026
//
// Brief Description : Base script for enemies that controls their limbs and actions during combat.
*****************************************************************************/
using System;

namespace FoolsBrand
{
    public class MinPriorityQueue<T> : ListMinHeap<PriorityQueueNode<T>>
    {
        public int Count => list.Count;

        public void Enqueue(T item, int priority)
        {
            PriorityQueueNode<T> node = new PriorityQueueNode<T>(item, priority);
            base.Add(node);
        }

        public T Dequeue()
        {
            PriorityQueueNode<T> node = base.RemoveMin();
            return node.Data;
        }

    }

    public class PriorityQueueNode<T> : IComparable<PriorityQueueNode<T>>
    {
        private static int insertionCount = 0;
        private T data;
        private int priority;
        private int insertionOrder;

        internal PriorityQueueNode(T data, int priority)
        {
            this.data = data;
            this.priority = priority;
            this.insertionOrder = insertionCount;
            insertionCount++;
        }

        internal T Data => data;

        public int CompareTo(PriorityQueueNode<T> obj)
        {
            if(priority > obj.priority)
            {
                return 1;
            }
            else if (priority < obj.priority)
            {
                return -1;
            }
            else if (insertionOrder > obj.insertionOrder)
            {
                return 1;
            }
            else if (insertionOrder < obj.insertionOrder)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public override string ToString()
        {
            return data.ToString();
        }
    }
}
