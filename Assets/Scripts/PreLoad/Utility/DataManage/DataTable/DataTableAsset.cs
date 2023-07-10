using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableAsset<T> : ScriptableObject where T : DataTableRow
{
    public DataTable<T> Table;
}
