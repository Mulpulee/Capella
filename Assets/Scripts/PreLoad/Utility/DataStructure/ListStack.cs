using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class ListStack<T> : List<T>
{
    new public void Add(T item) { throw new NotSupportedException(); }
    new public void AddRange(IEnumerable<T> collection) { throw new NotSupportedException(); }
    new public void Insert(int index, T item) { throw new NotSupportedException(); }
    new public void InsertRange(int index, IEnumerable<T> collection) { throw new NotSupportedException(); }
    new public void Reverse() { throw new NotSupportedException(); }
    new public void Reverse(int index, int count) { throw new NotSupportedException(); }
    new public void Sort() { throw new NotSupportedException(); }
    new public void Sort(Comparison<T> comparison) { throw new NotSupportedException(); }
    new public void Sort(IComparer<T> comparer) { throw new NotSupportedException(); }
    new public void Sort(int index, int count, IComparer<T> comparer) { throw new NotSupportedException(); }

    public void Push(T item)
    {
        base.Add(item);
    }

    public T Pop()
    {
        Int32 index = base.Count - 1;
        var instance = base[index];
        base.RemoveAt(index);
        return instance;
    }

    public T Peek()
    {
        Int32 index = base.Count - 1;
        return base[index];
    }
}