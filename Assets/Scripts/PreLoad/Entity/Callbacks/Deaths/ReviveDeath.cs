using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ReviveDeath : IDeath
{
    public event Action OnDeathEvent;
    private Single m_reviveTime;
    public ReviveDeath(Single pReviveTime)
    {
        m_reviveTime = pReviveTime;
    }

    public void Destroy()
    {
    }

    public void OnDeath(EntityBehaviour pEntity)
    {
        Debug.Log("Dead!");
        OnDeathEvent?.Invoke();
        pEntity.StartCoroutine(ReviveRoutine(pEntity));
    }

    private IEnumerator ReviveRoutine(EntityBehaviour pEntity)
    {
        yield return new WaitForSeconds(m_reviveTime);
        pEntity.Model.HealthPoint = pEntity.Model.MaxHealth;
        Debug.Log("Revive!");
    }
}