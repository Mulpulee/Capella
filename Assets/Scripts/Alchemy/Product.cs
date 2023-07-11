using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public int _id;
    public string _name;
    public string _description;

    public Color _Color;

    public bool _isGas;

    public void SetData(Product product)
    {
        _id = product._id;
        _name = product._name;
        _description = product._description;
        _Color = product._Color;
        _isGas = product._isGas;
    }
}
