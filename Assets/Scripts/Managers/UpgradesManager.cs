using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : UnitySingleton<UpgradesManager>
{
    public List<Upgrade> upgrades = new List<Upgrade>();
    public Transform upgradeButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // choose three random upgrades to throw into upgradeButtons
    public void ChooseUpgrades() {
        List<Upgrade> tmpUpgrades = new List<Upgrade>(upgrades);
        int totalWeight = 0;
        for (int i = tmpUpgrades.Count-1; i >= 0; i--) {
            Upgrade upgrade = tmpUpgrades[i];
            Upgrade playerUpgrade = PlayerManager.Instance.combatant.ContainsUpgrade(upgrade);
            if (playerUpgrade && playerUpgrade.level >= playerUpgrade.maxLevel) {
                tmpUpgrades.Remove(upgrade);
                continue;
            }
            totalWeight += upgrade.spawnWeighting;
        }
        foreach (Transform btn in upgradeButtons) {
            UpgradeButton uBtn = btn.GetComponent<UpgradeButton>();
            int currentWeight = 0;
            int randomWeight = Random.Range(0, totalWeight+1);
            for (int i = tmpUpgrades.Count-1; i >= 0; i--) {
                Upgrade upgrade = tmpUpgrades[i];
                currentWeight += upgrade.spawnWeighting;
                if (randomWeight <= currentWeight) {
                    Upgrade playerUpgrade = PlayerManager.Instance.combatant.ContainsUpgrade(upgrade);
                    if (playerUpgrade) {
                        uBtn.Init(playerUpgrade, true);
                    }
                    else {
                        uBtn.Init(upgrade);
                    }
                    tmpUpgrades.RemoveAt(i);
                    totalWeight -= upgrade.spawnWeighting;
                    break;
                }
            }
        }
    }
}
