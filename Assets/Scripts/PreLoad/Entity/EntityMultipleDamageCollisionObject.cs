using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EntityMultipleDamageCollisionObject : MonoBehaviour
{
    public event Action<Collider> OnCollide;
    private Collider m_collider;
    private Single m_damage;
    
    private Single m_interval;
    private Boolean m_isStarted;
    
    private Single m_duration;

    private Coroutine m_damageRoutine;

    public Collider Collider => m_collider;
    public Single Damage => m_damage;

    private IHittable m_exclude;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_collider.enabled = false;
    }

    public void StartDamage(Single pDamage,Single pInterval, IHittable pExclude = null)
    {
        m_damage = pDamage;
        m_exclude = pExclude;
        m_interval = pInterval;
        m_isStarted = true;
        m_duration = -1;
        m_damageRoutine = StartCoroutine(DamageRoutine());
    }

    public void EndDamage()
    {
        m_isStarted = false;

        if (m_damageRoutine != null)
            StopCoroutine(m_damageRoutine);
    }

    public void StartDamageFor(Single pDamage,Single pInterval,Single pDuration,IHittable pExclude = null)
    {
        StartDamage(pDamage, pInterval, pExclude);
        m_duration = pDuration;

        IEnumerator StopRoutine()
        {
            yield return new WaitForSeconds(m_duration);
            EndDamage();
        }
        StartCoroutine(StopRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        while(m_isStarted)
        {
            m_collider.enabled = true;
            yield return null;
            m_collider.enabled = false;

            yield return new WaitForSeconds(m_interval * 0.9f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHittable>(out IHittable hittable))
        {
            if (m_exclude != null)
            {
                if (hittable == m_exclude)
                    return;
            }

            hittable.OnHit(gameObject, m_damage);
            OnCollide?.Invoke(other);
        }
    }
}
