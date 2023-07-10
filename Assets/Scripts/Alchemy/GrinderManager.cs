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
    private int GrindGrade;
    private bool left;

    private void Start()
    {
        mouse = GameObject.FindObjectOfType<MouseManager>();
        canGrind = true;
        GrindGrade = 0;
        left = true;
    }

    private void SetGrinding(Ingredient ingred)
    {
        if (!canGrind) return;

        grinding = ingred;
        canGrind = false;
    }

    private void Grind()
    {
        Debug.Log("갈"); 

        GrindGrade++;
        left = !left;

        if(GrindGrade == 5)
        {
            p = Instantiate(powder);
            Ingredient inp = p.GetComponent<Ingredient>();
            inp._type = grinding._type;
            inp._name = grinding._name;
            inp._description = grinding._description;
            inp._powderColor = grinding._powderColor;

            inp._grinded = true;

            p.GetComponent<SpriteRenderer>().color = inp._powderColor;

            grinding = null;
        }
    }

    private void OnMouseDown()
    {
        if(mouse.GetHoldingObject(false) != null && canGrind)
            if(mouse.GetHoldingInfo()._type != IngredType.Liquid && mouse.GetHoldingInfo()._type != IngredType.etc)
                SetGrinding(mouse.GetHoldingObject(true).GetComponent<Ingredient>());

        if (!mouse.GetHoldingObject(false) && GrindGrade == 5)
            mouse.SetHoldingObject(p, false);
    }

    private void OnMouseDrag()
    {
        if(!canGrind && GrindGrade < 5)
        {
            float distance = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;

            if (left && distance < -1) Grind();
            if (!left && distance > 1) Grind();
        }
    }
}
