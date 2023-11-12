using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : BaseCombatant
{
    protected Vector2 moveDirection;
    public float energyYield;
    public float expYield;
    public GameObject expDrop;
    public float movementMultiplier = 1000f;
    public Transform target;
    public bool targetTree = false;
    public int tier = 1;

    protected override void Awake() {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        UpdateTarget();
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateTarget();
        MoveCombatant();
        sprite.flipX = target.position.x > transform.position.x;
    }
    public override IEnumerator OnSpawn() {
        Global.Appear(sprite, 0.3f);
        Global.Appear(shadow, 0.3f, shadow.color.a);
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

    protected virtual void UpdateTarget() {
        if (targetTree) {
            int size = InstantiationManager.Instance.treeParent.transform.childCount;
            if (size != 0 && (!target || ReferenceEquals(target, PlayerManager.Instance.transform))) {
                target = InstantiationManager.Instance.treeParent.transform.GetChild(Random.Range(0, size));
            }
            else {
                target = PlayerManager.Instance.transform;
            }
        }
        else if (!target) {
            target = PlayerManager.Instance.transform;
        }
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

    public Transform SelectTarget() {
        //get Tree Parent Object (from GameManager)
        if(targetTree){
            int size = InstantiationManager.Instance.treeParent.transform.childCount;
            if (size != 0) {
                //choose random child out of parent.numOfChildren
                return InstantiationManager.Instance.treeParent.transform.GetChild(Random.Range(0, size));
            }
        }
        //return Player transform
        return PlayerManager.Instance.transform;
    }
}
