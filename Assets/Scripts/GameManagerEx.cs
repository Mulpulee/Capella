using Automation.DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerEx : MonoBehaviour
{
    [SerializeField] private Vector3 counterPos;
    [SerializeField] private Vector3 alchemyPos;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject warning;

    private Save data;
    private MoveNemo fade;
    private SceneMove scene;

    private List<int>[] specialDate;
    private List<OrderScriptRow> script;
    private List<MenuRow> menus;

    private Playerdata playerdata;
    private string screen;

    private bool isDay = true;
    public bool isSetting = false;

    private SoundManagerEx sm;

    private Coroutine playBgm;
    private int bgmIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        screen = "Title";

        data = GetComponent<Save>();
        LoadData();
        if (data.playerData.day == 0) data.playerData.day = 1;
        
        scene = GetComponent<SceneMove>();
        sm = GameObject.FindObjectOfType<SoundManagerEx>();
    }

    private void Start()
    {
        SceneManager.LoadScene("TitleScene");
        sm.SetAll(playerdata.mstVolume, playerdata.bgmVolume, playerdata.sfxVolume);
        sm.PlayBgm(0);
    }

    public void NewGame()
    {
        playerdata = new Playerdata();
        playerdata.day = 1;
        scene.ChangeScene("CounterScene", () => { }, () => { });
        InGameScene();
    }

    public void ContinueGame()
    {
        scene.ChangeScene("CounterScene", () => { }, () => { });
        InGameScene();
    }

    private void InGameScene()
    {
        bgmIndex = 1;
        playBgm = StartCoroutine(PlayBgm());
        LoadData();

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

        script = DataTableLoader.GetTable<OrderScriptRow>().ToList<OrderScriptRow>();
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

    public Playerdata GetData()
    {
        return playerdata;
    }

    public void SetVolumes(float mst, float bgm, float sfx)
    {
        playerdata.mstVolume = mst;
        playerdata.bgmVolume = bgm;
        playerdata.sfxVolume = sfx;

        SaveData();
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

    private IEnumerator PlayBgm()
    {
        if (isDay) sm.PlayBgm(bgmIndex);
        else sm.PlayBgm(4);

        while (sm.BGMsource.isPlaying) yield return null;

        if(isDay)
        {
            bgmIndex++;
            if (bgmIndex == 4) bgmIndex = 0;
        }
        else
        {
            bgmIndex++;
            if (bgmIndex == 8) bgmIndex = 4;
        }

        playBgm = StartCoroutine(PlayBgm());
    }

    public void ChangeTime(bool day)
    {
        isDay = day;
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
        LoadData();
        return playerdata.recipe;
    }

    public void CloseCafe()
    {
        playerdata.day++;
        isDay = true;

        SaveData();

        StartCoroutine(CloseCafeRoutine());
    }

    public void InResult()
    {

    }

    public IEnumerator CloseCafeRoutine()
    {
        bool isEnd = false;

        scene.ChangeScene("ResultScene", () => { }, () => isEnd = true);

        while (!isEnd) yield return null;
        isEnd = false;

        StartCoroutine(GameObject.FindObjectOfType<ChangeDate>().ChangeAnim(() => isEnd = true));

        while (!isEnd) yield return null;
        isEnd = false;

        scene.ChangeScene("CounterScene", () => { }, () => isEnd = true);

        while (!isEnd) yield return null;
        isEnd = false;

        InGameScene();
    }

    public void ButtonTrigger(string command)
    {
        if(command == "NewGame")
        {
            Instantiate(warning);
        }
        if(command == "Reset")
        {
            NewGame();
        }
        if(command == "ContinueGame")
        {
            ContinueGame();
        }
        if(command == "Exit")
        {
            Application.Quit();
        }
    }

    public void GotoTitle()
    {
        screen = "Title";
        StopCoroutine(playBgm);
        sm.PlayBgm(0);
        SceneManager.LoadScene("TitleScene");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (screen == "Counter")
            {
                fade = GameObject.FindObjectOfType<MoveNemo>();
                fade.StartMovingBlackSquare(alchemyPos);
                screen = "Alchemy";
            }
            else if (screen == "Alchemy")
            {
                fade = GameObject.FindObjectOfType<MoveNemo>();
                fade.StartMovingBlackSquare(counterPos);
                screen = "Counter";
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isSetting) Instantiate(setting);
        }
    }
}
