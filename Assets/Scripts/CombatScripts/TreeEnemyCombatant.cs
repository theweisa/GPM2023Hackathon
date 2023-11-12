using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemyCombatant : EnemyCombatant
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        print("we made it");
    }
    public override void MoveCombatant()
    {
        base.FixedUpdate();
        print("we made it");
    }
}