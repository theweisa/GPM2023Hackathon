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
        energyText.text = $"Energy: {(ratio*100f).ToString("F1")}%";
        LeanTween.scaleX(bar.gameObject, ratio, 0.7f).setEaseOutExpo();
    }
}
