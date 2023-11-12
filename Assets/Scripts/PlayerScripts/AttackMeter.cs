using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMeter : Meter
{

    public float regenPerSecond = 30f;
    public float regenCooldown = 1f;
    float regenTimer;
    // Start is called before the first frame update
    // Update is called once per frame
    protected override void Update()
    {
        if (regenTimer <= 0f && currentMeter < maxMeter) {
            currentMeter = Mathf.Min(currentMeter + regenPerSecond * Time.deltaTime, maxMeter);
            fill.localScale = new Vector3(currentMeter/maxMeter, fill.localScale.y, fill.localScale.z);
        }
        else if (regenTimer > 0) {
            regenTimer = Mathf.Max(regenTimer-Time.deltaTime, 0f);
        }
    }

    public override void AddMeter(float amt, LeanTweenType ease=LeanTweenType.easeOutExpo) {
        regenTimer = regenCooldown;
        base.AddMeter(amt, ease);
    }
}
