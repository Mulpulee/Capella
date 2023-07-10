using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EntityHit : IHit
{
    public virtual void Destroy()
    {
    }

    public virtual void OnHit(EntityBehaviour pEntity, GameObject pAttacker, float pDamage)
    {
        if (pEntity.Model.HealthPoint <= 0)
            return;

        pEntity.Model.HealthPoint -= pDamage;

        // Vector3 velocity = (pEntity.transform.position - pAttacker.transform.position).normalized + (Vector3)UnityEngine.Random.insideUnitCircle * 0.3f;
        Vector3 velocity = new Vector3(0, 1, 0) * pDamage / 150f + new Vector3(0, 1, 0);
        Vector3 position = pEntity.transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle); 

        if(pEntity.Model.HealthPoint <= 0)
        {
            pEntity.Model.HealthPoint = 0;
            pEntity.OnDeath();
            OnDeath();
        }
    }
    protected virtual void OnDeath() { }
}