using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyPackage
{
    public GameObject enemyPrefab;
    public int spawnAmount;
    public float weight;
}

public class WaveManager : MonoBehaviour
{

    public int MinEnemyCount = 3;
    public int MaxEnemyCount = 6;

    private int EnemiesActive = 0;
    private Vector2 spawnPos;

    public List<EnemyPackage> enemyList = new List<EnemyPackage>();
    public Transform spawnArea;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckEnemyCount());
    }

    void SelectEnemy()
    {
        float totalWeight = 0;

        foreach (EnemyPackage enemy in enemyList)
        {
            totalWeight += enemy.weight;
        }

        float randomWeight = Random.Range(0, totalWeight);

        float iterativeWeight = 0f;
        foreach (var enemy in enemyList)
        {
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

        EnemiesActive = enemies.Length;

        yield return new WaitForSeconds(2f);
        StartCoroutine(CheckEnemyCount());
    }

}
