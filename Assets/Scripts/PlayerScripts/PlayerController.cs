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
    SpriteRenderer weaponSprite;
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
        weaponSprite = Global.FindComponent<SpriteRenderer>(weapon.gameObject);
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
        CheckFlip();
        if (!primaryRef && AudioManager.Instance.GetSound("primary").isPlaying) {
            EndPrimary();
        }
    }

    void CheckFlip() {
        combatant.sprite.flipX = fireDirection.x > 0f;
        weaponSprite.flipY = fireDirection.x <= 0f;
        weaponSprite.sortingOrder = weapon.rotation.eulerAngles.z > 35 && weapon.rotation.eulerAngles.z < 125 ? combatant.sprite.sortingOrder-1 : combatant.sprite.sortingOrder+1;   
    }
    public void Move(InputAction.CallbackContext context) {
        moveDirection = context.ReadValue<Vector2>();
        if (moveDirection != Vector2.zero) {
            combatant.anim.Play("playerRun", -1, 0f);
        }
        else {
            combatant.anim.Play("playerIdle", -1, 0f);
        }
    }
    public void FirePrimary(InputAction.CallbackContext context) {
        if (context.started && combatant.meter.currentMeter > 0f && Time.timeScale != 0) {
            PlayerPrimary primary = Instantiate(primaryAttack, firePoint.position, weapon.rotation, weapon).GetComponent<PlayerPrimary>();
            primary.Init(combatant);
            primaryRef = primary;
            AudioManager.Instance.PlaySound("primary");
        }
        else if (context.canceled) {
            EndPrimary();
        }
    }

    public void EndPrimary() {
        AudioManager.Instance.PlaySound("primaryOff");
        AudioManager.Instance.StopSound("primary");
        if (primaryRef) {
            StartCoroutine(primaryRef.OnDeath());
        }
    }

    public void FireSecondary(InputAction.CallbackContext context) {
        if (!context.started || combatant.meter.currentMeter <= 0f || Time.timeScale == 0) return;
        PlayerSecondary secondary = Instantiate(secondaryAttack, firePoint.position, weapon.rotation, InstantiationManager.Instance.damageSourceParent).GetComponent<PlayerSecondary>();
        secondary.InitProjectile(combatant, fireDirection);
        AudioManager.Instance.PlayAudioChild("secondary", combatant.sounds);
        combatant.meter.DepleteMeter(secondary.meterCost);
    }

    void ApplyMovement() {
        if (!combatant.canMove) return;
        rb.velocity = moveDirection*combatant.GetStatValue(StatType.Spd);
        //rb.AddForce(Time.deltaTime*moveDirection*movementSpeed);
    }
}
