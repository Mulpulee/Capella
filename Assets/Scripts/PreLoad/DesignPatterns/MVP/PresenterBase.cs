using System;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public abstract class PresenterBase<TView> : Presenter where TView : ViewBase
{
    private TView __view;
    protected TView _view
    {
        get
        {
            if (__view == null)
                __view = GetComponent<TView>();
            return __view;
        }
    }
    public override void InitPresenter()
    {
        if (__view == null)
            __view = GetComponent<TView>();
    }

    public override bool GetViewFlag(ViewOptions pOptions) => _view.options.HasFlag(pOptions);

    protected void OpenView(Single pDuration = -1)
    {
        if (_view.isStacking)
        {
            UIInputManager.ins.AddPresenter(this);
        }
        _view.OpenView(pDuration);
    }

    protected void CloseView(Single pDuration = -1)
    {
        if (_view.isStacking)
        {
            UIInputManager.ins.RemovePresenter(this);
        }

        _view.CloseView(pDuration);
    }

    public override void Show()
    {
        _view.OpenView(0);
    }

    public override void Hide()
    {
        _view.CloseView(0);
    }
}