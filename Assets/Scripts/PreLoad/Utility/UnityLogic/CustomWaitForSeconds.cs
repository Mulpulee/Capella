using System;
using System.Collections;
using UnityEngine;

public class CustomWaitForSeconds : CustomYieldInstruction
{
    private Single _leftDuration;
    private Single _targetDuration;
    public CustomWaitForSeconds(Single pDuration)
    {
        _targetDuration = pDuration;
        _leftDuration = pDuration;
    }

    public override bool keepWaiting
    {
        get
        {
            _leftDuration -= TimeManager.ins.GetDeltaTime();
            Boolean value = _leftDuration > 0;
            if(!value)
            {
                _leftDuration = _targetDuration;
            }
            return value;
        }
    }
}

