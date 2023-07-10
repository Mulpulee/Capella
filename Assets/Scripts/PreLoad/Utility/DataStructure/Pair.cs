using UnityEngine;
using System;

[System.Serializable]
public class Pair<TPrime,TSecond> 
{
    public TPrime primary;
    public TSecond secondary;
    public Pair() { }
    public Pair(TPrime pPrimary,TSecond pSecondary)
    {
        primary = pPrimary;
        secondary = pSecondary;
    }
}

public class StructPair<TPrime,TSecond>
{
    public TPrime primary;
    public TSecond secondary;
    public StructPair()
    {

    }
    public StructPair(TPrime pPrimary,TSecond pSecond)
    {
        primary = pPrimary;
        secondary = pSecond;
    }
}