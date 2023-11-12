using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/healUpgrade")]
public class HealUpgrade : Upgrade
{
    float healAmount;
    float healOnLevel;
    public override void OnApply(BaseDamageable damageable)
    {
        base.OnApply(damageable);
        damageable.Heal(healAmount);
    }
    public override void OnLevelUp()
    {
        base.OnLevelUp();
        healAmount += 10;
        hostDamageable.Heal(healAmount);
    }

}
