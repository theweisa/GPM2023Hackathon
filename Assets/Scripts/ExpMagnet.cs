using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpMagnet : MonoBehaviour
{
    private bool move = false;
    public float attractMagnitude = 1000f;
    public Rigidbody2D rb;
    public float radius;
    void Start() {
        rb = rb ? rb : GetComponentInParent<Rigidbody2D>();
        radius = GetComponent<CircleCollider2D>().radius;
    }
    void FixedUpdate() {
        if (!move) return;
        float distance = Vector2.Distance(transform.position, PlayerManager.Instance.transform.position);
        Vector2 direction = (PlayerManager.Instance.transform.position - transform.position).normalized;
        Debug.Log(distance/radius);
        rb.AddForce(direction*(distance/radius)*attractMagnitude*Time.deltaTime);
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D coll) {
        if (!coll.CompareTag("Player")) return;
        move = true;
    }
    void OnTriggerExit2D(Collider2D coll) {
        if (!coll.CompareTag("Player")) return;
        move = false;
    }
}
