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
