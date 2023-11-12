using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class BaseDamageSource : MonoBehaviour
{
    public Rigidbody2D rb;
    [HideInInspector] public BaseDamageable host;
    [HideInInspector] public List<BaseDamageable> contactedDamageables = new List<BaseDamageable>();
    [HideInInspector] public List<BaseDamageable> hitDamageables = new List<BaseDamageable>();
    public float damageScaling = 1f;
    public float knockback = 0f;
    public bool hasLifetime = true;
    [ShowIf("hasLifetime")] public float lifetime;
    [HideInInspector] public float baseDamageScaling;
    [HideInInspector] public float damage;
    [HideInInspector] public bool active = true;
    public bool damageOverTime = false;
    [HideIf("damageOverTime")] public bool destroyOnContact = false;
    [ShowIf("damageOverTime")] public float hitRate;
    
    float hitTimer;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        baseDamageScaling = damageScaling;
        BaseDamageable selfHost = Global.FindComponent<BaseDamageable>(gameObject);
        if (selfHost) {
            Init(selfHost);
        }
        if (damageOverTime) destroyOnContact = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateTimers();
        CheckContactedDamageables();
    }

    protected virtual void CheckContactedDamageables() {
        if (!active) {
            return;
        }
        for (int i = contactedDamageables.Count-1; i >= 0; i--) {
            BaseDamageable damageable = contactedDamageables[i];
            if (hitDamageables.Contains(damageable) && !damageOverTime) continue;
            OnHit(damageable);
        }
    }

    protected virtual void UpdateTimers() {
        if (hasLifetime && lifetime > 0) {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0) {
                StartCoroutine(OnDeath());
            }
        }
        hitTimer = Mathf.Max(hitTimer-Time.deltaTime, 0f);
        if (hitTimer <= 0) {
            hitTimer = hitRate;
            active = true;
        }
    }
    public virtual void Init(BaseDamageable host) {
        this.host = host;
        damage = damageScaling * host.GetStatValue(StatType.Atk);
    }

    public virtual void OnHit(BaseDamageable damageable) {
        damageable.Damage(this);
        hitDamageables.Add(damageable);
        if (destroyOnContact) {
            StartCoroutine(OnDeath());
        }
        else if (damageOverTime){
            active = false;
        }
    }

    public virtual void Knockback(BaseDamageable damageable) {
        Vector2 dir = (damageable.transform.position - transform.position).normalized;
        damageable.rb.AddForce(dir * knockback, ForceMode2D.Impulse);
    }

    public virtual IEnumerator OnDeath() {
        active = false;
        yield return null;
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        BaseDamageable damageable = Global.FindComponent<BaseDamageable>(coll.gameObject);
        if (damageable && host && !ReferenceEquals(host.gameObject, damageable.gameObject)) {
            contactedDamageables.Add(damageable);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll) {
        BaseDamageable damageable = Global.FindComponent<BaseDamageable>(coll.gameObject);
        if (damageable) {
            contactedDamageables.Remove(damageable);
        }
    }
}
