using Automation.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private Vector3 openPos;
    [SerializeField] private Vector3 closePos;
    [SerializeField] private Sprite waterPot;
    [SerializeField] private Sprite oilPot;

    private Dictionary<string, Sprite> drinkSprites;
    private Dictionary<string, Sprite> gasSprites;
    private Dictionary<string, Sprite> ingredSprites;
    
    private GameObject[] tabs;
    private GameObject ingredInfo;
    private GameObject recipe;

    private GameManagerEx gm;
    private SoundManagerEx sm;

    private bool isClosed = true;
    private int showRecipe = -1;

    private int page = 0;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManagerEx>();
        sm = GameObject.FindObjectOfType<SoundManagerEx>();

        tabs = new GameObject[3];
        for (int i = 0; i < 3; i++) tabs[i] = transform.GetChild(i).gameObject;
        for (int i = 1; i < 3; i++) tabs[i].SetActive(false);

        ingredInfo = transform.GetChild(3).gameObject; ingredInfo.SetActive(false);
        recipe = transform.GetChild(4).gameObject; recipe.SetActive(false);

        page = 0;
        
        drinkSprites = new Dictionary<string, Sprite>();
        gasSprites = new Dictionary<string, Sprite>();
        ingredSprites = new Dictionary<string, Sprite>();
        LoadSprites();
    }

    private void LoadSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Arts/Drinks");
        foreach (Sprite sprite in sprites) drinkSprites.Add(sprite.name, sprite);
        sprites = Resources.LoadAll<Sprite>("Arts/Gases");
        foreach (Sprite sprite in sprites) gasSprites.Add(sprite.name, sprite);
        sprites = Resources.LoadAll<Sprite>("Arts/Ingredients");
        foreach (Sprite sprite in sprites) ingredSprites.Add(sprite.name, sprite);
    }

    public void ChangePage(bool next)
    {
        if (showRecipe != -1)
        {
            CloseIngredInfo();
            CloseRecipeInfo();
            return;
        }

        sm.OnSfx(9);
        if (next) page++;
        else page--;

        if (page == -1) page = 2;
        if (page == 3) page = 0;

        for (int i = 0; i < 3; i++) tabs[i].SetActive(false);
        tabs[page].SetActive(true);

        if (page != 0) SetRecipeOpen();
    }

    private void SetRecipeOpen()
    {
        int[] recipe = gm.GetOpendedRecipe();

        if (page == 1)
        {
            for (int i = 1; i <= 16; i++)
                if (recipe[i - 1] == 1)
                    tabs[1].transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
        }
        else
        {
            for (int i = 17; i <= 29; i++)
                if (recipe[i - 1] == 1)
                    tabs[2].transform.GetChild(i - 16).GetChild(0).GetComponent<Image>().color = Color.white;
        }
    }

    public void ShowIngredInfo(int id)
    {
        if (showRecipe != -1) return;

        IngredientTableRow ingredTable = DataSystem<IngredientTableRow>.GetRow(30000 + id + 1);

        showRecipe = 0;
        tabs[0].SetActive(false);
        ingredInfo.SetActive(true);

        ingredInfo.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ingredSprites[id.ToString()];
        ingredInfo.transform.GetChild(1).GetComponent<SpriteRenderer>().color
            = new Color(ingredTable.PowderColor_r, ingredTable.PowdeColor_g, ingredTable.PowdeColor_b);
        ingredInfo.transform.GetChild(2).GetComponent<Text>().text = ingredTable.Name;
        ingredInfo.transform.GetChild(3).GetComponent<Text>().text = ingredTable.Taste;
        ingredInfo.transform.GetChild(4).GetComponent<Text>().text = ingredTable.Description;
    }

    public void CloseIngredInfo()
    {
        if (showRecipe != 0) return;

        showRecipe = -1;
        tabs[0].SetActive(true);
        ingredInfo.SetActive(false);
    }
    
    public void ShowRecipeInfo(int info)
    {
        int tab = info / 10000;
        int id = (info - (tab * 10000)) / 100;
        int index = info % 100;

        if (showRecipe != -1) return;

        MenuRow menuTable = DataSystem<MenuRow>.GetRow(10000 + id);

        showRecipe = tab;
        tabs[tab].SetActive(false);
        recipe.SetActive(true);

        recipe.transform.GetChild(3).GetComponent<Text>().text = menuTable.Name;
        recipe.transform.GetChild(4).GetComponent<Text>().text = menuTable.Description;

        if (gm.GetOpendedRecipe()[index] == 0)
        {
            recipe.transform.GetChild(0).gameObject.SetActive(true);
            return;
        }
        recipe.transform.GetChild(0).gameObject.SetActive(false);

        recipe.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = drinkSprites[id.ToString()];
        recipe.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = gasSprites[id.ToString()];
        if(id == 88)
        {
            recipe.transform.GetChild(6).GetComponent<SpriteRenderer>().sprite = oilPot;
            recipe.transform.GetChild(7).GetComponent<SpriteRenderer>().sprite = null;
            recipe.transform.GetChild(8).GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            recipe.transform.GetChild(6).GetComponent<SpriteRenderer>().sprite = waterPot;
            recipe.transform.GetChild(7).GetComponent<SpriteRenderer>().sprite = ingredSprites[menuTable.Ingredient1.ToString()];
            recipe.transform.GetChild(8).GetComponent<SpriteRenderer>().sprite = ingredSprites[menuTable.Ingredient2.ToString()];
        }
    }

    public void CloseRecipeInfo()
    {
        if (showRecipe == -1 || showRecipe == 0) return;

        tabs[showRecipe].SetActive(true);
        showRecipe = -1;
        recipe.SetActive(false);
    }

    private void OnMouseDown()
    {
        sm.OnSfx(11);
        if(isClosed)
        {
            isClosed = false;
            if (page != 0) SetRecipeOpen();
            StartCoroutine(MoveCoroutine(openPos, 50f));
        }
        else
        {
            isClosed = true;
            StartCoroutine(MoveCoroutine(closePos, 50f));
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) OnMouseDown();
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
