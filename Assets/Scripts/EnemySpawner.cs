using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public float[] TimeBetweenWaves = new float[2] { 2f, 3f };

    public float timeBeforeFirstWave = 2.25f;

    public GameObject lNormalEnemy;
    public GameObject lFastEnemy;
    public GameObject lSlowEnemy;

    public GameObject hNormalEnemy;
    public GameObject hFastEnemy;
    public GameObject hSlowEnemy;

    public GameObject laserEnemy;

    public Spawnpoints topSpawnpoints;
    public Spawnpoints bottomSpawnpoints;

    public GameManager gameManager = null;
    public Serializer serializer;
    
    private bool gameLost = false;

    [HideInInspector]
    public float totalTimer = 0f;

    void Start()
    {
        StartCoroutine(SpawnDelay());
        SpawnNormalWave();
    }

    private void Update()
    {
        totalTimer += Time.deltaTime;

        if (gameManager != null)
            gameLost = gameManager.gameLost;

    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSecondsRealtime(timeBeforeFirstWave);
        while (true)
        {
            if (!gameLost)
            {
                int r = Random.Range(1, 101);

                if (r < 80)
                { StartCoroutine(SpawnNormalWave()); }
                else if (r < 101)
                {
                    int lasersSpawned = SpawnLaser();
                    yield return new WaitForSeconds(lasersSpawned / 4f);
                }
            }
            yield return new WaitForSeconds(Random.Range(TimeBetweenWaves[0], TimeBetweenWaves[1]));
        }
    }

    public int SpawnLaser()
    {
        int lasers = 0;
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            Transform spawnPoint = ChooseSpawnPoint();
            GameObject laser = Instantiate(laserEnemy, spawnPoint.position - Vector3.up * spawnPoint.position.y, spawnPoint.rotation);
            Destroy(laser, 8f);
            lasers = i;
        }
        return lasers;
    }


    IEnumerator SpawnNormalWave()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject enemyToSpawn = EnemyToSpawn();
            if (enemyToSpawn != null)
            {
                Transform spawnPoint = ChooseSpawnPoint();
                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSeconds(Random.Range(0f, .1f));
            }
        }
    }

    private Transform ChooseSpawnPoint()
    {
        if (Random.Range(0, 2) == 0)
        {
            return topSpawnpoints.points[Random.Range(0, topSpawnpoints.points.Length)];
        }
        else
        {
            return bottomSpawnpoints.points[Random.Range(0, bottomSpawnpoints.points.Length)];
        }
    }

    private GameObject EnemyToSpawn()
    {
        int r = Random.Range(1, 101);
        
        if (r > 95 - Mathf.Clamp(totalTimer / 5, 0, 20))
        {
            if (serializer.data.Graphics)
                return hSlowEnemy;
            else
                return lSlowEnemy;
        }
        else if (r > 85 - Mathf.Clamp(totalTimer / 5, 0, 17))
        {
            if (serializer.data.Graphics)
                return hFastEnemy;
            else
                return lFastEnemy;
        }
        else if (r > 50 - Mathf.Clamp(totalTimer / 5, 0, 15))
        {
            if (serializer.data.Graphics)
                return hNormalEnemy;
            else
                return lNormalEnemy;
        }

        return null;
        
    }
}
