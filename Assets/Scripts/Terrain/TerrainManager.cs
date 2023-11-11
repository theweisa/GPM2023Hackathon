using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// There's definitely a better way of coding this but I don't care, it works
public class TerrainManager : MonoBehaviour{

    public GameObject player;
    public int chunkSize = 20;
    public float chunkUpdateInterval = 1f;
    public int renderDistance = 2; // in each direction
    public List<GameObject> validChunks;

    [HideInInspector] public List<GameObject> spawnedChunks;
    [HideInInspector] public List<Vector2> invalidSpawns; // Coords for which locations already spawned a chunk
    [HideInInspector] public Vector3 _initialPos = new Vector3(0,0,0);
    [HideInInspector] public Vector2 currentGridPos = new Vector2(0,0);

    void Start(){
        if (player){ 
            _initialPos = new Vector3(Mathf.Round(player.transform.position.x), Mathf.Round(player.transform.position.y), 0);
        }
        InvokeRepeating("OptimizedChunkLoad", 0f, chunkUpdateInterval);
    }

    private void OptimizedChunkLoad(){
        GenerateChunks();
        DisableChunks();
    }

    private void GenerateChunks(){
        currentGridPos = PosToGrid(player.transform.position);
        for (float x=currentGridPos.x-renderDistance; x<currentGridPos.x+renderDistance+1; x++){
            for (float y=currentGridPos.y-renderDistance; y<currentGridPos.y+renderDistance+1; y++){
                Vector2 gridPos= new Vector2(x, y);
                // Check if there's no spawned chunk in a given position. If true
                if (!invalidSpawns.Contains(gridPos)){
                    GameObject chunk = validChunks[Random.Range(0, validChunks.Count)];
                    // Convert gridPos to world pos and instantiate chunk at that position
                    GameObject spawnedChunk = Instantiate(chunk, GridToPos(gridPos), Quaternion.identity);
                    spawnedChunk.transform.parent = this.transform;
                    // Add it to spawned chunks list
                    spawnedChunks.Add(spawnedChunk);
                    invalidSpawns.Add(gridPos);
                } 
            }
        }
    }

    private void DisableChunks(){
        foreach (GameObject chunk in spawnedChunks){
            float distance = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (distance > renderDistance*chunkSize*2){
                chunk.SetActive(false);
            } else {
                chunk.SetActive(true);
            }
        }
    }

    private Vector2 PosToGrid(Vector3 position){
        return new Vector2(Mathf.Round((position.x - _initialPos.x)/chunkSize), Mathf.Round((position.y - _initialPos.y)/chunkSize));
    }
    private Vector3 GridToPos(Vector2 position){
        return new Vector3((position.x * chunkSize)+_initialPos.x, (position.y * chunkSize)+_initialPos.y, 0);
    }
}
