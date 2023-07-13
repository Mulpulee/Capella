using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerEx : MonoBehaviour
{
    [SerializeField] private AudioClip[] BGMs;
    [SerializeField] private AudioClip[] SFXs;
    [SerializeField] public AudioSource BGMsource;

    private GameManagerEx gm;

    private float mstVol = 0;
    private float bgmVol = 0;
    private float sfxVol = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gm = GameObject.FindObjectOfType<GameManagerEx>();
    }

    public void SetMasterVolume(float volume)
    {
        mstVol = volume;
        BGMsource.volume = bgmVol * mstVol;
        gm.SetVolumes(mstVol, bgmVol, sfxVol);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVol = volume;
        BGMsource.volume = bgmVol * mstVol;
        gm.SetVolumes(mstVol, bgmVol, sfxVol);
    }

    public void SetSfxVolume(float volume)
    {
        sfxVol = volume;
        gm.SetVolumes(mstVol, bgmVol, sfxVol);
    }

    public void SetAll(float mst, float bgm, float sfx)
    {
        mstVol = mst;
        bgmVol = bgm;
        sfxVol = sfx;
        gm.SetVolumes(mstVol, bgmVol, sfxVol);
    }

    public void PlayBgm(int index)
    {
        BGMsource.volume = bgmVol * mstVol;
        BGMsource.clip = BGMs[index];
        BGMsource.Play();
    }

    public void StopBgm()
    {
        BGMsource.Stop();
    }

    public void OnSfx(int index)
    {
        StartCoroutine(PlaySfx(index));
    }

    private IEnumerator PlaySfx(int index)
    {
        AudioSource source = transform.AddComponent<AudioSource>();
        source.volume = sfxVol * mstVol;
        source.clip = SFXs[index];
        source.Play();

        while (source.isPlaying) yield return null;

        yield return new WaitForSeconds(1);

        Destroy(source);
    }
}
