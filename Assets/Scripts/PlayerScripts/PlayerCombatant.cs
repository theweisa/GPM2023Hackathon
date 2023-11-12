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

    public float exp = 0f;
    public float expThreshold;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        meter = meter ? meter : Global.FindComponent<AttackMeter>(gameObject);
        controller = controller ? controller : Global.FindComponent<PlayerController>(gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public void AddEnergy(float newEnergy) {
        energy += newEnergy;
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
            LevelUp();
            float rem = exp - expThreshold;
            UIManager.Instance.expBar.ResetUI();
            exp = 0;
            AddExp(rem);
        }
    }

    void PlantTree() {
        Instantiate(tree, transform.position, Quaternion.identity, InstantiationManager.Instance.treeParent);
    }

    void LevelUp() {
        level++;
        Debug.Log("level up!");
    }
}
