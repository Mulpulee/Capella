using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EntityDamageCollisionObject : MonoBehaviour
{
    public event Action<Collider> OnCollide;
    public event Func<Single, Single> OnCollideDamageModify;

    private Collider m_collider;
    private Single m_damage;

    public Collider Collider => m_collider;
    public Single Damage => m_damage;

    private IHittable m_exclude;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_collider.enabled = false;
    }

    public void Initialize(Single pDamage,IHittable pExclude = null)
    {
        m_damage = pDamage;
        m_collider.enabled = true;
        m_exclude = pExclude;
    }

    public void Disable()
    {
        if (m_collider == null)
            return;

        m_collider.enabled = false;
    }

    public void Enable()
    {
        if (m_collider == null)
            return;

        m_collider.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IHittable>(out IHittable hittable))
        {
            if(m_exclude != null)
            {
                if (hittable == m_exclude)
                    return;
            }

            Single damage = m_damage;

            if(OnCollideDamageModify != null)
            {
                damage = OnCollideDamageModify(damage);
            }

            hittable.OnHit(gameObject, damage);
            OnCollide?.Invoke(other);
        }
    }
}