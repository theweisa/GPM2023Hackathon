using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/purifyGasses")]
public class PurifyGasses : Upgrade
{
    public float healChance = 0.3f;
    public float healChanceOnLevel = 0.1f;
    public float healOnKill = 10f;
    public float healOnLevel = 3f;
    // Start is called before the first frame update
    public override void OnKill(BaseDamageable damageable, BaseDamageSource source)
    {
        base.OnKill(damageable, source);
        if (!hostDamageable) return;
        if (Random.Range(0f, 1f) > healChance) return;
        hostDamageable.Heal(healOnKill);
    }
    public override void OnLevelUp()
    {
        base.OnLevelUp();
        healChance += healChanceOnLevel;
        healOnKill += healOnLevel;
    }
}
