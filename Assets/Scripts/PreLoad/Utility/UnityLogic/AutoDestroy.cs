using System;
using UnityEngine;


public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private Boolean activated;
    [SerializeField] private Single lifetime;
    private Single _leftDuration;

    private void Awake()
    {
        if (activated)
            _leftDuration = lifetime;
    }

    private void Update()
    {
        if (!activated)
            return;

        _leftDuration -= TimeManager.ins.GetDeltaTime();
        if(_leftDuration <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Activate(Single pLifeTime = 0)
    {
        if (pLifeTime == 0)
            _leftDuration = lifetime;
        else
            _leftDuration = pLifeTime;
        activated = true;
    }
}