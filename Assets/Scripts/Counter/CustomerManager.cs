using Automation.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] MobCustomer;
    [SerializeField] private GameObject[] SpecialCustomer;

    private GameObject canvas;
    private Text[] texts = new Text[2];

    private GameManagerEx gm;
    private MouseManager mouse;
    private Inventory inven;
    private TextDelay textprinter;
    private List<int>[] specialDates;

    private int special;
    private Product ordered = null;
    private OrderScriptRow orderScript;
    private GameObject cs = null;

    private int csmCount = 0;
    private bool isServed = false;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManagerEx>();
        mouse = GameObject.FindObjectOfType<MouseManager>();
        inven = GameObject.FindObjectOfType<Inventory>();
        textprinter = GetComponent<TextDelay>();
        specialDates = gm.GetSpecialDate();

        special = GetSpecialDate(gm.GetDay());
        if(special == -1)
            StartCoroutine(CustomerOrder(5, MobCustomer[Random.Range(0, MobCustomer.Length)]));
        else
            StartCoroutine(CustomerOrder(special, SpecialCustomer[special], true));

        ordered = GetComponent<Product>();

        canvas = transform.GetChild(0).gameObject;
        texts[0] = canvas.transform.GetChild(1).GetComponent<Text>();
        texts[1] = canvas.transform.GetChild(2).GetComponent<Text>();

        canvas.SetActive(false);

        csmCount = Random.Range(10, 15);
    }

    private int GetSpecialDate(int day)
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (int date in specialDates[i])
            {
                if (date == day) return i;
            }
        }

        return -1;
    }

    private IEnumerator CustomerOrder(int id, GameObject customer, bool isSpecial = false)
    {
        orderScript = gm.GetOrderScript(id);
        yield return new WaitForSeconds(Random.Range(5, 10));

        cs = Instantiate(customer, new Vector3(19, -10), Quaternion.identity);

        MenuRow orderMenu = gm.GetMenuRow(Random.Range(0, 28));
        ordered._id = orderMenu.ID;
        ordered._name = orderMenu.Name;
        ordered._description = orderMenu.Description;
        ordered._isGas = Random.Range(1, 10) <= 5 ? true : false;

        StartCoroutine(MoveCoroutine(cs.transform, new Vector3(19, 0.3f), 120f));

        yield return new WaitForSeconds(0.8f);

        string ment;

        switch (Random.Range(0, isSpecial ? 3 : 5))
        {
            case 1: ment = orderScript.Ment1; break;
            case 2: ment = orderScript.Ment2; break;
            case 3: ment = orderScript.Ment3; break;
            case 4: ment = orderScript.Ment4; break;
            case 5: ment = orderScript.Ment5; break;

            default: ment = orderScript.Ment1; break;
        }

        string orderMent = orderScript.OrderScript_front + orderMenu.Name
            + (ordered._isGas ? " 기체" : orderMenu.Eu ? "으" : "") + orderScript.OrderScript_back;

        PrintMent(ment, orderMent);

        yield return null;
    }

    public void ClickCustomer()
    {
        Product holding = mouse.GetHoldingProduct();
        if (holding == null) return;

        if (holding._name == ordered._name && holding._isGas == ordered._isGas) Correct();
        else Wrong();

        inven.UseSlot();
    }

    private void Correct()
    {
        isServed = true;
        PrintMent(orderScript.Correct);
    }

    private void Wrong()
    {
        isServed = true;
        PrintMent(orderScript.Wrong);
    }

    private void PrintMent(string a, string b = null)
    {
        canvas.SetActive(true);
        texts[0].text = orderScript.Name;

        if (b == null) textprinter.SetTexts(new string[1] { a });
        else textprinter.SetTexts(new string[2] { a, b });

        textprinter.StartPrint(8.0f);
    }

    public void EndDialog()
    {
        if (!isServed) return;

        isServed = false;
        canvas.SetActive(false);

        StartCoroutine(MoveCoroutine(cs.transform, new Vector3(19, -10f), 120f, true));                               
    }

    private IEnumerator MoveCoroutine(Transform _transform, Vector3 pos, float duration, bool delete = false)
    {
        float dir = (pos.y - _transform.localPosition.y) / duration;
        while ((dir > 0 && _transform.localPosition.y <= pos.y)
            || (dir < 0 && _transform.localPosition.y >= pos.y))
        {
            _transform.localPosition += new Vector3(0, dir, 0);
            yield return null;
        }

        _transform.localPosition = pos;

        if (delete)
        {
            Destroy(_transform.gameObject);
            if (csmCount-- > 0)
                StartCoroutine(CustomerOrder(5, MobCustomer[Random.Range(0, MobCustomer.Length)]));
        }

        yield return null;
    }
}
