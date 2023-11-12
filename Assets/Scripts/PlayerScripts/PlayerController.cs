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
    public GameObject primaryAttack;
    public GameObject secondaryAttack;
    [Header("Variables")]
    [Space(4)]
    [Tooltip("cost per second")] public float primaryMeterCostPerSecond = 20f;
    public float secondaryMeterCost = 33f;

    // private variables
    Vector2 moveDirection;
    Vector2 fireDirection;
    float weaponRadius;
    [HideInInspector] public Transform firePoint;
    [HideInInspector] public BaseDamageSource primaryRef;
    

    // Start is called before the first frame update
    void Awake()
    {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        combatant = combatant ? combatant : Global.FindComponent<PlayerCombatant>(gameObject);
        weaponRadius = weapon.localPosition.magnitude;
        firePoint = weapon.Find("FirePoint").transform;
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
    public void FirePrimary(InputAction.CallbackContext context) {
        if (context.started && combatant.meter.currentMeter > 0f) {
            PlayerPrimary primary = Instantiate(primaryAttack, firePoint.position, weapon.rotation, weapon).GetComponent<PlayerPrimary>();
            primary.Init(combatant);
            primaryRef = primary;
        }
        else if (context.canceled) {
            EndPrimary();
        }
    }

    public void EndPrimary() {
        if (!primaryRef) return;
        StartCoroutine(primaryRef.OnDeath());
    }

    public void FireSecondary(InputAction.CallbackContext context) {
        if (!context.started || combatant.meter.currentMeter <= 0f) return;
        PlayerSecondary secondary = Instantiate(secondaryAttack, firePoint.position, weapon.rotation, InstantiationManager.Instance.damageSourceParent).GetComponent<PlayerSecondary>();
        secondary.InitProjectile(combatant, fireDirection);
        combatant.meter.DepleteMeter(secondary.meterCost);
    }

    void ApplyMovement() {
        if (!combatant.canMove) return;
        rb.velocity = moveDirection*combatant.GetStatValue(StatType.Spd);
        //rb.AddForce(Time.deltaTime*moveDirection*movementSpeed);
    }
}
