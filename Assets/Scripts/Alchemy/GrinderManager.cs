using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderManager : MonoBehaviour
{
    [SerializeField] private GameObject powder;

    private MouseManager mouse;
    private GameObject p;

    private Ingredient grinding;
    private bool canGrind;
    private int grindGrade;
    private bool left;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
        ResetGrinder();
    }

    private void SetGrinding(Ingredient ingred)
    {
        if (!canGrind) return;

        grinding = ingred;
        canGrind = false;
    }

    private void Grind()
    {
        grindGrade++;
        left = !left;

        if(grindGrade == 5)
        {
            p = Instantiate(powder);
            p.GetComponent<Ingredient>().SetData(grinding);

            p.GetComponent<Ingredient>()._grinded = true;

            p.GetComponent<SpriteRenderer>().color = grinding._powderColor;

            grinding = null;
        }
    }

    private void ResetGrinder()
    {
        grinding = null;
        canGrind = true;
        grindGrade = 0;
        left = true;
    }

    private void OnMouseDown()
    {
        GameObject holding = mouse.GetHoldingObject(false);

        if (holding != null && canGrind)
        {
            Ingredient info = mouse.GetHoldingInfo();
            if (info == null) return;

            if (info._type != IngredType.Liquid && info._type != IngredType.etc && !info._grinded)
                SetGrinding(mouse.GetHoldingObject(true).GetComponent<Ingredient>());
        }

        if (holding == null && grindGrade == 5)
        {
            mouse.SetHoldingObject(p, false);
            ResetGrinder();
        }
    }

    private void OnMouseDrag()
    {
        if(!canGrind && grindGrade < 5)
        {
            float distance = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;

            if (left && distance < -1) Grind();
            if (!left && distance > 1) Grind();
        }
    }
}
