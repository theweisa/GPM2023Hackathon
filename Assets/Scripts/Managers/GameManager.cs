using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.SceneManagement;

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
    public WaveEvent curEvent;
    public int curEventNum;

    public float totalTimer = 300f;
    [SerializeField] float waveInterval; //user sets the length of time between each wave

    [SerializeField] int curSpawnNum;

    [SerializeField] float waveTimer; //public variables so you can see if the timer is working correctly
    [SerializeField] float spawnTimer;
    [SerializeField] float spawnRadius; //size of the circle that spawns enemies
    public bool startGame = false;
    public bool winGame = false;

    public void StartGame() {
        if (Input.GetMouseButtonDown(0)) {
            Time.timeScale = 1f;
            startGame = true;
            UIManager.Instance.menuScreen.gameObject.SetActive(false);
            UIManager.Instance.expBar.gameObject.SetActive(true);
            UIManager.Instance.energyBar.gameObject.SetActive(true);
        }
        
    }
    public void WinGame() {
        Time.timeScale = 0;
        winGame = true;
        UIManager.Instance.menuScreen.gameObject.SetActive(true);
        UIManager.Instance.menuText.text = "Area Purified!\nClick to Restart";
    }
    public void LostGame() {
        Time.timeScale = 0;
        winGame = true;
        UIManager.Instance.menuScreen.gameObject.SetActive(true);
        UIManager.Instance.menuText.text = "Greenhouse Gasses Win...\nClick to Restart";
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.menuText.text = "Click To Start";
        UIManager.Instance.menuScreen.gameObject.SetActive(true);
        UIManager.Instance.menuText.gameObject.SetActive(true);
        UIManager.Instance.expBar.gameObject.SetActive(false);
        UIManager.Instance.energyBar.gameObject.SetActive(false);
        //both timers should activate on game start
        waveTimer = 0;
        spawnTimer = 0;
        curSpawnNum = 0;
        curEvent = EventQueue[0];
        curEventNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startGame) {
            StartGame();
            return;
        }
        else if (winGame) {
            if (Input.GetMouseButtonDown(0)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
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
                Vector2 newPos = randomCirclePos(PlayerManager.Instance.transform.position, spawnRadius);

                //GameObject enemy = ResourceManager.Instance.GetEnemyByName("ChaserEnemy");
                GameObject enemy = selectEnemy();
                if (!enemy) return;
                Instantiate(enemy, newPos, Quaternion.identity, InstantiationManager.Instance.enemyParent);
                curSpawnNum += 1;
                
                //reset spawn timer
                spawnTimer = curEvent.SpawnRate_SecPerEnemy;
            }
            //if we haven't hit the quota, keep spawning
            if(curSpawnNum != curEvent.SpawnQuota)
            {
                spawnTimer -= Time.deltaTime;
            }
            else //we finished the wave, so we move on to the next
            {
                curSpawnNum = 0;
                waveTimer = waveInterval; //reset the wave timer
                nextWave();
                print("NEXT WAVE"); 
            }
        }
    }

    GameObject selectEnemy() //select Enemy from list of possible enemies from CurEvent
    {
        int num = Random.Range(0, curEvent.SelectedEnemies.Length);
        string name = curEvent.SelectedEnemies[num].name;
        print("Name of enemy spawned: " + name);
        return ResourceManager.Instance.GetEnemyByName(name);
        
    }

    void nextWave()//move to next WaveEvent in the queue
    {
        //if there's another event, move on
        if((curEventNum + 1) <= EventQueue.Length)
        {
            curEventNum += 1;
            curEvent = EventQueue[curEventNum];
        }else{ //if you reach the end, then restart at the beginning ---------- Probably want to change this eventually to insert safety default events or something
            curEventNum = 0;
            curEvent = EventQueue[curEventNum]; 
        }
    }

    void UpdateTimer() {
        totalTimer -= Time.deltaTime;
        if (totalTimer <= 0) {
            WinGame();
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