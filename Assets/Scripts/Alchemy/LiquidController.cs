using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidController : MonoBehaviour
{
    private MouseManager mouse;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
    }

    private void OnMouseDown()
    {
        if (mouse.GetHoldingObject(false) == null)
        {
            mouse.SetHoldingObject(transform.gameObject);
            transform.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (mouse.GetHoldingObject(false).CompareTag(transform.tag))
        {
            transform.GetComponent<SpriteRenderer>().enabled = true;
            mouse.RemoveHolding();
        }
    }
}
