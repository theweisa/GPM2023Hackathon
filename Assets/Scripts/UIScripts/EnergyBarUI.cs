using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyBarUI : MonoBehaviour
{
    public RectTransform bar;
    public TMP_Text energyText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI(PlayerManager.Instance.combatant.energy, PlayerManager.Instance.combatant.energyThreshold);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(float value, float maxValue) {
        float ratio = Mathf.Min(1f, value/maxValue);
        energyText.text = $"Plant Tree: {(ratio*100f).ToString("F1")}%";
        LeanTween.scaleX(bar.gameObject, ratio, 0.7f).setEaseOutExpo();
    }

    public void ResetUI() {
        energyText.text = $"Plant Tree: 0.0%";
        bar.localScale = new Vector3(0f, bar.localScale.y, bar.localScale.z);
    }
}
