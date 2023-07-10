using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomSpriteButton : MonoBehaviour, IPointerClickHandler,
                                  IPointerDownHandler, IPointerEnterHandler,
                                  IPointerUpHandler, IPointerExitHandler
{
    public SpriteRenderer spriteRenderer;

    [SerializeField] private AudioClip MouseClickSound = null;


    public UnityEvent<System.Action> onClick;
    public UnityEvent<System.Action> onMouseEnter;
    public UnityEvent<System.Action> onMouseExit;

    public bool isActivated;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActivated = true;

        if (MouseClickSound == null)
            MouseClickSound = PreloadingManager.Settings.ButtonClickSound;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isActivated)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onClick?.Invoke(null);
            PlayButtonSound();
            OnPress();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isActivated)
            return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActivated)
            return;

        onMouseEnter?.Invoke(null);
        OnEnter();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isActivated)
            return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActivated)
            return;

        onMouseExit?.Invoke(null);
        OnExit();
    }

    public void RemoveAllListeners()
    {
        onClick.RemoveAllListeners();
    }

    public void AddListener(Action pAction)
    {
        onClick.AddListener(temp => pAction.Invoke());
    }

    protected virtual void PlayButtonSound()
    {
        if (MouseClickSound == null)
            return;

        SoundManager.ins.PlaySFX(MouseClickSound?.name);
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnPress() { }

}
