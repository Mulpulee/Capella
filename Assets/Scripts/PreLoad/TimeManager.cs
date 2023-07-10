using System;
using UnityEngine;

public class TimeManager : IndestructibleSingleton<TimeManager>
{
    public Boolean isTimeStopped;
    public Single timeSpeed = 1f;

    protected override void OnSingletonInstantiated()
    {
    }

    public Single GetDeltaTime()
    {
        if (isTimeStopped)
            return 0;
        else
            return timeSpeed*Time.deltaTime;
    }

    public void SetTime(Boolean pValue)
    {
        isTimeStopped = pValue;
    }

    public void ToggleTime()
    {
        isTimeStopped = !isTimeStopped;
    }

    public void StopTime()
    {
        isTimeStopped = true;
        Debug.Log("TIMEMANAGER :: Time has Stopped");
    }

    public void ReleaseTime()
    {
        isTimeStopped = false;
        Debug.Log("TIMEMANAGER :: Time has Released");
    }
}