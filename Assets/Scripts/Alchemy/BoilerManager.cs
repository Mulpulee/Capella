using Automation.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class BoilerManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer water;
    [SerializeField] private SpriteRenderer oil;
    [SerializeField] private GameObject product;

    private MouseManager mouse;
    private GameObject p;
    private int stackCount = 0;
    private int powderCode = 0;

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

        if (powderCode > input._id) powderCode += input._id * 10;
        else
        {
            powderCode *= 10;
            powderCode += input._id;
        }
    }

    private void Drip()
    {
        if (stackCount == 0) oil.enabled = true;
        else water.enabled = true;

        mouse.RemoveHolding();

        MenuRow menu;
        if (stackCount == 0) menu = DataSystem<MenuRow>.GetRow(10088);
        else menu = DataSystem<MenuRow>.GetRow(10000 + powderCode);

        p = Instantiate(product);
        Product pp = p.GetComponent<Product>();
        pp._name = menu.Name;
        pp._description = menu.Description;
        pp._ingreds = new int[2];
        pp._ingreds[0] = menu.Ingredient1;
        pp._ingreds[1] = menu.Ingredient2;
        pp._gasColor = new Color(menu.GasColor_r, menu.GasColor_g, menu.GasColor_b);
        pp._isGas = false;

        ResetBoiler();

        return;
    }

    private void ResetBoiler()
    {
        stackCount = 0;
        powderCode = 0;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.clear;
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
