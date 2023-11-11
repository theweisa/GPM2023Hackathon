using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameManager : UnitySingleton<GameManager>
{
    float waveTimer;
    float spawnTimer;
    [SerializeField] float spawnRadius;

    [SerializeField] GameObject myPrefab;

    // Start is called before the first frame update
    void Start()
    {
        waveTimer = 1f;
        spawnTimer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //every minute, create a wave
        waveTimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;
        
        //every wave has a spawn rate and an enemy quotient::: Level 1 spawns 50 enemies at 4 enemies per second
        if(waveTimer <= 0f)
        {
            print("next wave");
            //every spawn rate interval, spawn an enemy until we hit the quotient
            if(spawnTimer <= 0f)
            {
                print("new enemy");
                //Instantiate: enemyPrefab at randomCirclePos()
                Vector2 newPos = randomCirclePos(new Vector2(0,0), spawnRadius);
                Instantiate(myPrefab, new Vector3(newPos.x, newPos.y, 1), Quaternion.identity);
                spawnTimer = 1f;
            }
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