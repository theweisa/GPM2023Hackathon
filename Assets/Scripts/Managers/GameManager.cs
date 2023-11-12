using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class WaveEvent
{
    [SerializeField] public float SpawnRate_SecPerEnemy;
    [SerializeField] public float SpawnQuota; 
    [SerializeField] public EnemyCombatant[] SelectedEnemies; 
}

public class GameManager : UnitySingleton<GameManager>
{
    [SerializeField] public WaveEvent[] EventQueue; //the queue of events that the game manager will cycle through 

    public float totalTimer = 300f;
    [SerializeField] float waveInterval; //user sets the values of how long it takes to trigger the wave & spawn rate
    [SerializeField] float spawnInterval;

    [SerializeField] int spawnCount; //user sets how many enemies should get spawned during a wave
    [SerializeField] int curSpawnNum;

    [SerializeField] float waveTimer; //public variables so you can see if the timer is working correctly
    [SerializeField] float spawnTimer;
    [SerializeField] float spawnRadius; //size of the circle that spawns enemies

    // Start is called before the first frame update
    void Start()
    {
        //both timers should activate on game start
        waveTimer = 0;
        spawnTimer = 0;
        curSpawnNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        //every minute, create a wave
        waveTimer -= Time.deltaTime;
               
        //every wave has a spawn rate and an enemy quotient::: Level 1 spawns 50 enemies at 4 enemies per second
        if(waveTimer <= 0f)
        {
            //every spawn rate interval, spawn an enemy until we hit the quotient
            if(spawnTimer <= 0f)
            {
                print("NEW ENEMY");
                //spawn enemy at random location on a circle defined by user-given radius
                Vector2 newPos = randomCirclePos(new Vector2(0,0), spawnRadius);
                GameObject enemy = ResourceManager.Instance.GetEnemyByName("ChaserEnemy");
                if (!enemy) return;
                Instantiate(enemy, newPos, Quaternion.identity, InstantiationManager.Instance.enemyParent);
                spawnTimer = spawnInterval;

                curSpawnNum += 1;
                //reset spawn timer
                spawnTimer = spawnInterval;
            }
            //check if we've hit our quotient
            if(curSpawnNum != spawnCount)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                curSpawnNum = 0;
                waveTimer = waveInterval; //reset the wave timer
                print("NEXT WAVE"); 
            }
        }
    }

    void UpdateTimer() {
        totalTimer -= Time.deltaTime;
        if (totalTimer <= 0) {
            Debug.Log("You Win!");
        }
    }

    Vector2 randomCirclePos(Vector2 pos, float radius)
    {
        int ang = Random.Range(0, 360);
        Vector2 newPos;
        newPos.x = pos.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        newPos.y = pos.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        return newPos;
    }
}