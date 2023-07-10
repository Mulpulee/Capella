using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class BoilerManager : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject oil;
    [SerializeField] private GameObject product;

    private MouseManager mouse;
    private GameObject p;
    private int stackCount = 0;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
    }

    private void AddPowder(Ingredient input)
    {
        if (stackCount == 1 && transform.GetChild(0).GetComponent<Ingredient>()._id == input._id) return;

        mouse.RemoveHolding();

        Transform powder = transform.GetChild(stackCount++);
        powder.GetComponent<SpriteRenderer>().color = input._powderColor;
        powder.GetComponent<Ingredient>().SetData(input);
    }

    private void Drip()
    {
        if (stackCount == 0)
        {
            p = Instantiate(product);
            p.GetComponent<Product>();

            return;
        }


    }

    private void OnMouseDown()
    {
        GameObject holding = mouse.GetHoldingObject(false);

        if (holding != null)
        {
            Ingredient info = mouse.GetHoldingInfo();

            if (info._type != IngredType.Liquid && info._type != IngredType.etc && stackCount < 2) AddPowder(info);
            else if (info._type == IngredType.Liquid && info._id == 0 && stackCount == 2) Drip();
            else if (info._type == IngredType.Liquid && info._id == 8 && stackCount == 0) Drip();
        }

    }
}
