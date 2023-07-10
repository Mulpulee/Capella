using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour, IHittable, IDeathable
{
    [SerializeField] private Int32 m_entityID;
    [SerializeField] private EntityModel m_model;

    private IDeath m_death;
    private IHit m_hit;

    public T GetDeathAs<T>() where T : class, IDeath => m_death as T;
    public T GetHitAs<T>() where T : class, IHit => m_hit as T;

    public event Action OnHitEvent;
    public event Action OnDeathEvent;

    public void ResetID(Int32 pID)
    {
        m_entityID = pID;
    }

    public EntityModel Model => m_model;

    public Int32 EntityID => m_entityID;

    public Boolean IsAlive => m_model.HealthPoint > 0;

    public void OnHit(GameObject pAttacker, Single pDamage)
    {
        OnHitEvent?.Invoke();
        m_hit?.OnHit(this, pAttacker, pDamage);
    }

    public void OnDeath()
    {
        OnDeathEvent?.Invoke();
        m_death?.OnDeath(this);
    }

    private void OnDestroy()
    {
        m_hit.Destroy();
        m_death.Destroy();

        m_hit = null;
        m_death = null;
    }

    public static EntityBuilder GetBuilder(EntityBehaviour pPrefab,Vector3 pPosition)
    {
        return EntityBuilder.Get(pPrefab, pPosition);
    }

    public class EntityBuilder
    {
        private static EntityBuilder m_entityBuilderInstance;

        public static EntityBuilder Get(EntityBehaviour pPrefab, Vector3 pPosition)
        {
            if (m_entityBuilderInstance == null)
                m_entityBuilderInstance = new EntityBuilder();

            m_entityBuilderInstance.Reset();
            m_entityBuilderInstance.m_instance = GameObject.Instantiate(pPrefab, pPosition, pPrefab.transform.rotation);

            return m_entityBuilderInstance;
        }

        private EntityBehaviour m_instance;
        private EntityModel m_model;
        private IDeath m_death;
        private IHit m_hit;

        private EntityBuilder() { }

        public void Reset()
        {
            m_instance = null;
            m_model = null;
            m_hit = null;
            m_death = null;
        }

        public static EntityBehaviour GetNowEntityInstance() => m_entityBuilderInstance.m_instance;

        public EntityBuilder SetDeath(IDeath pDeath)
        {
            m_death = pDeath;
            return this;
        }
        public EntityBuilder SetHit(IHit pHit)
        {
            m_hit = pHit;
            return this;
        }
        public EntityBuilder SetModel(EntityModel pModel)
        {
            m_model = pModel;
            return this;
        }

        public EntityBehaviour Build()
        {
            if (m_model == null)
                m_model = new EntityModel(100, 10);

            if (m_hit == null)
                m_hit = new EntityHit();

            if (m_death == null)
                m_death = new EntityDeath();

            m_instance.m_death = m_death;
            m_instance.m_hit = m_hit;
            m_instance.m_model = m_model;

            return m_instance;
        }
    }
}
