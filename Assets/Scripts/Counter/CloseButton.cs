using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    private GameManagerEx gm;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManagerEx>();
    }

    private void OnMouseDown()
    {
        gm.CloseCafe();
    }
}
