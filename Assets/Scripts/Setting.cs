using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    private GameManagerEx gm;
    private SoundManagerEx sm;

    // master(2) bgm(3) sfx(4)
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        gm = GameObject.FindObjectOfType<GameManagerEx>();
        sm = GameObject.FindObjectOfType<SoundManagerEx>();

        gm.isSetting = true;

        Playerdata data = gm.GetData();

        transform.GetChild(2).GetComponent<Slider>().value = data.mstVolume;
        transform.GetChild(3).GetComponent<Slider>().value = data.bgmVolume;
        transform.GetChild(4).GetComponent<Slider>().value = data.sfxVolume;

        transform.GetChild(2).GetComponent<Slider>().onValueChanged.AddListener(volume => sm.SetMasterVolume(volume));
        transform.GetChild(3).GetComponent<Slider>().onValueChanged.AddListener(volume => sm.SetBGMVolume(volume));
        transform.GetChild(4).GetComponent<Slider>().onValueChanged.AddListener(volume => sm.SetSfxVolume(volume));
    }

    public void Back()
    {
        gm.isSetting = false;
        Destroy(gameObject);
    }

    public void Title()
    {
        gm.isSetting = false;
        gm.GotoTitle();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
}
