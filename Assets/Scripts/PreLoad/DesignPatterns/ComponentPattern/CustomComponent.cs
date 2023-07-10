using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomComponent 
{
    private Boolean _isActivated;
    protected GameObject _componentHolderGameObject;
    protected ComponentHolder _componentHolder;

    public Boolean IsActivetd => _isActivated;

    public CustomComponent(GameObject pGameObject)
    {
        _componentHolderGameObject = pGameObject;
        _isActivated = true;
    }

    public CustomComponent InitComponent(ComponentHolder pComponentHolder)
    {
        _componentHolder = pComponentHolder;
        return this;
    }

    public void SetActive(Boolean pValue) => _isActivated = pValue;

    public abstract void OnAwake();
    public abstract void OnUpdate();
    public virtual void OnFixedUpdate() { }
    public virtual void OnRemoved() { }
}
