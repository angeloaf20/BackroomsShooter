using System;
using UnityEngine;

public class EnemyHealth : Damageable
{
    protected override void Kill()
    {
        Destroy(gameObject);
    }
}

