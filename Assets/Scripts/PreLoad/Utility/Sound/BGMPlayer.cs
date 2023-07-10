using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private Boolean m_stopWhenDestroy;
    
    void Start() => SoundManager.ins.PlayBGM(_audioClip.name);

    private void OnDestroy()
    {
        if(m_stopWhenDestroy)
            SoundManager.ins.StopBGM();
    }
}
