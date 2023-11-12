using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

public class BaseDamageable : MonoBehaviour
{
    public List<Stat> stats = new List<Stat>() {
        new Stat(StatType.Hp), new Stat(StatType.Atk), new Stat(StatType.Spd)
    };
    public List<AudioChild> sounds = new List<AudioChild>();
    public List<Upgrade> upgrades = new List<Upgrade>();
    [Header("References")]
    [Space(4)]
    public Meter healthBar;
    public SpriteRenderer sprite;
    public SpriteRenderer shadow;
    public Animator anim;
    public Rigidbody2D rb;
    public Collider2D coll;
    [HideInInspector] public bool active = true;
    // Start is called before the first frame update
    virtual protected void Awake() {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        healthBar = healthBar ? healthBar : Global.FindComponent<Meter>(gameObject);
        sprite = sprite ? sprite : Global.FindComponent<SpriteRenderer>(gameObject);
        shadow = shadow ? shadow : transform.Find("Shadow").GetComponent<SpriteRenderer>();
        coll = coll ? coll : Global.FindComponent<Collider2D>(gameObject);
        anim = anim ? anim : Global.FindComponent<Animator>(gameObject);
        InitStats();
        StartCoroutine(OnSpawn());
        if (healthBar) {
            healthBar.maxMeter = GetStat(StatType.Hp).baseValue;
            healthBar.currentMeter = healthBar.maxMeter;
        }
        foreach (Upgrade u in upgrades) {
            u.OnApply(this);
        }
        BaseDamageSource source = Global.FindComponent<BaseDamageSource>(gameObject);
        if (source) {
            source.Init(this);
        }
    }
    virtual protected void Start() {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateStats();
        UpdateUpgrades();
    }

    protected virtual void FixedUpdate()
    {
        
    }

    virtual public void Damage(BaseDamageSource source) {
        float damageTaken = source.damage;
        Stat dr = GetStat(StatType.DmgReduction);
        if (dr != null) {
            damageTaken *= 1+dr.value;
        }
        if (damageTaken <= 0) return;
        Stat hp = ApplyDamage(damageTaken);
        UpgradesOnHit(source);
        UpgradesOnSelfHit(source);
        source.Knockback(this);
        if (hp.value <= 0 && active) {
            UpgradesOnKill(source);
            StartCoroutine(OnDeath());
        }
    }

    virtual public Stat ApplyDamage(float damageTaken) {
        Stat hp = GetStat(StatType.Hp);
        hp.SetValue(hp.value-damageTaken);
        InitDamageText(damageTaken);
        if (healthBar) {
            healthBar.SetMeter(hp.value, hp.baseValue);
        }
        Debug.Log($"{gameObject.name} hp: {hp.value} after {damageTaken} damage taken");
        return hp;
    }

    virtual public void InitDamageText(float dmg) {
        DamageText dmgTxt = Instantiate(ResourceManager.Instance.GetTextByName("DamageText"), transform.position, Quaternion.identity, InstantiationManager.Instance.otherParent).GetComponent<DamageText>();
        dmgTxt.Init(dmg);
    }

    virtual public void Heal(BaseDamageSource source) {
        Heal(source.damage);
    }

    virtual public void Heal(float amount) {
        if (amount <= 0) return;
        Stat hp = GetStat(StatType.Hp);
        hp.SetValue(Mathf.Min(hp.value+amount, hp.baseValue));
        //Debug.Log($"{hp.value}/{hp.baseValue}");
        DamageText healTxt = Instantiate(ResourceManager.Instance.GetTextByName("DamageText"), transform.position, Quaternion.identity, InstantiationManager.Instance.otherParent).GetComponent<DamageText>();
        healTxt.Init(amount, DamageText.TextType.Heal);
        if (healthBar) {
            healthBar.SetMeter(hp.value, hp.baseValue);
        }
        Debug.Log($"{gameObject.name} hp: {hp.value} after {amount} healing");
    }
    
    virtual public IEnumerator OnSpawn() {
        yield return null;
    }

    virtual public IEnumerator OnDeath() {
        if (active) {
            coll.enabled = false;
            active = false;
            Global.Fade(sprite, 0.4f);
            Global.Fade(shadow, 0.4f);
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }
    }
    
    public bool IsPlayer() {
        return gameObject.CompareTag("Player");
    }

