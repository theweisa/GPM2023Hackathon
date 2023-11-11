using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Stat {
    public StatType type;
    public float value;
    [HideInInspector] public float baseValue;

    public Stat(StatType type) {
        this.type = type;
    }
    public void Init() {
        baseValue = value;
    }
}
