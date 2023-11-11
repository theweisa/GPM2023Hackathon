using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [Space(4)]
    public Rigidbody2D rb;
    [Header("Variables")]
    [Space(4)]
    public float movementSpeed;
    [HideInInspector] public Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyMovement();
    }
    public void Move(InputAction.CallbackContext context) {
        moveDirection = context.ReadValue<Vector2>();
    }

    void ApplyMovement() {
        rb.velocity = moveDirection*movementSpeed;
        //rb.AddForce(Time.deltaTime*moveDirection*movementSpeed);
    }
}
