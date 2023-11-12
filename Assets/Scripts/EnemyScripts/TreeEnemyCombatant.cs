using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemyCombatant : EnemyCombatant
{
    /*
    Vector2 moveDirection;
    public float energyYield;
    public float expYield;
    public GameObject expDrop;
    public float movementMultiplier = 1000f;
    public Transform target;
    */

    protected override void Start() {
        base.Start();
        target = SelectTarget();        
    }

    public Transform SelectTarget(){
        //get Tree Parent Object (from GameManager)
        int size = InstantiationManager.Instance.treeParent.transform.childCount;
        if(size != 0){
            //choose random child out of parent.numOfChildren
            return InstantiationManager.Instance.treeParent.transform.GetChild(Random.Range(0, size));
        }
        else{
            //return Player transform
            return PlayerManager.Instance.transform;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //if there's no current target selected, then select a new one
        if(!target){
            target = SelectTarget();
        }
    }
}