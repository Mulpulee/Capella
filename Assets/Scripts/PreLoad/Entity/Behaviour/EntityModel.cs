using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityModel 
{
    private Func<Single> m_maxHealthGetter;
    private Func<Single> m_maxMoveSpeedGetter;

    private Single m_healthPoint;

    public Single MaxHealth => m_maxHealthGetter.Invoke();
    public Single MaxMovement => m_maxMoveSpeedGetter.Invoke();
    
    public Single HealthPoint 
    {
        get => m_healthPoint;
        set
        {
            m_healthPoint = value;
            if (m_healthPoint > MaxHealth)
                m_healthPoint = MaxHealth;
        }
    }
    public Single MoveSpeed => m_maxMoveSpeedGetter.Invoke();

    public EntityModel(Single pMaxHealth, float pMaxMoveSpeed)
    {
        m_maxHealthGetter = ()=> pMaxHealth;
        m_maxMoveSpeedGetter = ()=> pMaxMoveSpeed;

        HealthPoint = pMaxHealth;
    }

    public EntityModel(Func<Single> pMaxHealthGetter,Func<Single> pMaxMoveSpeedGetter )
    {
        m_maxHealthGetter = pMaxHealthGetter;
        m_maxMoveSpeedGetter = pMaxMoveSpeedGetter;

        HealthPoint = MaxHealth;
    }
}
