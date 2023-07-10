using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


[RequireComponent(typeof(Button))]
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(CanvasGroup))]
public class CustomBloomingButton : CustomButton
{

    [SerializeField] private AudioClip MouseOverSound;
    [SerializeField] private float Scale = 1.08f;
    [SerializeField] private float ScaleDuration = 0.3f;

    private EventTrigger _eventTrigger;
    private Vector3 _orgScale;
    private CanvasGroup _canvasGroup;

    public Action<CustomBloomingButton> OnMouseEnterAction;
    public Action<CustomBloomingButton> OnMouseExitAction;

    public CanvasGroup canvasGroup
    {
        get
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;
        }
    }

    public Boolean interactable
    {
        get => _button.interactable;
        set => _button.interactable = value;
    }

    protected override void Init()
    {
        base.Init();

        _orgScale = transform.localScale;

        _eventTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener((eventData) => { MouseOver(); });

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((eventData) => { MouseExit(); });

        _eventTrigger.triggers.Add(enter);
        _eventTrigger.triggers.Add(exit);

        if (MouseOverSound == null)
            MouseOverSound = PreloadingManager.Settings.ButtonMouseOverSound;
    }

    public void MouseOver()
    {
        if (!_button.interactable)
            return;

        OnMouseOverSound();
        Vector3 newScale = _orgScale * Scale;
        transform.DOKill();
        transform.DOScale(newScale, ScaleDuration);
        OnMouseEnterAction?.Invoke(this);
    }

    public void MouseExit()
    {
        if (!_button.interactable)
            return;

        OnMouseExitSound();
        transform.DOKill();
        transform.DOScale(_orgScale, ScaleDuration);
        OnMouseExitAction?.Invoke(this);
    }

    protected virtual void OnMouseOverSound()
    {
        if (MouseOverSound == null)
            return;

        SoundManager.ins.PlaySFX(MouseOverSound?.name);
    }

    protected virtual void OnMouseExitSound()
    {

    }

}
