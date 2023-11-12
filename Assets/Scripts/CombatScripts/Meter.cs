using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour
{
    public Transform fill;
    public Transform trail;

    public float maxMeter = 100f;
    public float currentMeter;
    bool depleting=false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentMeter = maxMeter;
        fill.localScale = new Vector3(currentMeter/maxMeter, fill.localScale.y, fill.localScale.z);
        trail.localScale = fill.localScale;
    }
    // Update is called once per frame
    protected virtual void Update()
    {

    }
    public virtual void SetMeter(float newAmt, float maxAmt, LeanTweenType ease=LeanTweenType.linear) {
        maxMeter = maxAmt;
        currentMeter = Mathf.Clamp(newAmt, 0f, maxAmt);
        StartCoroutine(UpdateMeter(ease));
    }
    public virtual void AddMeter(float amt, LeanTweenType ease=LeanTweenType.linear) {
        SetMeter(currentMeter+amt, maxMeter, ease);
    }
    public virtual void DepleteMeter(float amt, LeanTweenType ease=LeanTweenType.linear) {
        AddMeter(-amt, ease);
    }
    protected virtual IEnumerator UpdateMeter(LeanTweenType ease=LeanTweenType.linear) {
        if (ease != LeanTweenType.notUsed) {
            depleting = true;
            LeanTween.value(fill.gameObject, (float val)=>{ 
                SetScaleX(fill, val);
                depleting = true;
            }, fill.localScale.x, currentMeter/maxMeter, 0.3f).setEase(ease).setOnComplete(()=>{depleting=false;}); 
            //LeanTween.scaleX(fill.gameObject, currentMeter/maxMeter, 0.3f).setEase(ease).setOnComplete(()=>depleting=false);
            yield return MeterTrail();
        }
        else {
            SetScaleX(fill, currentMeter/maxMeter);
            yield return MeterTrail();
        }
    }
    IEnumerator MeterTrail() {
        yield return new WaitForSeconds(0.5f);
        if (!depleting) {
            LeanTween.scaleX(trail.gameObject, fill.localScale.x, 0.3f).setEaseOutQuart();
        }
    }
    /*
    void UpdateBossHealthbar(float baseHealthPoints, float healthPoints, float damage) {
        LeanTween.value(bossHealth.gameObject, (float val) => {
            bossHealth.sizeDelta = new Vector2(val, bossHealth.sizeDelta.y);
        }, bossHealth.sizeDelta.x, Mathf.Max(maxHealthWidth*(healthPoints/baseHealthPoints), 0f), 0.2f);
        //bossHealth.sizeDelta = new Vector2(Mathf.Max(maxHealthWidth * (baseHealthPoints / healthPoints), 0f), bossHealth.sizeDelta.y);
        if (!delayedBar) {
            delayedBar = true;
            StartCoroutine(HealthBarDelay());
        }
    }

    IEnumerator HealthBarDelay() {
        Debug.Log("start healthbar delay");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("begin delay deplete");
        LeanTween.value(healthDelay.gameObject, (float val) => {
            healthDelay.sizeDelta = new Vector2(val,bossHealth.sizeDelta.y);
        }, healthDelay.sizeDelta.x, bossHealth.sizeDelta.x, 1f).setEase(LeanTweenType.easeOutQuart);
        delayedBar = false;
    }
    */

    public void SetScaleX(Transform t, float x) {
        t.localScale = new Vector3(x, t.localScale.y, t.localScale.z);
    }
}
