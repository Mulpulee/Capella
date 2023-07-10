using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    [SerializeField] private AudioClip MouseClickSound = null;
    [HideInInspector] public Image image;

    protected Button _button;

    public UnityEvent onClick
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();

            return _button.onClick;
        }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        image = GetComponent<Image>();
        Init();
    }

    protected virtual void Init()
    {
        Navigation customNav = new Navigation();
        customNav.mode = Navigation.Mode.None;
        _button.navigation = customNav;
        _button.onClick.AddListener(PlayButtonSound);
        if (MouseClickSound == null)
            MouseClickSound = PreloadingManager.Settings.ButtonClickSound;
    }

    public void RemoveAllListeners()
    {
        onClick.RemoveAllListeners();
        onClick.AddListener(PlayButtonSound);
    }

    public void AddListener(Action pAction)
    {
        onClick.AddListener(pAction.Invoke);
    }

    protected virtual void PlayButtonSound()
    {
        if (MouseClickSound == null)
            return;

        SoundManager.ins.PlaySFX(MouseClickSound?.name);
    }
}
