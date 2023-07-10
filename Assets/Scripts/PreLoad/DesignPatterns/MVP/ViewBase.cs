using System;
using UnityEngine;
using Extensions;
using UnityEngine.UI;

[Flags]
public enum ViewOptions
{
    isEscapeClosing = 1,
    isStacking = 2,
    autoHideAtStart = 4,
    isClosingOther = 8,
}

public abstract class ViewBase : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField] protected Single transitionDuration = 0.25f;
    [SerializeField, EnumFlags] public ViewOptions options;

    public Boolean isEscapeClosing
    {
        get => (options & ViewOptions.isEscapeClosing) == ViewOptions.isEscapeClosing;
        set
        {
            if (value)
                options |= ViewOptions.isEscapeClosing;
            else
                options &= ~ViewOptions.isEscapeClosing;
        }
    }
    public Boolean isStacking => options.HasFlag(ViewOptions.isStacking);
    public Boolean isAutoHiding => options.HasFlag(ViewOptions.autoHideAtStart);

    public Boolean isViewActivated => _canvasGroup.interactable;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (isAutoHiding)
            CloseView(0);
        InitView();
    }

    public void CloseView(Single pDuration = -1, UITransitionType pType = UITransitionType.Scale, Action pCallback = null)
    {
        pDuration = pDuration == -1 ? transitionDuration : pDuration;

        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.Hide(pDuration, pType, pCallback);
    }
    public void OpenView(Single pDuration = -1, UITransitionType pType = UITransitionType.Scale, Action pCallback = null)
    {
        pDuration = pDuration == -1 ? transitionDuration : pDuration;

        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.Show(pDuration, pType, pCallback);
    }

    protected virtual void InitView() { }
}