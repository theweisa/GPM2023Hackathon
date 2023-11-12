using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDrop : MonoBehaviour
{
    public float exp;
    public float lifetime;
    public float initialImpactForce = 13f;
    public Rigidbody2D rb;
    public AudioSource pickup;
    public void Init(EnemyCombatant enemy) {
        exp = enemy.expYield;
    }
    // Start is called before the first frame update
    void Awake()
    {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        rb.AddForce(initialImpactForce*Random.insideUnitCircle.normalized, ForceMode2D.Impulse);
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
            StartCoroutine(PickUp());
        }
    }
    IEnumerator PickUp() {
        pickup.Play();
        GetComponent<Collider2D>().enabled = false;
        Global.FindComponent<SpriteRenderer>(gameObject).enabled = false;
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
