using Automation.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilerManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer water;
    [SerializeField] private SpriteRenderer oil;
    [SerializeField] private GameObject product;

    private GameManagerEx gm;
    private MouseManager mouse;
    private Inventory inventory;

    private GameObject p;
    private Dictionary<string, Sprite> drinkSprites;
    private Dictionary<string, Sprite> gasSprites;
    private int stackCount = 0;
    private int powderCode = 0;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManagerEx>();
        mouse = GameObject.FindObjectOfType<MouseManager>();
        inventory = GameObject.FindObjectOfType<Inventory>();

        drinkSprites = new Dictionary<string, Sprite>();
        gasSprites = new Dictionary<string, Sprite>();
        LoadSprites();
    }

    private void LoadSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Arts/Drinks");
        foreach (Sprite sprite in sprites) drinkSprites.Add(sprite.name, sprite);
        sprites = Resources.LoadAll<Sprite>("Arts/Gases");
        foreach (Sprite sprite in sprites) gasSprites.Add(sprite.name, sprite);
    }

    private void AddPowder(Ingredient input)
    {
        if (!input._grinded) return;

        if (stackCount == 1 && powderCode == input._id) return;

        mouse.RemoveHolding();
        stackCount++;

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
        pp._sprite = drinkSprites[powderCode.ToString()];
        pp._isGas = false;

        p.GetComponent<SpriteRenderer>().sprite = pp._sprite;

        gm.OpenRecipe(menu.Index);

        return;
    }

    private void ResetBoiler()
    {
        stackCount = 0;
        powderCode = 0;
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
                    p.GetComponent<Product>()._sprite = gasSprites[powderCode.ToString()];
                    inventory.SetSlot(p.GetComponent<Product>(), true);
                    Destroy(p);
                    mouse.RemoveHolding();

                    ResetBoiler();
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

            ResetBoiler();
        }
    }
}
