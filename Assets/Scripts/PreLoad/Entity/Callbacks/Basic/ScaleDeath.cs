using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScaleDeath : IDeath
{
    private Action m_onDeath;

    public ScaleDeath(Action pOnDeath)
    {
        m_onDeath = pOnDeath;
    }

    public void Destroy()
    {
    }

    public void OnDeath(EntityBehaviour pEntity)
    {
        m_onDeath?.Invoke();
        pEntity.transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            GameObject.Destroy(pEntity.gameObject);
        });
    }
}