using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/chemicalMask")]
public class ChemicalMaskUpgrade : Upgrade
{
    public float damage = 1f;
    public float healOnKill = 10f;
    public float healOnLevel = 3f;
    public float damageTakenOnLevel = 1f;

    // Start is called before the first frame update
    public override void OnCooldownUp() {
        base.OnCooldownUp();
        if (!hostDamageable) return;
        hostDamageable.ApplyDamage(damage);
    }
    public override void OnKill(BaseDamageable damageable, BaseDamageSource source)
    {
        Debug.Log(" on kill");
        base.OnKill(damageable, source);
        if (!hostDamageable) return;
        hostDamageable.Heal(healOnKill);
    }
    public override void OnLevelUp()
    {
        base.OnLevelUp();
        healOnKill += healOnLevel;
        damage += damageTakenOnLevel;
    }
}
