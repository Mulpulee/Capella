using System;
using UnityEngine;

public abstract class Presenter : MonoBehaviour
{
    private void Awake() => InitPresenter();

    public abstract void InitPresenter();

    public abstract Boolean GetViewFlag(ViewOptions pOptions);

    public abstract void Bind();
    public abstract void Release();

    public abstract void Show();
    public abstract void Hide();
}