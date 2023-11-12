using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : UnitySingleton<UIManager>
{
    public ExpBarUI expBar;
    public EnergyBarUI energyBar;
    public GameObject upgradesScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUpgradesScreen() {
        Time.timeScale = 0f;
        upgradesScreen.SetActive(true);
        UpgradesManager.Instance.ChooseUpgrades();
    }

    public void CloseUpgradesScreen() {
        Time.timeScale = 1f;
        upgradesScreen.SetActive(false);
    }

}
