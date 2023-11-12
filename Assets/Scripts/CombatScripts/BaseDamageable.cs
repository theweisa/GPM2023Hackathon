using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class BaseDamageable : MonoBehaviour
{
    public List<Stat> stats = new List<Stat>() {
        new Stat(StatType.Hp), new Stat(StatType.Atk), new Stat(StatType.Spd)
    };
    public Meter healthBar;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    protected virtual void Start() {

    }
    virtual protected void Awake() {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        InitStats();
        if (healthBar) {
            healthBar.maxMeter = GetStat(StatType.Hp).baseValue;
            healthBar.currentMeter = healthBar.maxMeter;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        
    }

    virtual public void Damage(BaseDamageSource source) {
        float damageTaken = source.damage;
        if (damageTaken <= 0) return;
        Stat hp = GetStat(StatType.Hp);
        hp.SetValue(hp.value-damageTaken);
        if (healthBar) {
            healthBar.SetMeter(hp.value, hp.baseValue);
        }
        source.Knockback(this);
        Debug.Log($"{gameObject.name} hp: {hp.value} after {damageTaken} damage taken");
        if (hp.value <= 0) {
            StartCoroutine(OnDeath());
        }
    }

    virtual public void Heal(BaseDamageSource source) {
        Heal(source.damage);
    }

    virtual public void Heal(float amount) {
        if (amount <= 0) return;
        Stat hp = GetStat(StatType.Hp);
        hp.SetValue(hp.value+amount);
        if (healthBar) {
            healthBar.SetMeter(hp.value, hp.baseValue);
        }
        Debug.Log($"{gameObject.name} hp: {hp.value} after {amount} healing");
    }

    virtual public IEnumerator OnDeath() {
        yield return null;
    }

    virtual protected void InitStats() {
        foreach (Stat s in stats) {
            s.Init();
        }
    }

    #region stats

    public Stat GetStat(StatType sType) {
        return stats.FirstOrDefault(s => s.type == sType);
    }
    public Stat SetStatValue(StatType sType, float newValue) {
        Stat s = GetStat(sType);
        s.value = newValue;
        return s;
    }

    public float GetStatValue(StatType sType) {
        Stat stat = GetStat(sType);
        if (stat != null) return stat.value;
        Debug.Log($"ERROR: {gameObject.name} does not have stat {sType}");
        return -1f;
    }

    public bool IsPlayer() {
        return gameObject.CompareTag("Player");
    }

    #endregion
}
