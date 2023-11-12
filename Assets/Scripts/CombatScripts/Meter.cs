using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour
{
    public Transform fill;
    public Transform trail;

    public float maxMeter = 100f;
    public float currentMeter;
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
    public virtual void SetMeter(float newAmt, float maxAmt, LeanTweenType ease=LeanTweenType.easeOutExpo) {
        maxMeter = maxAmt;
        currentMeter = Mathf.Clamp(newAmt, 0f, maxAmt);
        StartCoroutine(UpdateMeter(ease));
    }
    public virtual void AddMeter(float amt, LeanTweenType ease=LeanTweenType.easeOutExpo) {
        SetMeter(currentMeter+amt, maxMeter, ease);
    }
    public virtual void DepleteMeter(float amt, LeanTweenType ease=LeanTweenType.easeOutExpo) {
        AddMeter(-amt, ease);
    }
    protected virtual IEnumerator UpdateMeter(LeanTweenType ease=LeanTweenType.easeOutExpo) {
        if (ease != LeanTweenType.notUsed) {
            LeanTween.scaleX(fill.gameObject, currentMeter/maxMeter, 0.3f).setEase(ease);
            yield return new WaitForSeconds(0.5f);
            LeanTween.scaleX(trail.gameObject, fill.localScale.x, 0.3f).setEase(ease);
        }
        else {
            SetScaleX(fill, currentMeter/maxMeter);
            yield return new WaitForSeconds(0.5f);
            SetScaleX(trail, fill.localScale.x);
        }
    }

    public void SetScaleX(Transform t, float x) {
        t.localScale = new Vector3(x, t.localScale.y, t.localScale.z);
    }
}
