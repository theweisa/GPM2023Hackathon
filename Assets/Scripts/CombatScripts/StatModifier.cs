using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum ModifierCalc{ Additive, AdditiveMultipler, Multiplier }

public enum ModifierType { Passive, Timer, Decay }

[System.Serializable]
public class StatModifier : IStatMod
{
    public float modValue;
    [HideInInspector] public float baseModValue;
    public StatType modStat;
    public ModifierCalc modifierCalc;
    public ModifierType modifierType;
    [SerializeField] public IStatMod source;
    [ShowIf("ShowTimer")] public float modDuration;
    [HideInInspector] public float baseModTimer;
    public bool ShowTimer() { 
        if (modifierType == ModifierType.Passive) {
            return false;
        } 
        return true; 
    }

    public StatModifier(float value, StatType statType, ModifierCalc modifierCalc, ModifierType modType, StatModifier source=null, float duration=0)
    {
        this.baseModValue = value;
        this.modValue = value;
        this.modStat = statType;
        this.modifierCalc = modifierCalc;
        this.modifierType = modType;
        this.source = source;
        this.modDuration = duration;
        this.baseModTimer = duration;
    }
    public StatModifier() {
        source = this;
        this.baseModValue = this.modValue;
        this.baseModTimer = modDuration;
    }

    public bool RemoveOnTimer() {
        bool removed = false;
        switch(this.modifierType) {
            case ModifierType.Timer:
            {
                modDuration -= Time.deltaTime;
                if (modDuration <= 0) {
                    removed = true;
                    // remove self from the modifier collection
                }
                break;
            }
            case ModifierType.Decay:
            {
                this.baseModTimer = this.baseModTimer <= 0 ? this.modDuration : this.baseModTimer;
                this.baseModValue = this.baseModValue == 0 ? this.modValue : this.baseModValue;
                modDuration -= Time.deltaTime;
                //Debug.Log(baseModValue+": "+modDuration+"/"+baseModTimer+"="+(modDuration / baseModTimer));
                modValue = baseModValue * (modDuration / baseModTimer);
                if (modDuration <= 0) {
                    removed = true;
                    // remove self from the modifier collection
                }
                break;
            }
        }
        return removed;
    }

    public void SetSource(StatModifier source) {
        this.source = source;
    }

    public void ResetTimer() {
        this.modDuration = this.baseModTimer;
        Debug.Log("reset duration");
    }

    public void SetModValue(float value) {
        modValue = value;
    }
    public void SetModBaseValue(float value) {
        baseModValue = value;
    }
    public void SetMod(float value) {
        modValue = value;
        baseModValue = value;
    }
    public float GetModValue() {
        return modValue;
    }
    public float GetBaseModValue() {
        return baseModValue;
    }
    public float GetBaseModTimer() {
        return baseModTimer;
    }
}

// lmfao
public interface IStatMod
{
    
}
