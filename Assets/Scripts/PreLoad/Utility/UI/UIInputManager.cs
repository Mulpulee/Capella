using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInputManager : IndestructibleSingleton<UIInputManager>
{
    [SerializeField] private ListStack<Presenter> _presenterStack;
    private Action _onEmptyStackCallback;

    protected override void OnSingletonInstantiated()
    {
        _presenterStack = new ListStack<Presenter>();
        SceneLoader.onSceneLoaded += Clear;
    }
    public void Clear()
    {
        _presenterStack.Clear();
        _onEmptyStackCallback = null;
    }
    public void AddPresenter(Presenter pPresenter)
    {
        if (pPresenter.GetViewFlag(ViewOptions.isClosingOther) && pPresenter.GetViewFlag(ViewOptions.isStacking))
        {
            if (_presenterStack.Count != 0)
                _presenterStack.Peek().Hide();
        }

        _presenterStack.Push(pPresenter);
    }
    public void RemovePresenter(Presenter pPresenter)
    {
        if (pPresenter == null)
            return;

        Boolean isTopFlag = false;

        if (pPresenter.GetViewFlag(ViewOptions.isClosingOther) && pPresenter.GetViewFlag(ViewOptions.isStacking))
        {
            if (_presenterStack.Peek() == pPresenter)
            {
                isTopFlag = true;
            }
        }

        _presenterStack.Remove(pPresenter);

        if (isTopFlag)
        {
            if (_presenterStack.Count != 0)
                _presenterStack.Peek().Show();
        }
    }

    public void AssignEmptyStackCallback(Action pAction)
    {
        _onEmptyStackCallback = pAction;
    }
    public void RemoveEmptyStackCallback()
    {
        _onEmptyStackCallback = null;
    }

    private void PopPresenter()
    {
        if (_presenterStack.Count == 0)
            return;

        var presenter = _presenterStack.Peek();

        if (!presenter.GetViewFlag(ViewOptions.isEscapeClosing))
            return;

        var target = presenter;
        target.Release();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_presenterStack.Count == 0)
            {
                _onEmptyStackCallback?.Invoke();
            }
            else
            {
                PopPresenter();
            }
        }
    }
}