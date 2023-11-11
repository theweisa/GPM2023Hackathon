using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : BaseCombatant
{
    public GameObject tree;
    public int level = 1;
    public float energy;
    public float energyThreshold;

    public float exp = 0f;
    public float expThreshold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnergy(float newEnergy) {
        energy += newEnergy;
        UIManager.Instance.energyBar.UpdateUI(energy, energyThreshold);
        if (energy >= energyThreshold) {
            PlantTree();
            float rem = energy - energyThreshold;
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
            exp = 0;
            AddExp(rem);
        }
    }

    void PlantTree() {
        Instantiate(tree, transform.position, Quaternion.identity, InstantiationManager.Instance.transform);
    }

    void LevelUp() {
        level++;
        Debug.Log("level up!");
    }
}