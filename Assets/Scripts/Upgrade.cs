using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Upgrade/baseUpgrade")]
[System.Serializable]
public class Upgrade : ScriptableObject, IStatMod
{
    public Sprite image;
    public string id;
    public string description;
    public int level = 0;
    public int spawnWeighting = 5;
    public bool hasCooldown = false;
    [ShowIf("hasCooldown")] public float cooldown;
    [HideInInspector] public BaseDamageSource hostDamageSource;
    [HideInInspector] public BaseDamageable hostDamageable;
    float baseCooldown;
    // Start is called before the first frame update
    void InitUpgrade() {
        if (!hasCooldown) {
            cooldown = 0;
        }
        baseCooldown = cooldown;
    }
    public virtual void LevelUp() {
        level++;
    }

    // when the mod is first added to the form unit
    public virtual void OnApply() {
        InitUpgrade();
        return;
    }

    // on enemy hit
    public virtual void OnHit(BaseDamageable damageable, BaseDamageSource source) {
        return;
    }

    // on init of a damage source
    public virtual void OnDamageSource(BaseDamageSource source) {
        return;
    }

    // on killing an enemy
    public virtual void OnKill(BaseDamageable damageable, BaseDamageSource source) {
        return;
    }

    // when the mod is unapplied and you need to potentially clean up
    public virtual void OnUnapply() {
        return;
    }

    // if you need something actively on update
    public virtual void OnUpdate() {
        return;
    }

    public void SetHostDamageSource(BaseDamageSource damageSource) {
        hostDamageSource = damageSource;
    }
    public void SetHostDamageable(BaseDamageable damageable) {
        hostDamageable = damageable;
    }

    public virtual void OnDamageableUpdate(BaseDamageable damageable) {
        if (hasCooldown) {
            cooldown = Mathf.Max(cooldown-Time.deltaTime, 0f);
        }
    }

    public virtual void OnDamageSourceUpdate(BaseDamageSource source) {
        return;
    }

    public void GiveUpgrade() {
        PlayerManager.Instance.combatant.AddUpgrade(this);
    }
}
