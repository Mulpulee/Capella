using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameObject[] slots;
    private bool[] isEmpty;

    private Product[] products;

    private MouseManager mouse;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();

        slots = new GameObject[3];
        slots[0] = transform.GetChild(0).gameObject;
        slots[1] = transform.GetChild(1).gameObject;
        slots[2] = transform.GetChild(2).gameObject;

        isEmpty = new bool[3];
        for (int i = 0; i < 3; i++) isEmpty[i] = true;

        products = new Product[3];
    }

    public int GetEmptySlot()
    {
        for (int i = 0; i < 3; i++)
            if (isEmpty[i]) return i;

        return -1;
    }

    public void SetSlot(Product product, bool isGas = false)
    {
        if (GetEmptySlot() == -1) return;

        int emptySlot = GetEmptySlot();
            
        if (isGas)
        {
            product._isGas = true;
            slots[emptySlot].transform.GetChild(1).GetComponent<SpriteRenderer>().color = product._Color;
        }
        else
        {
            Transform drink = slots[emptySlot].transform.GetChild(0);
            drink.GetComponent<SpriteRenderer>().color = product._Color;

            drink.GetChild((product._id - 10000) / 10).GetComponent<SpriteRenderer>().color = Color.white;
            drink.GetChild(product._id % 10).GetComponent<SpriteRenderer>().color = Color.white;
        }
        products[emptySlot] = product;
        slots[emptySlot].GetComponent<Product>().SetData(product);

        isEmpty[emptySlot] = false;
    }

    public void RemoveSlot(int index)
    {
        if (isEmpty[index]) return;

        products[index] = null;

        Transform slot = slots[index].transform;
        Transform drink = slot.GetChild(0);

        drink.GetComponent<SpriteRenderer>().color = Color.clear;
        slot.GetChild(1).GetComponent<SpriteRenderer>().color = Color.clear;


        for(int i = 0; i < 9; i++) drink.GetChild(i).GetComponent<SpriteRenderer>().color = Color.clear;

        isEmpty[index] = true;

        return;
    }

    public void ClickSlot(int index)
    {
        if (isEmpty[index]) return;

        GameObject slot = slots[index];

        if (slot.activeSelf && mouse.GetHoldingObject() == null)
        {
            mouse.SetHoldingObject(slot);
            slot.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (mouse.GetHoldingProduct() == null) return;

        for(int i = 0; i < 3; i++)
        {
            if (!slots[i].activeSelf && mouse.GetHoldingProduct()._id == products[i]._id)
            {
                mouse.RemoveHolding();
                slots[i].SetActive(true);
                return;
            }
        }
    }
}
