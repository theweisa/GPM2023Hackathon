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

    Vector3 baseScale;
    public void Init(Upgrade upgrade, bool nextLevel=false) {
        upgradeName.text = upgrade.id;
        description.text = upgrade.description;
        image.sprite = upgrade.image;
        if (!nextLevel) {
            level.text = $"Level: {upgrade.level}";
        }
        else {
            level.text = $"Level: {upgrade.level+1}";
            if (upgrade.level+1 >= upgrade.maxLevel) {
                level.text = $"Level: MAX";
            }
        }
        
        currUpgrade = upgrade;
        transform.localScale = baseScale;
    }

    public void ChooseUpgrade() {
        Debug.Log($"{upgradeName.text} was chosen!");
        PlayerManager.Instance.combatant.AddUpgrade(currUpgrade);
        UIManager.Instance.CloseUpgradesScreen();
    }

    public void OnHover() {
        Debug.Log("hover");
        LeanTween.scale(gameObject, baseScale*1.13f, 0.2f).setIgnoreTimeScale(true);
    }
    public void OnExit() {
        LeanTween.scale(gameObject, baseScale, 0.2f).setIgnoreTimeScale(true);
    }
    // Start is called before the first frame update
    void Awake()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