    public void AddUpgrade(Upgrade newUpgrade) {
        Upgrade upgrade = ContainsUpgrade(newUpgrade);
        if (upgrade) {
            upgrade.OnLevelUp();
            return;
        }
        Upgrade clone = newUpgrade.Clone();
        clone.OnApply(this);
        upgrades.Add(clone);
    }

    public Upgrade ContainsUpgrade(Upgrade newUpgrade) {
        foreach (Upgrade upgrade in upgrades) {
            if (newUpgrade.id == upgrade.id) {
                return upgrade;
            }
        }
        return null;
    }

    public void UpdateUpgrades() {
        foreach (Upgrade upgrade in upgrades) {
            upgrade.OnDamageableUpdate(this);
        }
    }
    public void UpgradesOnHit(BaseDamageSource source) {
        foreach (Upgrade u in source.upgrades) {
            u.OnHit(this, source);
        }
    }
    public void UpgradesOnSelfHit(BaseDamageSource source) {
        foreach (Upgrade u in upgrades) {
            u.OnHostHit(this, source);
        }
    }
    public void UpgradesOnKill(BaseDamageSource source) {
        foreach (Upgrade u in source.upgrades) {
            u.OnKill(this, source);
        }
    }

    #region stats
    virtual protected void InitStats() {
        foreach (Stat s in stats) {
            s.Init();
        }
    }

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

    public bool AddModifier(StatModifier mod) {
        if (mod == null) return false;
        foreach (Stat stat in stats) {
            if (stat.type == mod.modStat) {
                stat.AddModifier(mod);
                return true;
            }
        }
        return false;
    }

    public bool RemoveModifierFromSource(IStatMod source) {
        bool retval = false;
        foreach (Stat stat in stats) {

            if (stat.RemoveModifierFromSource(source)) retval = true;
        }
        return retval;
    }

    public bool RemoveModifierFromReference(StatModifier mod) {
        bool retval = false;
        foreach (Stat stat in stats) {
            if (stat.type == mod.modStat) {
                if (stat.RemoveModifierFromReference(mod)) retval = true;
            }
        }
        return retval;
    }

    public void RemoveAllModifiers()
    {
        foreach(Stat stat in stats)
        {
            stat.modifiers.Clear();
        }
    }

    public Stat GetStatObject(StatType type)
    {
        foreach (Stat stat in stats)
        {
            if (stat.type == type)
            {
                return stat;
            }
        }
        return null;
    }

    public float GetBaseStat(StatType type)
    {
        foreach (Stat stat in stats)
        {
            if (stat.type == type)
            {
                return stat.baseValue;
            }
        }
        return 0;
    }

    public Stat SetBaseStat(StatType type, float amount) {
        foreach (Stat stat in stats)
        {
            if (stat.type == type)
            {
                stat.baseValue = amount;
                return stat;
            }
        }
        return null;
    }

    public Stat SetStat(StatType type, float amount) {
        foreach (Stat stat in stats)
        {
            if (stat.type == type)
            {
                stat.value = amount;
                return stat;
            }
        }
        return null;
    }

    public Stat CopyStat(Stat cStat) {
        for (int i = 0; i < stats.Count; i++) {
            if (stats[i].type == cStat.type) {
                stats[i] = cStat;
                return stats[i];
            }
        }
        return null;
    }

    public bool AddStat(StatType newStat, float value=0) {
        foreach (Stat stat in stats) {
            if (stat.type == newStat) {
                return false;
            }
        }
        stats.Add(new Stat(newStat, value));
        return true;
    }

    public bool AddStat(Stat newStat) {
        foreach (Stat stat in stats) {
            if (stat.type == newStat.type) {
                return false;
            }
        }
        stats.Add(newStat);
        return true;
    }

    public void SetToBaseStats() {
        foreach (Stat stat in stats) {
            stat.value = stat.baseValue;
        }
    }

    public void PrintStats()
    {
        foreach(Stat stat in stats)
        {
            string type = stat.type.ToString();
            Debug.Log($"{type}: {stat.value}/{stat.baseValue}");
        }
    }

    public void UpdateStats() {
        foreach (Stat stat in stats) {
            stat.Update();
        }
    }

    public void CalculateStats() {
        foreach (Stat stat in stats) {
            stat.CalculateStat();
            if (stat.type == StatType.Hp) {
                healthBar.SetMeter(stat.value, stat.baseValue);
            }
        }
    }

    public bool ContainsModifier(StatModifier mod) {
        foreach (Stat stat in stats) {
            if (stat.type != mod.modStat) continue;
            return stat.ContainsModifier(mod);
        }
        return false;
    }

    #endregion
}
