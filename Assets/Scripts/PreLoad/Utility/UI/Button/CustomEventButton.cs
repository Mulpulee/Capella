using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum EventType
{
    LeftClick = 0,
    RightClick = 1,
    MiddleClick = 2,
    PointerEnter = 3,
    PointerExit = 4
}


public class CustomEventButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,IPointerExitHandler
{
    private Image _image;
    private UnityEvent[] _events;
    private Collider _collider;

    public Image Image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();
            return _image;
        }
    }

    private void Init()
    {
        _events = new UnityEvent[5];
    }

    public void RemoveAllListener(EventType pButton)
    {
        if (_events == null)
            return;

        _events[(Int32)pButton]?.RemoveAllListeners();
    }

    public void AddListener(EventType pButton, Action pAction)
    {
        if (_events == null)
            Init();

        if (_events[(Int32)pButton] == null)
            _events[(Int32)pButton] = new UnityEvent();

        _events[(Int32)pButton].AddListener(() => pAction.Invoke());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_events == null)
            return;

        _events[(Int32)eventData.button]?.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_events == null)
            return;
        _events[(Int32)EventType.PointerExit]?.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_events == null)
            return;
        _events[(Int32)EventType.PointerEnter]?.Invoke();
    }

}
