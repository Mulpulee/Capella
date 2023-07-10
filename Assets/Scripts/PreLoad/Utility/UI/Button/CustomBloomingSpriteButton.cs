using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomBloomingSpriteButton : CustomSpriteButton
{
    [SerializeField] private AudioClip MouseOverSound;

    private Vector3 orgScale;
    private void Start()
    {
        orgScale = transform.localScale;
        if (MouseOverSound == null)
            MouseOverSound = PreloadingManager.Settings.ButtonMouseOverSound;
    }
    public override void OnEnter()
    {
        Vector3 newScale = orgScale * 1.08f;
        transform.DOKill();
        OnMouseOverSound();
        transform.DOScale(newScale, 0.3f);

        if (MouseOverSound == null)
            return;

        SoundManager.ins.PlaySFX(MouseOverSound?.name);
    }
    public override void OnExit()
    {
        transform.DOKill();
        transform.DOScale(orgScale, 0.3f);
    }

    protected virtual void OnMouseOverSound()
    {
        if (MouseOverSound == null)
            return;

        SoundManager.ins.PlaySFX(MouseOverSound?.name);
    }

}
