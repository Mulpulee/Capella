using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredType
{
    Plant,
    Mushroom,
    Stone,
    Liquid,
    etc
}

public class Ingredient : MonoBehaviour
{
    public IngredType _type;

    public int _id;
    public string _name;
    public string _description;

    public Sprite[] _sprites;
    public Color _powderColor;

    public bool _grinded;

    public void SetData(Ingredient ingredient)
    {
        _type = ingredient._type;
        _id = ingredient._id;
        _name = ingredient._name;
        _description = ingredient._description;
        _sprites = ingredient._sprites;
        _powderColor = ingredient._powderColor;
        _grinded = ingredient._grinded;
    }
}
