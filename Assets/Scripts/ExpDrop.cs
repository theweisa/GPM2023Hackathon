using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDrop : MonoBehaviour
{
    public float exp;
    public float lifetime;
    public void Init(EnemyCombatant enemy) {
        exp = enemy.expYield;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifetimeRoutine());
    }

    IEnumerator LifetimeRoutine() {
        yield return new WaitForSeconds(lifetime);
        Global.Fade(gameObject, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Player")) {
            Debug.Log("collect");
            PlayerManager.Instance.combatant.AddExp(exp);
            Destroy(gameObject);
        }
        
    }
}
