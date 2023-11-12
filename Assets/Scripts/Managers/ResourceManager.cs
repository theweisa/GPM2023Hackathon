using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : UnitySingleton<ResourceManager>
{
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> text = new List<GameObject>();
    // Start is called before the first frame update
    public override void Awake() {
        base.Awake();
        FetchResource("Enemies", enemies);
        FetchResource("Text", text);
    }

    // Update is called once per frame
    void FetchResource(string objectType, List<GameObject> objectList) {
        objectList.Clear();
        foreach(GameObject obj in Resources.LoadAll(objectType, typeof(GameObject))) {
            objectList.Add(obj);
        }
    }
    public GameObject GetEnemyByName(string objName) {
        return enemies.FirstOrDefault(obj => obj.name == objName);
    }

    public GameObject GetRandomEnemy() {
        return enemies[Random.Range(0, enemies.Count)];
    }

    public GameObject GetRandomEnemyOfTier(int tier) {
        List<GameObject> ens = new List<GameObject>();
        foreach (GameObject e in enemies) {
            EnemyCombatant en = e.GetComponent<EnemyCombatant>();
            if (!en || en.tier != tier) continue;
            ens.Add(en.gameObject);
        }
        if (ens.Count > 0)
            return ens[Random.Range(0, ens.Count)];
        return null;
    }

    public GameObject GetTextByName(string objName) {
        return text.FirstOrDefault(obj => obj.name == objName);
    }
}
