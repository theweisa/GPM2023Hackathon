using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Stat {
    public StatType type;
    public float value;
    [HideInInspector] public float baseValue;
    public List<StatModifier> modifiers = new List<StatModifier>();

    public Stat(StatType type) {
        this.type = type;
    }
    public Stat(StatType type, float val) {
        this.type = type;
        value = val;
        baseValue = val;
    }
    public void Init() {
        baseValue = value;
        CalculateStat();
    }

    public void Update() {
        UpdateModifiers();
    }

    public void SetValue(float newValue) {
        value = newValue;
    }

    public float CalculateStat() {
        // this fucking shit piece of code was the damn issue
        if (this.type == StatType.Hp) {
            this.baseValue = CalculateValue(this);
            return this.baseValue;
        }
        this.value = CalculateValue(this);
        //if (!canBeNegative) this.value = Mathf.Max(0f, this.value);
        return this.value;
    }

    #region stat modifier code
    public bool AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        CalculateStat();
        return true;
    }

    public StatModifier GetStatModifier(StatType type) {
        foreach (StatModifier mod in modifiers) {
            if (type == mod.modStat) return mod;
        }
        return null;
    }

    public StatModifier GetStatModifier(StatModifier _mod) {
        foreach (StatModifier mod in modifiers) {
            if (ReferenceEquals(mod, _mod)) return mod;
        }
        return null;
    }

    public bool AddSourceToAll(IStatMod source) {
        foreach(StatModifier mod in modifiers) {
            mod.source = source;
        }
        return true;
    }

    public bool RemoveModifierFromSource(IStatMod source)
    {
        bool didRemove = false;
 
        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            if (ReferenceEquals(modifiers[i].source, source))
            {
                didRemove = true;
                modifiers.RemoveAt(i);
            }
        }
        CalculateStat();
        return didRemove;
    }

    public bool RemoveModifierFromReference(StatModifier mod)
    {
        bool didRemove = false;
        
        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            if (modifiers[i] == mod)
            {
                didRemove = true;
                modifiers.RemoveAt(i);
            }
        }
        CalculateStat();
        return didRemove;
    }

    public void SetModValue(StatType type, float value)
    {
        foreach(StatModifier mod in modifiers)
        {
            if (mod.modStat == type)
            {
                mod.SetModValue(value);
            }
        }
    }
    public void SetModBaseValue(StatType type, float value)
    {
        foreach(StatModifier mod in modifiers)
        {
            if (mod.modStat == type)
            {
                mod.SetModBaseValue(value);
            }
        }
    }

    // return an additive base total for if you want the base after additive modifiers applied
    public float GetAdditiveBase(Stat stat) {
        float additiveBase = stat.baseValue;
        foreach(var mod in modifiers) {
            if (mod.modifierCalc != ModifierCalc.Additive || mod.modStat != stat.type) continue; 
            additiveBase += mod.modValue;
        }
        return additiveBase;
    }

    // this works under the assumption that mods in StatModifiers can be different stats
    public float CalculateValue(Stat stat)
    {
        // want to add first then multiply
        modifiers.Sort(comparison: (x, y) => x.modifierCalc.CompareTo(y.modifierCalc));
        float finalValue = stat.baseValue;
        float additiveBase = finalValue;

        // go through each modifier for a single stat
        foreach (var mod in modifiers)
        {
            if (mod.modStat != stat.type) continue; 
            switch (mod.modifierCalc)
            {
                // get additive base here and use for additive multiplier
                case ModifierCalc.Additive:
                    additiveBase += mod.modValue;
                    finalValue += mod.modValue;
                    break;
                case ModifierCalc.AdditiveMultipler:
                    finalValue += (additiveBase * mod.modValue);
                    break;
                case ModifierCalc.Multiplier:
                    finalValue *= mod.modValue;
                    break;
            }
        }

        return finalValue;
    }

    public void UpdateModifiers() {
        for (int i = modifiers.Count - 1; i >= 0; i--) {
            bool remove = modifiers[i].RemoveOnTimer();
            if (modifiers[i].modifierType == ModifierType.Decay) {
                CalculateStat();
            }
            if (remove) {
                RemoveModifierFromReference(modifiers[i]);
            }
        }
    }

    // check if modifier collection contains stat mod
    public bool ContainsModifier(StatModifier StatModifier) {
        foreach (StatModifier mod in modifiers) {
            if (StatModifier == mod) return true;
        }
        return false;
    }
    #endregion
}
