using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMeter : MonoBehaviour
{
    public Transform fill;
    public Transform trail;

    public float maxMeter = 100f;
    public float currentMeter;
    public float regenPerSecond = 30f;
    public float regenCooldown = 1f;
    float regenTimer;
    // Start is called before the first frame update
    void Start()
    {
        currentMeter = maxMeter;
        fill.localScale = new Vector3(1f, fill.localScale.y, fill.localScale.z);
        trail.localScale = fill.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (regenTimer <= 0f && currentMeter < maxMeter) {
            currentMeter = Mathf.Min(currentMeter + regenPerSecond * Time.deltaTime, maxMeter);
            fill.localScale = new Vector3(currentMeter/maxMeter, fill.localScale.y, fill.localScale.z);
        }
        else if (regenTimer > 0) {
            regenTimer = Mathf.Max(regenTimer-Time.deltaTime, 0f);
        }
    }

    public void DepleteMeter(float amt, LeanTweenType ease=LeanTweenType.easeOutExpo) {
        regenTimer = regenCooldown;
        currentMeter = Mathf.Max(0f, currentMeter-amt);
        StartCoroutine(UpdateMeter(ease));
    }
    IEnumerator UpdateMeter(LeanTweenType ease=LeanTweenType.easeOutExpo) {
        trail.localScale = fill.localScale;
        if (ease != LeanTweenType.notUsed) {
            LeanTween.scaleX(fill.gameObject, currentMeter/maxMeter, 0.3f).setEase(ease);
            yield return new WaitForSeconds(0.3f);
            LeanTween.scaleX(trail.gameObject, fill.localScale.x, 0.3f).setEase(ease);
        }
        else {
            SetScaleX(fill, currentMeter/maxMeter);
            yield return new WaitForSeconds(0.3f);
            SetScaleX(trail, fill.localScale.x);
        }
    }

    public void SetScaleX(Transform t, float x) {
        t.localScale = new Vector3(x, t.localScale.y, t.localScale.z);
    }
}
