using Automation.DataTable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerEx : MonoBehaviour
{
    [SerializeField] Vector3 counterPos;
    [SerializeField] Vector3 alchemyPos;

    private Save data;
    private MoveNemo fade;

    private List<int>[] specialDate;
    private List<OrderScriptRow> script;
    private List<MenuRow> menus;

    private Playerdata playerdata;
    private string screen;

    private void Awake()
    {
        data = GameObject.FindObjectOfType<Save>();
        LoadData();
        fade = GameObject.FindObjectOfType<MoveNemo>();

        specialDate = new List<int>[5];
        SpecialDateRow table;
        for (int i = 0; i < 5; i++)
        {
            table = DataSystem<SpecialDateRow>.GetRow(40000 + i + 1);

            List<int> date = new List<int>();
            date.Add(table.Date1);
            date.Add(table.Date2);
            date.Add(table.Date3);
            date.Add(table.Date4);
            date.Add(table.Date5);
            date.Add(table.Date6);
            date.Add(table.Date7);
            date.Add(table.Date8);
            date.Add(table.Date9);

            specialDate[i] = date;
        }

        script =  DataTableLoader.GetTable<OrderScriptRow>().ToList<OrderScriptRow>();
        script.RemoveAt(0);
        script.Add(DataSystem<OrderScriptRow>.GetRow(20001));

        menus = DataTableLoader.GetTable<MenuRow>().ToList<MenuRow>();

        screen = "Counter";
    }

    private void LoadData()
    {
        data.LoadPlayerDataFromJson();
        playerdata = data.playerData;
    }

    private void SaveData()
    {
        data.playerData = playerdata;
        data.SavePlayerDataToJson();
    }

    public List<int>[] GetSpecialDate()
    {
        return specialDate;
    }

    public MenuRow GetMenuRow(int index)
    {
        return menus[index];
    }

    public OrderScriptRow GetOrderScript(int id)
    {
        return script[id];
    }

    public void AddMoney(int amount)
    {
        playerdata.money += amount;
    }

    public int GetDay()
    {
        return playerdata.day;
    }

    public void OpenRecipe(int index)
    {
        playerdata.recipe[index] = 1;
    }

    public int[] GetOpendedRecipe()
    {
        return playerdata.recipe;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (screen == "Counter")
            {
                fade.StartMovingBlackSquare(alchemyPos);
                screen = "Alchemy";
            }
            else if (screen == "Alchemy")
            {
                fade.StartMovingBlackSquare(counterPos);
                screen = "Counter";
            }
        }
    }
}
