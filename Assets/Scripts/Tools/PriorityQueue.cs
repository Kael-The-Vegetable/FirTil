using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PriorityQueue<TElement, TPriority>
{
	private readonly List<(TElement Element, TPriority Priority)> heap = new();
	private readonly IComparer<TPriority> comparer;

	public int Count => heap.Count;

	public PriorityQueue() : this(null) { }

	public PriorityQueue(IComparer<TPriority> customComparer)
	{
		comparer = customComparer ?? Comparer<TPriority>.Default;
	}

	public void Enqueue(TElement element, TPriority priority)
    {
        heap.Add((element, priority));
        HeapifyUp(heap.Count - 1);
    }

    public TElement Dequeue()
    {
        if (heap.Count == 0)
            throw new ArgumentOutOfRangeException("Priority queue is empty.");

        var result = heap[0].Element;
        var last = heap[^1];
        heap.RemoveAt(heap.Count - 1);

        if (heap.Count > 0)
        {
            heap[0] = last;
            HeapifyDown(0);
        }

        return result;
    }

    public TElement Peek()
    {
        if (heap.Count == 0)
            throw new ArgumentOutOfRangeException("Priority queue is empty.");
        return heap[0].Element;
    }

    private void HeapifyUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (comparer.Compare(heap[i].Priority, heap[parent].Priority) >= 0)
                break;

            Swap(i, parent);
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        int last = heap.Count - 1;
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left <= last && comparer.Compare(heap[left].Priority, heap[smallest].Priority) < 0)
                smallest = left;
            if (right <= last && comparer.Compare(heap[right].Priority, heap[smallest].Priority) < 0)
                smallest = right;

            if (smallest == i) break;

            Swap(i, smallest);
            i = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        (heap[i], heap[j]) = (heap[j], heap[i]);
    }
}
