using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class EnemyPackage
{
    public GameObject enemyPrefab;
    public int spawnAmount;
    public float weight;
}

public class WaveManager : MonoBehaviour
{

    public float timeBetweenSpawns = 1.0f;
    private float spawnTimer = 0f;
    public bool spawning = true;

    public int totalSpawnsInStage = 10;

    public int MinEnemyCount = 3;
    public int MaxEnemyCount = 6;

    private int enemiesActive = 0;
    private Vector2 spawnPos;

    public List<EnemyPackage> enemyPool = new List<EnemyPackage>();
    public Transform playerTransform;
         
    public float minSpawnDistance = 10f; 
    public float maxSpawnDistance = 30f;
    public LayerMask terrainLayer; 

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = new Vector2(0,0);

        gameManager = GetComponent<GameManager>();

        playerTransform = FindObjectOfType<Player>().transform;

        CheckEnemyCount();
        SpawnRandomEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckEnemyCount());
        HandleSpawning();
    }

    void SpawnRandomEnemy()
    {
        Debug.Log("Spawn ENemy");

        float totalWeight = 0;

        Debug.Log("Total weight is: " + totalWeight);

        foreach (EnemyPackage enemy in enemyPool)
        {
            totalWeight += enemy.weight;
        }

        float randomWeight = Random.Range(0, totalWeight);

        float iterativeWeight = 0f;
        foreach (var enemy in enemyPool)
        {
            Debug.Log("Enemy check");

            iterativeWeight += enemy.weight;
            if (randomWeight <= iterativeWeight)
            {
                SpawnEnemy(enemy);
                {
                    break;
                }
            }
        }
    }

    void SpawnEnemy (EnemyPackage enemy)
    {

        Vector2 spawnPos = GetValidSpawnPos();

        Debug.Log("Spawned:", enemy.enemyPrefab);

        for(int i = 0; i < enemy.spawnAmount; i++)
        {
            float x_offset = Random.Range(-1,1);
            float y_offset = Random.Range(-1,1);

            Vector2 IndividualPos = new Vector2(spawnPos.x + x_offset, spawnPos.y +y_offset);

            Instantiate(enemy.enemyPrefab,IndividualPos,Quaternion.identity);
        }

    }

    private IEnumerator CheckEnemyCount()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        enemiesActive = enemies.Length;
        yield return new WaitForSeconds(2f);

        if(enemiesActive < 1 && totalSpawnsInStage < 1)
        {
            gameManager.Win();
        }

        StartCoroutine(CheckEnemyCount());
    }

    Vector3 GetValidSpawnPos()
    {
        int iterations = 0;

        while(iterations < 100)
        {
            iterations++;

            Vector2 direction =  Random.insideUnitCircle.normalized;
            float distance = Random.Range(minSpawnDistance,maxSpawnDistance);

            Vector2 spawnPosition = (Vector2)playerTransform.position + (direction * distance);

            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero, 0f, terrainLayer);
            if (!hit.collider)
            {
                return spawnPosition;
            }
            else
            {
                Debug.Log("Hit Terrain");
            }
        }

        return playerTransform.position;

    }

    void HandleSpawning()
    {
        if(totalSpawnsInStage >= 1)
        {
            
            if(spawning)
            {
                spawnTimer -= Time.deltaTime;

                if(spawnTimer <= 0f)
                {
                    SpawnRandomEnemy();
                    spawnTimer = timeBetweenSpawns;
                    totalSpawnsInStage--;
                }
            }

            if(enemiesActive > MaxEnemyCount)
            {
                spawning = false;
            }
            else if (enemiesActive < MinEnemyCount)
            {
                spawning = true;
            }

        }
    }

}
