using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    [SerializeField] private Sprite ingred;

    private MouseManager mouse;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
    }

    private void OnMouseDown()
    {
        if (mouse.GetHoldingObject(false) == null) mouse.SetHoldingObject(transform.gameObject, true, ingred);
        else
        {
            if (mouse.GetHoldingObject(false).CompareTag(transform.tag)) mouse.RemoveHolding();
        }
    }
}
