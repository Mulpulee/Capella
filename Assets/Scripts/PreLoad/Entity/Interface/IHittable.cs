using System;
using UnityEngine;

public interface IHittable
{
    void OnHit(GameObject pAttacker, Single pDamage);
}