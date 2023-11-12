using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : BaseCombatant
{
    public GameObject tree;
    public AttackMeter meter;
    public PlayerController controller;
    public int level = 1;
    public float energy;
    public float energyThreshold;
    public float levelExpMultiplier = 1.2f;

    public float exp = 0f;
    public float expThreshold;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        meter = meter ? meter : Global.FindComponent<AttackMeter>(gameObject);
        controller = controller ? controller : Global.FindComponent<PlayerController>(gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        /*if (Input.GetKeyDown(KeyCode.L)) {
            LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            PlantTree();
        }*/
    }

    public override Stat ApplyDamage(float damageTaken) {
        Stat hp = base.ApplyDamage(damageTaken);
        // call damage sound here
        AudioManager.Instance.PlayAudioChild("hit", sounds);
        return hp;
    }

    public override void InitDamageText(float dmg)
    {
        DamageText dmgTxt = Instantiate(ResourceManager.Instance.GetTextByName("DamageText"), transform.position, Quaternion.identity, InstantiationManager.Instance.otherParent).GetComponent<DamageText>();
        dmgTxt.Init(dmg, DamageText.TextType.Player);
    }

    public override IEnumerator OnDeath()
    {
        yield return null;
        Debug.Log("game over");
    }

    public void AddEnergy(float newEnergy) {
        energy += newEnergy * GetStatValue(StatType.EnergyMult);
        UIManager.Instance.energyBar.UpdateUI(energy, energyThreshold);
        if (energy >= energyThreshold) {
            PlantTree();
            float rem = energy - energyThreshold;
            UIManager.Instance.energyBar.ResetUI();
            energy = 0;
            AddEnergy(rem);
        }
    }

    public void AddExp(float newExp) {
        exp += newExp;
        UIManager.Instance.expBar.UpdateUI(exp, expThreshold);
        if (exp >= expThreshold) {
            float rem = exp - expThreshold;
            exp = 0;
            UIManager.Instance.expBar.ResetUI();
            LevelUp();
            AddExp(rem);
        }
    }

    void PlantTree() {
        Instantiate(tree, transform.position, Quaternion.identity, InstantiationManager.Instance.treeParent);
    }

    void LevelUp() {
        level++;
        expThreshold *= levelExpMultiplier;
        Debug.Log("level up!");
        UIManager.Instance.ShowUpgradesScreen();
    }
}
