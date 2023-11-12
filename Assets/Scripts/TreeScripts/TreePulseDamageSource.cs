using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TreePulseDamageSource : BaseDamageSource
{
    [Header("Pulse Variables")]
    public float healCoef = 1f;
    public bool useCurrentScaleAsFinalScale = true;
    [HideIf("useCurrentScaleAsFinalScale")] public float endScale;
    // expands until its lifespan ends
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Vector3 eScale = useCurrentScaleAsFinalScale ? transform.localScale : new Vector3(endScale, endScale, endScale);
        eScale *= PlayerManager.Instance.combatant.GetStatValue(StatType.TreeProjSize);
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, eScale, lifetime).setEaseOutExpo();
    }

    public override void OnHit(BaseDamageable damageable) {
        if (damageable.IsPlayer()) {
            damageable.Heal(damageable.GetStatValue(StatType.TreeHeal) * healCoef);
        }
        else {
            damageable.Damage(this);
        }
        hitDamageables.Add(damageable);
        if (destroyOnContact) {
            StartCoroutine(OnDeath());
        }
        else if (damageOverTime){
            active = false;
        }
    }
}
