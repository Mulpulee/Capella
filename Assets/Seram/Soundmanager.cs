using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soundmanager : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;
    public Slider masterslider;

    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume * masterslider.value;
    }

    public void SetButtonMusicVolume(float volume)
    {
        btnsource.volume = volume * masterslider.value;
    }

    public void SetMasterVolume(float volume)
    {
        musicsource.volume = volume * masterslider.value;
        btnsource.volume = volume * masterslider.value;  
    }

    public void OnSfx()
    {
        btnsource.Play();
    }

}
