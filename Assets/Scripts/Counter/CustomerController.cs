using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private CustomerManager cm;

    private void Start()
    {
        cm = GameObject.FindObjectOfType<CustomerManager>();
    }

    private void OnMouseDown()
    {
        cm.ClickCustomer();
    }
}
