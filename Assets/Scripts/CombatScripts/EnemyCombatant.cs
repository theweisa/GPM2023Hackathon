using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : BaseCombatant
{
    Vector2 moveDirection;
    public float energyYield;
    public float expYield;
    public GameObject expDrop;
    public float movementMultiplier = 1000f;
    public Transform target;

    protected override void Awake() {
        base.Awake();
        target = PlayerManager.Instance.transform;
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveCombatant();
    }
    public override IEnumerator OnSpawn() {
        Global.Appear(sprite, 0.3f);
        Global.Appear(shadow, 0.3f);
        yield return base.OnSpawn();
    }

    public virtual void GetMoveDirection() {
        moveDirection = target.position - transform.position;
        moveDirection.Normalize();
    }

    public virtual void MoveCombatant() {
        if (!canMove) return;
        GetMoveDirection();
        //rb.velocity = GetStatValue(StatType.Spd) * moveDirection;
        rb.AddForce(GetStatValue(StatType.Spd) * movementMultiplier * moveDirection * Time.deltaTime);
    }

    public override IEnumerator OnDeath()
    {
        rb.velocity = Vector2.zero;
        canMove = false;
        ExpDrop drop = Instantiate(expDrop, transform.position, Quaternion.identity, InstantiationManager.Instance.otherParent).GetComponent<ExpDrop>();
        drop.Init(this);
        PlayerManager.Instance.combatant.AddEnergy(expYield);
        yield return base.OnDeath();
    }
}
