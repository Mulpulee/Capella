using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Vector3 openPos;
    [SerializeField] private Vector3 closePos;

    private GameObject[] slots;
    private bool[] isEmpty;
    private int isHolding;

    private Product[] products;

    private MouseManager mouse;
    private SoundManagerEx sm;
    private BoxCollider2D col;

    private bool isClosed = true;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
        sm = GameObject.FindObjectOfType<SoundManagerEx>();
        col = GetComponent<BoxCollider2D>(); SetCollider(true);

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

        sm.OnSfx(7);
        int emptySlot = GetEmptySlot();

        SpriteRenderer drink;
        if (isGas)
        {
            product._isGas = true;
            drink = slots[emptySlot].transform.GetChild(1).GetComponent<SpriteRenderer>();
        }
        else
        {
            drink = slots[emptySlot].transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        drink.sprite = product._sprite;
        drink.color = Color.white;
        products[emptySlot] = product;
        slots[emptySlot].GetComponent<Product>().SetData(product);

        isEmpty[emptySlot] = false;
    }

    public void UseSlot()
    {
        mouse.RemoveHolding();
        slots[isHolding].SetActive(true);
        SetCollider(true);
        RemoveSlot(isHolding);
        isHolding = -1;
    }

    public void RemoveSlot(int index)
    {
        if (isEmpty[index]) return;

        products[index] = null;

        Transform slot = slots[index].transform;
        Transform drink = slot.GetChild(0);

        drink.GetComponent<SpriteRenderer>().color = Color.clear;
        slot.GetChild(1).GetComponent<SpriteRenderer>().color = Color.clear;

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
            isHolding = index;
            slot.SetActive(false);
            SetCollider(false);
        }
    }

    private void SetCollider(bool handle)
    {
        if(handle)
        {
            col.offset = new Vector2(0.2f, 1.6f);
            col.size = new Vector2(6.5f, 1f);
        }
        else
        {
            col.offset = new Vector2(0.2f, -0.4f);
            col.size = new Vector2(9f, 3f);
        }
    }

    private void OnMouseDown()
    {
        if(mouse.GetHoldingObject() == null)
        {
            sm.OnSfx(11);
            if (isClosed)
            {
                isClosed = false;
                StartCoroutine(MoveCoroutine(openPos, 50f));
            }
            else
            {
                isClosed = true;
                StartCoroutine(MoveCoroutine(closePos, 50f));
            }
        }

        if (mouse.GetHoldingProduct() == null) return;

        for(int i = 0; i < 3; i++)
        {
            if (!slots[i].activeSelf && mouse.GetHoldingProduct()._id == products[i]._id)
            {
                mouse.RemoveHolding();
                isHolding = -1;
                slots[i].SetActive(true);
                SetCollider(true);
                return;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            sm.OnSfx(11);
            if (isClosed)
            {
                isClosed = false;
                StartCoroutine(MoveCoroutine(openPos, 50f));
            }
            else
            {
                isClosed = true;
                StartCoroutine(MoveCoroutine(closePos, 50f));
            }
        }
    }

    private IEnumerator MoveCoroutine(Vector3 pos, float duration)
    {
        float dir = (pos.y - transform.localPosition.y) / duration;
        while ((dir > 0 && transform.localPosition.y <= pos.y)
            || (dir < 0 && transform.localPosition.y >= pos.y))
        {
            transform.localPosition += new Vector3(0, dir, 0);
            yield return null;
        }

        transform.localPosition = pos;

        yield return null;
    }
}
