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
    public int level = 1;
    public int maxLevel = 5;
    public int spawnWeighting = 10;
    public bool hasCooldown = false;
    [ShowIf("hasCooldown")] public float cooldown;
    [HideInInspector] public BaseDamageSource hostDamageSource;
    [HideInInspector] public BaseDamageable hostDamageable;
    float baseCooldown;
    // Start is called before the first frame update
    public virtual void InitUpgrade() {
        if (!hasCooldown) {
            cooldown = 0;
        }
        baseCooldown = cooldown;
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
    public StatModifier GetStatModById(string id) {
        foreach (UpgradeStatMod u in statMods) {
            if (u.id.ToLower() == id.ToLower()) {
                return u.statMod;
            }
        }
        return null;
    }

    // when the mod is first added to the form unit
    public virtual void OnApply(BaseDamageable damageable) {
        SetHostDamageable(damageable);
        InitUpgrade();
        damageable.AddModifier(GetStatModById("onApply"));
    }

    // on enemy hit
    public virtual void OnHit(BaseDamageable damageable, BaseDamageSource source) {
        damageable.AddModifier(GetStatModById("onHit"));
    }

    // on init of a damage source
    public virtual void OnDamageSource(BaseDamageSource source) {
        SetHostDamageSource(source);
        if (hostDamageable) {
            hostDamageable.AddModifier(GetStatModById("onDamageSource"));
        }
    }

    // on killing an enemy
    public virtual void OnKill(BaseDamageable damageable, BaseDamageSource source) {
        if (hostDamageable) {
            hostDamageable.AddModifier(GetStatModById("onKill"));
        }
    }

    // when the mod is unapplied and you need to potentially clean up
    public virtual void OnUnapply() {
        return;
    }

    public virtual void OnHostHit(BaseDamageable damageable, BaseDamageSource souce) {
        damageable.AddModifier(GetStatModById("onHostHit"));
    }

    public virtual void OnDamageableUpdate(BaseDamageable damageable) {
        if (hasCooldown) {
            cooldown = Mathf.Max(cooldown-Time.deltaTime, 0f);
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
}
