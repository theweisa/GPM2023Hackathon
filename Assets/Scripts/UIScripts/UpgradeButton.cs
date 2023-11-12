using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text upgradeName;
    public TMP_Text description;
    public TMP_Text level;
    public Image image;
    Upgrade currUpgrade;
    public void Init(Upgrade upgrade) {
        upgradeName.text = upgrade.id;
        description.text = upgrade.description;
        if (upgrade.image) image.sprite = upgrade.image;
        level.text = upgrade.level.ToString();
        currUpgrade = upgrade;
    }

    public void ChooseUpgrade() {
        Debug.Log($"{upgradeName.text} was chosen!");
        PlayerManager.Instance.combatant.AddUpgrade(currUpgrade);
        UIManager.Instance.CloseUpgradesScreen();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
