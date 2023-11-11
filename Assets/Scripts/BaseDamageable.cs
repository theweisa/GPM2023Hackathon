using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseDamageable : MonoBehaviour
{
    public Rigidbody2D rb;
    public List<Stat> stats = new List<Stat>() {
        new Stat(StatType.Hp), new Stat(StatType.Atk), new Stat(StatType.Spd)
    };
    // Start is called before the first frame update
    virtual protected void Awake() {
        rb = rb ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        InitStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void InitStats() {
        foreach (Stat s in stats) {
            s.Init();
        }
    }

    #region stats

    public Stat GetStat(StatType sType) {
        return stats.FirstOrDefault(s => s.type == sType);
    }

    public float GetStatValue(StatType sType) {
        Stat stat = stats.FirstOrDefault(s => s.type == sType);
        if (stat != null) return stat.value;
        return -1f;
    }

    #endregion
}
