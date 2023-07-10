using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : IndestructibleSingleton<SoundManager>
{
    private static readonly String SFXVolumeKey = "SFXVolume";
    private static readonly String BGMVolumeKey = "BGMVolume";

    private Single _sfxVolume;
    private Single _bgmVolume;

    private AudioSource _bgmSource;
    private AudioSource _bgmSource2;
    private AudioSource _nowBGMSource;

    private Dictionary<String, AudioClip> _soundByName;
    private UnityObjectBag<SoundPlayer> _soundPlayerPool;

    private List<SoundPlayer> _activatedSoundPlayer;

    public Single SFXVolume => _sfxVolume;
    public Single BGMVolume => _bgmVolume;

    protected override void OnSingletonInstantiated()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;

        _bgmSource2 = gameObject.AddComponent<AudioSource>();
        _bgmSource2.loop = true;

        _soundByName = new Dictionary<String, AudioClip>();
        _soundPlayerPool = new UnityObjectBag<SoundPlayer>(InstantiateSoundPlayer, temp => { }, 100, transform);
        _activatedSoundPlayer = new List<SoundPlayer>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");

        foreach (var clip in clips)
        {
            _soundByName.Add(clip.name, clip);
        }

        LoadVolumes();
    }
    private SoundPlayer InstantiateSoundPlayer()
    {
        GameObject instance = new GameObject("SoundPlayer");
        instance.AddComponent<AudioSource>();
        SoundPlayer player = instance.AddComponent<SoundPlayer>();
        player.ChangeVolume(_sfxVolume);
        return player;
    }

    public SoundPlayer PlaySFX(AudioClip pClip,Single pPitch = 1f,Action pCallback = null)
    {
        var player = _soundPlayerPool.GetObject();
        player.transform.SetParent(transform);
        player.ChangeVolume(_sfxVolume);
        player.PlaySound(pClip, pPitch, pCallback);
        player.AddCallback(() => _activatedSoundPlayer.Remove(player));
        player.Source.spatialBlend = 0.0f;

        _activatedSoundPlayer.Add(player);

        return player;
    }
    public SoundPlayer PlaySFX(String pSoundName, Single pPitch = 1f)
    {
        if(_soundByName.TryGetValue(pSoundName,out AudioClip clip))
            return PlaySFX(clip, pPitch);

        return null;
    }



    public SoundPlayer PlaySFXFrom(AudioClip pClip,Transform pFrom,Single pPitch = 1.0f)
    {
        SoundPlayer player = PlaySFX(pClip, pPitch);
        player.transform.SetParent(pFrom);
        player.transform.localPosition = new Vector3(0, 0, 0);
        player.AddCallback(() => player.transform.SetParent(pFrom));
        player.Source.spatialBlend = 1.0f;
        return player;
    }
    public SoundPlayer PlaySFXFrom(String pSoundName,Transform pFrom,Single pPitch = 1.0f)
    {
        if (_soundByName.TryGetValue(pSoundName, out AudioClip clip))
            return PlaySFXFrom(clip,pFrom, pPitch);

        Debug.LogError($"SOUNDMANAGER :: {pSoundName} is Unknown key");
        return null;
    }

    public SoundPlayer PlaySFXFrom(AudioClip pClip, Vector3 pFrom, Single pPitch = 1.0f)
    {
        SoundPlayer player = PlaySFX(pClip, pPitch);
        player.transform.position = pFrom;
        player.Source.spatialBlend = 0.5f;
        return player;
    }

    public SoundPlayer PlaySFXFrom(String pSoundName, Vector3 pFrom, Single pPitch = 1.0f)
    {
        if (_soundByName.TryGetValue(pSoundName, out AudioClip clip))
            return PlaySFXFrom(clip, pFrom, pPitch);

        Debug.LogError($"SOUNDMANAGER :: {pSoundName} is Unknown key");
        return null;
    }
    public void PlayBGM(AudioClip pClip)
    {
        if(_nowBGMSource == null)
        {
            _nowBGMSource = _bgmSource2;
        }

        if (_nowBGMSource.clip == pClip)
            return;
        else
        {
            AudioSource nowPlayingSource = _nowBGMSource;
            AudioSource nonPlayingSource = _bgmSource.isPlaying ? _bgmSource2 : _bgmSource;

            _nowBGMSource = nonPlayingSource;

            nonPlayingSource.volume = 0;
            nonPlayingSource.clip = pClip;
            nonPlayingSource.Play();

            DOTween.To(() => nowPlayingSource.volume, x => nowPlayingSource.volume = x, 0, 3f).OnComplete(() =>
            {
                nowPlayingSource.Stop();
            });

            DOTween.To(() => nonPlayingSource.volume, x => nonPlayingSource.volume = x, _bgmVolume, 3f);
        }
    }
    public void PlayBGM(String pBGMName)
    {
        if (_soundByName.TryGetValue(pBGMName, out AudioClip clip))
        {
            PlayBGM(clip);
            return;
        }

        Debug.LogError($"SOUNDMANAGER :: {pBGMName} is Unknown key");
    }
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    public void OnBGMVolumeChange(Single pVolume)
    {
        _bgmSource.volume = pVolume;
        _bgmSource2.volume = pVolume;
        _bgmVolume = pVolume;

        SaveVolumes();
    }
    public void OnSFXVolumeChange(Single pVolume)
    {
        foreach (var item in _activatedSoundPlayer)
            item.ChangeVolume(pVolume);

        _sfxVolume = pVolume;

        SaveVolumes();
    }

    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat(SFXVolumeKey, _sfxVolume);
        PlayerPrefs.SetFloat(BGMVolumeKey, _bgmVolume);
    }
    private void LoadVolumes()
    {
        if (PlayerPrefs.HasKey(SFXVolumeKey))
            _sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey);
        else
            _sfxVolume = 0.5f;

        if (PlayerPrefs.HasKey(BGMVolumeKey))
            _bgmVolume = PlayerPrefs.GetFloat(BGMVolumeKey);
        else
            _bgmVolume = 0.1f;

        OnSFXVolumeChange(_sfxVolume);
        OnBGMVolumeChange(_bgmVolume);
    }
}
