using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TreePulseDamageSource : BaseDamageSource
{
    [Header("Pulse Variables")]
    public bool useCurrentScaleAsFinalScale = true;
    [HideIf("useCurrentScaleAsFinalScale")] public float endScale;
    // expands until its lifespan ends
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Vector3 eScale = useCurrentScaleAsFinalScale ? transform.localScale : new Vector3(endScale, endScale, endScale);
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, eScale, lifetime).setEaseOutExpo();
    }

    public override void OnHit(BaseDamageable damageable) {
        if (damageable.IsPlayer()) {
            Debug.Log("Heal Player");
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
