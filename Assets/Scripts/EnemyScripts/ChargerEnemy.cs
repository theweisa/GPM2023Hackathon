using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

// fly across the screen
public class ChargerEnemy : EnemyCombatant
{
    public bool chargeAtTarget = false;
    bool dirGotten = false;
    [HideIf("chargeAtPlayer")] public float chargeAngle;
    public virtual void SetChargeAngle(float newAngle) {
        chargeAngle = newAngle;
        chargeAtTarget = false;
        dirGotten = false;
        GetMoveDirection();
    }
    public virtual void ChargeAtTarget(Transform newTarget) {
        target = newTarget;
        chargeAtTarget = true;
        dirGotten = false;
        GetMoveDirection();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void GetMoveDirection()
    {
        if (dirGotten) return;
        dirGotten = true;
        if (!chargeAtTarget) {
            moveDirection = new Vector2(Mathf.Cos(chargeAngle*Mathf.Deg2Rad), Mathf.Sin(chargeAngle*Mathf.Deg2Rad)).normalized;
        }
        else {
            base.GetMoveDirection();
        }
    }
}
