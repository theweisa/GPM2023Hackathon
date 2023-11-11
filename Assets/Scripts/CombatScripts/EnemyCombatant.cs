using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : BaseCombatant
{
    Vector2 moveDirection;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCombatant();
    }
    void GetMoveDirection() {
        moveDirection = PlayerManager.Instance.transform.position - transform.position;
        moveDirection.Normalize();
    }

    void MoveCombatant() {
        GetMoveDirection();
        rb.velocity = GetStatValue(StatType.Spd) * moveDirection;
    }
}
