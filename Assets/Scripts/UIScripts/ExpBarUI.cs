using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpBarUI : MonoBehaviour
{
    public RectTransform bar;
    public TMP_Text expText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI(PlayerManager.Instance.combatant.exp, PlayerManager.Instance.combatant.expThreshold);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(float value, float maxValue) {
        float ratio = Mathf.Min(1f, value/maxValue);
        expText.text = $"EXP: {(ratio*100f).ToString("F1")}%";
        LeanTween.scaleX(bar.gameObject, ratio, 0.7f).setEaseOutExpo();
    }
}
