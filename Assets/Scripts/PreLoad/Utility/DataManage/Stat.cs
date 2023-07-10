using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Stat : IObservable<Stat>
{
    private readonly Int32 MaxValue;
    private readonly Int32 MinValue;

    [SerializeField,ReadOnly] private Int32 value;

    public delegate void OnChange();

    [NonSerialized] private OnChange onIncrease;
    [NonSerialized] private OnChange onDecrease;
    [NonSerialized] private OnChange onChange;

    public Stat(Int32 Min, Int32 Max, Int32 initValue)
    {
        MaxValue = Max;
        MinValue = Min;
        value = initValue;
    }

    public Int32 GetValue() => value;

    public void AssignOnIncrease(OnChange pOnChange)
    {
        onIncrease += pOnChange;
    }
    public void AssignOnDecrease(OnChange pOnChange)
    {
        onDecrease += pOnChange;
    }
    public void AssignOnChange(OnChange pOnChange)
    {
        onChange += pOnChange; 
    }
    public void ReleaseOnChange(OnChange pOnChange)
    {
        onChange -= pOnChange;
    }
    public void ReleaseOnIncrease(OnChange pOnChange)
    {
        onIncrease -= pOnChange;
    }
    public void ReleaseOnDecrease(OnChange pOnChange)
    {
        onDecrease -= pOnChange;
    }


    public void Increase(Int32 value)
    {
        if (this.value + value <= MaxValue)
        {
            this.value += value;
            onIncrease?.Invoke();
            onChange?.Invoke();
            SendNotify(this);
        }
        else
            this.value = MaxValue;
    }
    public void Decrease(Int32 value)
    {
        if (this.value - value >= MinValue)
        {
            this.value -= value;
            onDecrease?.Invoke();
            onChange?.Invoke();
            SendNotify(this);
        }
        else
            this.value = MinValue;
    }
    public void ChangeValue(Int32 value)
    {
        if (value <= MaxValue && value >= MinValue)
        {
            this.value = value;
            if (this.value > value)
            {
                onChange?.Invoke();
                onDecrease?.Invoke();
                SendNotify(this);
            }
            else if (this.value < value)
            {
                onChange?.Invoke();
                onIncrease?.Invoke();
                SendNotify(this);
            }
        }
        else if (value > MaxValue)
            this.value = MaxValue;
        else if (value < MinValue)
            this.value = MinValue;
    }
    public void DecreasePercent(Single percent, Boolean invokeOnChange)
    {
        Single ChangeVal = (Int32)this.value * percent;

        if (this.value - ChangeVal >= MinValue)
        {
            value -= (Int32)ChangeVal;
            if (invokeOnChange)
            {
                onChange?.Invoke();
                SendNotify(this);
                onDecrease?.Invoke();
            }
        }
        else
            value = MinValue;
    }
    public Boolean UseStat(Int32 value)
    {
        if(this.value - value >= MinValue)
        {
            Decrease(value);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CalculateValue(Int32 value)
    {
        if(value > 0)
            Increase(value);
        else
            Decrease(Mathf.Abs(value));
    }


    [NonSerialized] private List<IObserver<Stat>> _subscribers;
    public void Subscribe(IObserver<Stat> pObserver)
    {
        if (_subscribers == null)
            _subscribers = new List<IObserver<Stat>>();

        _subscribers.Add(pObserver);
        pObserver.OnNotify(this);
    }

    public void UnSubscribe(IObserver<Stat> pObserver)
    {
        if (_subscribers == null)
            return;

        _subscribers.Remove(pObserver);
    }

    public void SendNotify(Stat pValue)
    {
        if (_subscribers == null)
            return;

        for(Int32 i=0,iCount = _subscribers.Count;i<iCount;i++)
        {
            _subscribers[i].OnNotify(pValue);
        }
    }
}
