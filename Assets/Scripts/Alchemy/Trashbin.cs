using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbin : MonoBehaviour
{
    private MouseManager mouse;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
    }

    private void OnMouseDown()
    {
        if(mouse.GetHoldingObject())
        {
            Ingredient info = mouse.GetHoldingInfo();

            if (info == null) return;

            if (info._grinded) mouse.RemoveHolding();
        }
    }
}
