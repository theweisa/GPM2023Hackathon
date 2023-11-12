using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyCombatant
{
    public float fireRate = 2f;
    public float fireRange = 8f;
    protected float fireTimer;
    public GameObject projectile;
    protected Vector2 fireDirection;
    public bool stopToAttack = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        fireTimer = fireRate;   
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        fireDirection = (target.position - transform.position).normalized;
        UpdateFireRate();
    }
    protected virtual void UpdateFireRate() {
        if (!InRange()) {
            canMove = true;
            return;
        }
        if (stopToAttack) {
            canMove = false;
        }
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f) {
            Fire();
            fireTimer = fireRate;
        }
    }
    protected virtual void Fire() {
        BaseProjectile proj = Instantiate(projectile, transform.position, Quaternion.identity, InstantiationManager.Instance.damageSourceParent).GetComponent<BaseProjectile>();
        proj.InitProjectile(this, fireDirection);
    }

    protected virtual bool InRange() {
        return target && Vector2.Distance(transform.position, target.transform.position) <= fireRange;
    }   
}
