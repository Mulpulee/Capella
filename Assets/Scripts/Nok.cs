using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nok : MonoBehaviour
{
    private SoundManagerEx sm;

    private void Start()
    {
        sm = GameObject.FindObjectOfType<SoundManagerEx>();
    }

    public void Back()
    {
        sm.OnSfx(11);
        Destroy(transform.parent.gameObject);
    }
}
