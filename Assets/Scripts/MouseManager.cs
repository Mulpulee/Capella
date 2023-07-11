using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private GameObject holding;

    public void SetHoldingObject(GameObject go, bool clone = true)
    {
        if (holding == null)
        {
            GameObject g;
            if (clone) g = Instantiate(go);
            else g = go;

            Destroy(g.GetComponent<BoxCollider2D>());
            Destroy(g.GetComponent<IngredientController>());
            if (g.GetComponent<SpriteRenderer>()) g.GetComponent<SpriteRenderer>().sortingOrder = 100;
            holding = g;
            

            holding.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
        }

        return;
    }

    public void RemoveHolding()
    {
        if(holding != null)
        {
            Destroy(holding);
        }

        return;
    }

    public GameObject GetHoldingObject(bool destroy = false)
    {
        if (holding == null) return null;

        if (destroy) RemoveHolding();
        return holding;
    }

    public Ingredient GetHoldingInfo()
    {
        if (holding == null) return null;

        return holding.GetComponent<Ingredient>();
    }

    public Product GetHoldingProduct()
    {
        if (holding == null) return null;

        return holding.GetComponent<Product>();
    }

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
