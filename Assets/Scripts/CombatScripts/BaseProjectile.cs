using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BaseProjectile : BaseDamageSource
{
    public float projectileSpeed;

    public void InitProjectile(BaseDamageable host, Vector2 direction) {
        Init(host);
        rb.AddForce(projectileSpeed*direction, ForceMode2D.Impulse);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
