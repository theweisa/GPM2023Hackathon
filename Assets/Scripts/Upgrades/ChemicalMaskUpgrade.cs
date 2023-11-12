using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/chemicalMask")]
public class ChemicalMaskUpgrade : Upgrade
{
    public float damage = 1f;
    public float damageTakenOnLevel = 1f;

    // Start is called before the first frame update
    public override void OnCooldownUp() {
        base.OnCooldownUp();
        if (!hostDamageable) return;
        hostDamageable.ApplyDamage(damage);
    }
}
