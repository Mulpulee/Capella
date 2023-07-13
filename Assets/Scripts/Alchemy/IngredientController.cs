using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    [SerializeField] private Sprite ingred;

    private MouseManager mouse;
    private SoundManagerEx sm;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
        sm = GameObject.FindObjectOfType<SoundManagerEx>();
    }

    private void OnMouseDown()
    {
        if (mouse.GetHoldingObject(false) == null)
        {
            if (GetComponent<Ingredient>()._type == IngredType.Stone) sm.OnSfx(4);
            if (GetComponent<Ingredient>()._type == IngredType.Plant) sm.OnSfx(5);
            if (GetComponent<Ingredient>()._type == IngredType.Mushroom) sm.OnSfx(5);

            mouse.SetHoldingObject(transform.gameObject, true, ingred);
        }
        else
        {
            if (GetComponent<Ingredient>()._type == IngredType.Stone) sm.OnSfx(4);
            if (GetComponent<Ingredient>()._type == IngredType.Plant) sm.OnSfx(5);
            if (GetComponent<Ingredient>()._type == IngredType.Mushroom) sm.OnSfx(5);

            if (mouse.GetHoldingObject(false).CompareTag(transform.tag)) mouse.RemoveHolding();
        }
    }
}
