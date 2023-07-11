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
    private Inventory inventory;

    private GameObject p;
    private int stackCount = 0;
    private int powderCode = 0;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    private void AddPowder(Ingredient input)
    {
        if (stackCount == 1 && powderCode == input._id) return;

        mouse.RemoveHolding();

        Transform powder = transform.GetChild(stackCount++);
        powder.GetComponent<SpriteRenderer>().color = input._powderColor;

        if (powderCode > input._id) powderCode += input._id * 10;
        else
        {
            powderCode *= 10;
            powderCode += input._id;
        }
    }

    private void Drip()
    {
        mouse.RemoveHolding();

        MenuRow menu;
        if (stackCount == 0)
        {
            menu = DataSystem<MenuRow>.GetRow(10088);
            oil.enabled = true;
            powderCode = 88;
        }
        else 
        {
            menu = DataSystem<MenuRow>.GetRow(10000 + powderCode);
            water.enabled = true;
        }

        p = Instantiate(product);
        Product pp = p.GetComponent<Product>();
        pp._id = menu.ID;
        pp._name = menu.Name;
        pp._description = menu.Description;
        pp._Color = new Color(menu.GasColor_r, menu.GasColor_g, menu.GasColor_b);
        pp._isGas = false;

        p.transform.GetChild(powderCode / 10).GetComponent<SpriteRenderer>().color = Color.white;
        p.transform.GetChild(powderCode % 10).GetComponent<SpriteRenderer>().color = Color.white;

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
            if (info == null) return;

            if (p != null)
            {
                if (info._type == IngredType.etc)
                {
                    inventory.SetSlot(p.GetComponent<Product>(), true);
                    Destroy(p);
                    mouse.RemoveHolding();
                }
                return;
            }
            if (info._type != IngredType.Liquid && info._type != IngredType.etc && stackCount < 2) AddPowder(info);
            else if (info._type == IngredType.Liquid && info._id == 0 && stackCount == 2) Drip();
            else if (info._type == IngredType.Liquid && info._id == 8 && stackCount == 0) Drip();
        }
        else if (p != null)
        {
            if (inventory.GetEmptySlot() == -1) return;

            inventory.SetSlot(p.GetComponent<Product>());
            Destroy(p);
        }
    }
}
