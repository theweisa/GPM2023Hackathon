using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDamageable : BaseDamageable
{
    [Header("Tree Variables")]
    [Space(4)]
    public GameObject treePulse;
    public float pulseRate;
    float pulseTimer=0f;
    // Start is called before the first frame update

    protected override void Awake() {
        base.Awake();
        Stat hp = GetStat(StatType.Hp);
        Stat atk = GetStat(StatType.Atk);
        //Debug.Log("hp mod: " + PlayerManager.Instance.combatant.GetStatValue(StatType.TreeHpMult));
        hp.SetStat(hp.baseValue*(1+PlayerManager.Instance.combatant.GetStatValue(StatType.TreeHpMult)));
        atk.SetStat(atk.baseValue+PlayerManager.Instance.combatant.GetStatValue(StatType.TreeAtkUp));
        pulseRate *= 1+PlayerManager.Instance.combatant.GetStatValue(StatType.TreeFireRateMult);
        if (PlayerManager.Instance.combatant) {
            
        }
        
    }
    public override IEnumerator OnSpawn()
    {
        Vector3 baseScale = sprite.transform.localScale;
        sprite.transform.localScale = Vector3.zero;
        LeanTween.scale(sprite.gameObject, baseScale, 1f).setEaseOutBounce();
        Vector3 shadowScale = shadow.transform.localScale;
        shadow.transform.localScale = Vector3.zero;
        LeanTween.scale(shadow.gameObject, shadowScale, 1f).setEaseInExpo();
        yield return base.OnSpawn();
    }
    protected override void Update()
    {
        base.Update();
        UpdateTimers();
    }

    void UpdateTimers() {
        pulseTimer = Mathf.Max(pulseTimer-Time.deltaTime, 0f);
        if (pulseTimer <= 0f) {
            Pulse();
            pulseTimer = pulseRate;
        }
    }

    void Pulse() {
        TreePulseDamageSource pulse = Instantiate(treePulse, transform.position, Quaternion.identity, InstantiationManager.Instance.damageSourceParent).GetComponent<TreePulseDamageSource>();
        pulse.Init(this);
    }
}
