using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nok : MonoBehaviour
{
    public void Back()
    {
        Destroy(transform.parent.gameObject);
    }
}
