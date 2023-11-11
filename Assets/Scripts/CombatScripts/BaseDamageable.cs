using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseDamageable : MonoBehaviour
{
    public List<Stat> stats = new List<Stat>() {
        new Stat(StatType.Hp), new Stat(StatType.Atk), new Stat(StatType.Spd)
    };
    public Rigidbody2D rb;
    // Start is called before the first frame update
    virtual protected void Awake() {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        InitStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void Damage(BaseDamageSource source) {
        float damageTaken = source.damage;
        //if (damageTaken <= 0) return;
        Stat hp = GetStat(StatType.Hp);
        hp.SetValue(hp.value-damageTaken);
        KnockBack(source);
        Debug.Log($"{gameObject.name} hp: {hp.value} after {damageTaken} damage taken");
        if (hp.value <= 0) {
            StartCoroutine(OnDeath());
        }
    }

    virtual public void KnockBack(BaseDamageSource source) {
        
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
