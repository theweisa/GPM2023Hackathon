using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/speedUpgrade")]
public class SpeedUpgrade : Upgrade
{
    public override void OnApply(BaseDamageable damageable)
    {
        base.OnApply(damageable);
        damageable.AddModifier(GetStatModById("onApply"));
    }
}
