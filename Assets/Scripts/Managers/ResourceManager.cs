using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : UnitySingleton<ResourceManager>
{
    public List<GameObject> enemies = new List<GameObject>();
    // Start is called before the first frame update
    public override void Awake() {
        base.Awake();
        FetchResource("Enemies", enemies);
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
}
