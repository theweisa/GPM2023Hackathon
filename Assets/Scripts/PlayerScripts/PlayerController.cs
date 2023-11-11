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
    public PlayerCombatant combatant;
    public Transform weapon;
    public GameObject suckMelee;
    [Header("Variables")]
    [Space(4)]
    Vector2 moveDirection;
    Vector2 fireDirection;
    
    float weaponRadius;
    

    // Start is called before the first frame update
    void Awake()
    {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        combatant = combatant ? combatant : Global.FindComponent<PlayerCombatant>(gameObject);
        weaponRadius = weapon.localPosition.magnitude;
    }

    void SetWeaponPosition() {
        fireDirection = -Global.GetRelativeMousePosition(transform.position).normalized;
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        weapon.localPosition = fireDirection * weaponRadius;
        weapon.rotation = rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyMovement();
        SetWeaponPosition();
    }
    public void Move(InputAction.CallbackContext context) {
        moveDirection = context.ReadValue<Vector2>();
    }
    public void Fire(InputAction.CallbackContext context) {
        if (!context.started) return;
        //Instantiate()
    }

    void ApplyMovement() {
        rb.velocity = moveDirection*combatant.GetStatValue(StatType.Spd);
        //rb.AddForce(Time.deltaTime*moveDirection*movementSpeed);
    }
}
