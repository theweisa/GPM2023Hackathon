using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageable : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    virtual protected void Awake() {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
