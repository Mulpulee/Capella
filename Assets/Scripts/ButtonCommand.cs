using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCommand : MonoBehaviour
{
    [SerializeField] private string command;

    private GameManagerEx gm;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManagerEx>();

        GetComponent<Button>().onClick.AddListener(() => gm.ButtonTrigger(command));
        if(command == "Reset")
            GetComponent<Button>().onClick.AddListener(() => Destroy(transform.parent.gameObject));
    }
}
