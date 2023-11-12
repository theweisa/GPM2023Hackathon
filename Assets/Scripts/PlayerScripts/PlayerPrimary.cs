using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimary : BaseMelee
{
    public float meterPerSecond = 20f;
    private AttackMeter meterRef;

    public override void Init(BaseDamageable host) {
        base.Init(host);
        PlayerCombatant player = host.GetComponent<PlayerCombatant>();
        if (player) {
            meterRef = player.meter;
        }
    }
    // Start is called before the first frame update
    protected override void Update() {
        base.Update();
        if (!meterRef) return;
        meterRef.DepleteMeter(meterPerSecond*Time.deltaTime, LeanTweenType.notUsed);
        if (meterRef.currentMeter <= 0f) {
            StartCoroutine(OnDeath());
        }
    }
}
