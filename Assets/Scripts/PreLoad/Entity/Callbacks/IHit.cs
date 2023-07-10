using System;
using UnityEngine;

public interface IHit
{
    void OnHit(EntityBehaviour pEntity, GameObject pAttacker,Single pDamage);
    void Destroy();
}