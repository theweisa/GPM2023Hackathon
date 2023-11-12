using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : UnitySingleton<UIManager>
{
    public ExpBarUI expBar;
    public EnergyBarUI energyBar;
    public GameObject upgradesScreen;
    public TMP_Text timerText;
    public GameObject menuScreen;
    public TMP_Text menuText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timer = GameManager.Instance.totalTimer;
        int mins = (int)(timer / 60f);
        float s = timer % 60;
        if (s < 10f) {
            timerText.text = $"{mins}:0{s.ToString("F0")}";
        }
        else {
            timerText.text = $"{mins}:{s.ToString("F0")}";
        }
    }

    public void ShowUpgradesScreen() {
        AudioManager.Instance.PlaySound("UILevelUp");
        Time.timeScale = 0f;
        upgradesScreen.SetActive(true);
        UpgradesManager.Instance.ChooseUpgrades();
    }

    public void CloseUpgradesScreen() {
        Time.timeScale = 1f;
        upgradesScreen.SetActive(false);
    }

}
