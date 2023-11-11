using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BaseProjectile : BaseDamageSource
{
    public float projectileSpeed;
    public void InitProjectile(BaseDamageable host, Vector2 direction) {
        this.host = host;
        rb.AddForce(projectileSpeed*direction, ForceMode2D.Impulse);
    }
}
