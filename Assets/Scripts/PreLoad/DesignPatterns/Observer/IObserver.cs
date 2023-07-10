using UnityEngine;
using UnityEditor;

public interface IObserver<TValue>
{
    void OnNotify(TValue pValue);
}