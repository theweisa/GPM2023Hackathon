using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/thornUpgrade")]
public class ThornsUpgrade : Upgrade
{
    public float thornsDamage = 1f;
    public float levelDamage = 1f;
    public override void OnHostHit(BaseDamageable damageable, BaseDamageSource source)
    {
        base.OnHostHit(damageable, source);
        if (!source.host) return;
        source.host.ApplyDamage(thornsDamage);
        //enemy
    }
    public override void OnLevelUp()
    {
        base.OnLevelUp();
        thornsDamage += levelDamage;
    }
}
