using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour, IPoolable
{
    private AudioSource _source;
    private Coroutine _soundPlayRoutine;

    private Action _returnCallback;
    private Action _onPlayFinished;

    public AudioSource Source => _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void ChangeVolume(Single pVolume)
    {
        _source.volume = pVolume;
    }

    public void AddCallback(Action pCallback)
    {
        if (_onPlayFinished == null)
            _onPlayFinished = pCallback;
        else
            _onPlayFinished += pCallback;
    }

    public void PlaySound(AudioClip pClip,Single pPitch,Action pCallback = null)
    {
        _source.clip = pClip;
        _source.pitch = pPitch;
        _source.Play();
        _soundPlayRoutine = StartCoroutine(SoundPlayRoutine(pClip.length,pCallback));
    }

    public void PlaySoundFrom(AudioClip pClip,Single pPitch,Transform pParent, Action pCallback = null)
    {
        transform.SetParent(pParent);
        transform.localPosition = new Vector3(0, 0, 0);
        PlaySound(pClip, pPitch,pCallback);
    }

    public void SetRelease(Action pRelease)
    {
        _returnCallback = pRelease;
    }

    private IEnumerator SoundPlayRoutine(Single pDelay, Action pCallback)
    {
        yield return new WaitForSeconds(pDelay);
        _returnCallback.Invoke();
        pCallback?.Invoke();
        _onPlayFinished?.Invoke();
        _onPlayFinished = null;
    }

}
