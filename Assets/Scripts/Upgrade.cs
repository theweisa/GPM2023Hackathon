using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

[CreateAssetMenu(menuName = "Upgrade/baseUpgrade")]
[System.Serializable]
public class Upgrade : ScriptableObject, IStatMod
{
    [System.Serializable] public class UpgradeStatMod {
        public string id;
        public StatModifier statMod;
        public float statLevelUp;
        public UpgradeStatMod(string id) {
            this.id = id;
        }
    }
    [SerializeField] public List<UpgradeStatMod> statMods = new List<UpgradeStatMod>();
    public Sprite image;
    public string id;
    public string description;
    public string levelUpDescription;
    public int level = 1;
    public int maxLevel = 5;
    public int spawnWeighting = 10;
    public bool hasCooldown = false;
    [ShowIf("hasCooldown")] public float cooldown;
    [HideInInspector] public float cooldownTimer;
    [HideInInspector] public BaseDamageSource hostDamageSource;
    [HideInInspector] public BaseDamageable hostDamageable;
    // Start is called before the first frame update
    public virtual void InitUpgrade() {
        if (!hasCooldown) {
            cooldown = 0;
        }
        cooldownTimer = cooldown;
    }
    public virtual void OnLevelUp() {
        level++;
        //spawnWeighting-=1;
        foreach (UpgradeStatMod mod in statMods) {
            mod.statMod.SetMod(mod.statMod.modValue+mod.statLevelUp);
        }
        if (hostDamageable) {
            hostDamageable.CalculateStats();
        }
    }
    public List<StatModifier> GetStatModsById(string id) {
        List<StatModifier> ret = new List<StatModifier>();
        foreach (UpgradeStatMod u in statMods) {
            if (u.id.ToLower() == id.ToLower()) {
                ret.Add(u.statMod);
            }
        }
        return ret;
    }

    public void AddStatMods(string modId) {
        foreach(StatModifier m in GetStatModsById(modId)) {
            hostDamageable.AddModifier(m);
        }
    }
    public void AddStatMods(string modId, BaseDamageable damageable) {
        foreach(StatModifier m in GetStatModsById(modId)) {
            damageable.AddModifier(m);
        }
    }

    // when the mod is first added to the form unit
    public virtual void OnApply(BaseDamageable damageable) {
        SetHostDamageable(damageable);
        InitUpgrade();
        AddStatMods("onApply", damageable);
    }

    // on enemy hit
    public virtual void OnHit(BaseDamageable damageable, BaseDamageSource source) {
        AddStatMods("onHit", damageable);
    }

    // on init of a damage source
    public virtual void OnDamageSource(BaseDamageSource source) {
        SetHostDamageSource(source);
        if (hostDamageable) {
            AddStatMods("onDamageSource", hostDamageable);
        }
    }

    // on killing an enemy
    public virtual void OnKill(BaseDamageable damageable, BaseDamageSource source) {
        if (hostDamageable) {
            AddStatMods("onKill", hostDamageable);
        }
    }

    // when the mod is unapplied and you need to potentially clean up
    public virtual void OnUnapply() {
        return;
    }

    public virtual void OnHostHit(BaseDamageable damageable, BaseDamageSource souce) {
        AddStatMods("onHostHit", damageable);
    }

    public virtual void OnDamageableUpdate(BaseDamageable damageable) {
        if (hasCooldown) {
            cooldownTimer = Mathf.Max(cooldownTimer-Time.deltaTime, 0f);
            if (cooldownTimer <= 0f) {
                cooldownTimer = cooldown;
                OnCooldownUp();
            }
            
        }
    }

    public virtual void OnDamageSourceUpdate(BaseDamageSource source) {
        return;
    }
    public void SetHostDamageSource(BaseDamageSource damageSource) {
        hostDamageSource = damageSource;
    }
    public void SetHostDamageable(BaseDamageable damageable) {
        hostDamageable = damageable;
    }
    public virtual void OnCooldownUp() {

    }
}
