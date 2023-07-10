using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHolder : IEnumerable<CustomComponent>, IEnumerator<CustomComponent>
{
    private List<CustomComponent> _components;

    public ComponentHolder()
    {
        _components = new List<CustomComponent>();
    }

    public ComponentHolder(List<CustomComponent> pComponents)
    {
        _components = new List<CustomComponent>();
        _components = pComponents;
    }

    public void Update()
    {
        for(Int32 i =0,iCount = _components.Count; i<iCount;i++)
        {
            if(_components[i].IsActivetd)
                _components[i].OnUpdate();
        }
    }

    public void FixedUpdate()
    {
        for (Int32 i = 0, iCount = _components.Count; i < iCount; i++)
        {
            if (_components[i].IsActivetd)
                _components[i].OnFixedUpdate();
        }
    }

    public CustomComponent AddComponent(CustomComponent pComponent)
    {
        _components.Add(pComponent);
        pComponent.OnAwake();
        return pComponent;
    }

    public T GetComponent<T>() where T :CustomComponent
    {
        for (Int32 i = 0, iCount = _components.Count; i < iCount; i++)
        {
            if (_components[i] is T)
                return _components[i] as T;
        }

        Debug.LogWarning($"COMPONENTHOLDER :: {typeof(T)} is not found");
        return null;
    }

    public void RemoveComponent(CustomComponent pComponent)
    {
        _components.Remove(pComponent);
        pComponent.OnRemoved();
    }

    #region Implemetation for Foreach

    private Int32 position = -1;

    public IEnumerator<CustomComponent> GetEnumerator()
    {
        for (Int32 i = 0; i < _components.Count; i++)
            yield return _components[position];
    }

    public Boolean MoveNext()
    {
        if (position == _components.Count - 1)
        {
            Reset();
            return false;
        }

        position++;
        return (position < _components.Count);
    }

    public CustomComponent Current => _components[position];

    System.Object IEnumerator.Current => this.Current;

    public void Reset() => position = -1;

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public void Dispose()
    {
        Debug.LogError($"Dispose Called!");
    }

    #endregion
}
