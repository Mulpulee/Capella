using System;
using UnityEngine;



[Serializable]
public struct SingleRange
{
    [SerializeField] private Single m_min;
    [SerializeField] private Single m_max;

    public Single From
    {
        get => m_min;
        set => m_min = value;
    }

    public Single To
    {
        get => m_max;
        set => m_max = value;
    }
    
    public SingleRange(Single pFromInclusive, Single pToInclusive)
    {
        m_min = pFromInclusive;
        m_max = pToInclusive;
    }

    public Single Random()
    {
        return UnityEngine.Random.Range(m_min, m_max);
    }
}


[Serializable]
public struct IntRange
{
    [SerializeField] private Int32 m_min;
    [SerializeField] private Int32 m_max;

    public Int32 From
    {
        get => m_min;
        set => m_min = value;
    }

    public Int32 To
    {
        get => m_max;
        set => m_max = value;
    }
    
    public IntRange(Int32 pFromInclusive, Int32 pToInclusive)
    {
        m_min = pFromInclusive;
        m_max = pToInclusive;
    }
    public Int32 Random()
    {
        return UnityEngine.Random.Range(m_min, m_max+1);
    }
}