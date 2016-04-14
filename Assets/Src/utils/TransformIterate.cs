using System;
using System.Collections.Generic;
using UnityEngine;

namespace utils
{
    public static class TransformIterate
    {
        public static IEnumerable<Transform> IterateBreadthFirst(this Transform top)
        {
            if (top == null) {
                throw new ArgumentException("Object should no be null", "top");
            }
            var queue = new Queue<Transform>();
            queue.Enqueue(top);
            for (var current = queue.Dequeue(); current != null; current = queue.Dequeue()) {
                for (int i = 0; i < current.transform.childCount; i++) {
                    var child = current.transform.GetChild(i);
                    queue.Enqueue(child);
                    yield return child;
                }
                if (queue.Count == 0) { //cause Dequeue throws exception when is being called on empty queue. Great!
                    break;
                }
            }
        }
    }
}
