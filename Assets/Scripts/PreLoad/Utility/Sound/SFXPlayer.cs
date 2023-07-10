using System;
using System.Collections.Generic;
using UnityEngine;


public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip m_audioClip;

    private void Awake()
    {
        SoundManager.ins.PlaySFX(m_audioClip);
    }
}