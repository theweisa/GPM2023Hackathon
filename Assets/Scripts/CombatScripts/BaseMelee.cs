using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMelee : BaseDamageSource
{
    public virtual void FixedUpdate() {
        if (host.gameObject.CompareTag("Player")) {
            transform.position = PlayerManager.Instance.controller.firePoint.position;
        }
    }
}
