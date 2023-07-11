using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField] private int slotIndex;

    private void OnMouseDown()
    {
        transform.parent.GetComponent<Inventory>().ClickSlot(slotIndex);
    }
}
