using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecondary : BaseProjectile
{
    public float meterCost;
    protected override void Awake() {
        base.Awake();
        knockback += PlayerManager.Instance.combatant.GetStatValue(StatType.KnockbackUp);
    }
    public override void Knockback(BaseDamageable damageable) {
        if (host) {
            Debug.Log("knock back");
            Vector2 dir = (damageable.transform.position - host.transform.position).normalized;
            damageable.rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }
        else {
            base.Knockback(damageable);
        }
    }
}
