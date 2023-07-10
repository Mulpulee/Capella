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

    public string _name;
    public string _description;

    public Sprite[] _sprites;
    public Color _powderColor;

    public bool _grinded;
}
