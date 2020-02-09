using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public float[] TimeBetweenWaves = new float[2] { 2f, 3f };

    public float timeBeforeFirstWave = 2.25f;
    public float defaultLaserDistance = 4.2f;

    public GameObject lNormalEnemy;
    public GameObject lFastEnemy;
    public GameObject lSlowEnemy;

    public GameObject hNormalEnemy;
    public GameObject hFastEnemy;
    public GameObject hSlowEnemy;

    public GameObject LaserEnemy;

    public Spawnpoints topSpawnpoints;
    public Spawnpoints bottomSpawnpoints;

    public GameManager gameManager = null;
    public Serializer serializer;
    
    private bool gameLost = false;
    private int lanes = 5;

    [HideInInspector]
    public float totalTimer = 0f;

    void Start()
    {
        Camera.main.orthographicSize = 2.5f / Camera.main.aspect;
        StartCoroutine(SpawnDelay());
        SpawnNormalWave();
    }

    private void Update()
    {
        totalTimer += Time.deltaTime;

        if (gameManager != null)
        {
            lanes = gameManager.lanes;
            gameLost = gameManager.gameLost;
        }

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
        for (int i = 0; i < Random.Range(1, lanes - ((lanes-1)/2)); i++)
        {
            Vector3 spawnpoint = ChooseSpawnPoint();
            GameObject laser = Instantiate(LaserEnemy, spawnpoint - Vector3.up * spawnpoint.y, Quaternion.identity);
            Destroy(laser, 4.1f);
            lasers = i;
        }
        return lasers;
    }


    IEnumerator SpawnNormalWave()
    {
        for(int i = 0; i < lanes - 1; i++)
        {
            GameObject enemyToSpawn = EnemyToSpawn();
            if (enemyToSpawn != null)
            {
                Instantiate(enemyToSpawn, ChooseSpawnPoint(), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(0f, .1f));
            }
        }
    }

    private Vector3 ChooseSpawnPoint()
    {
        if (Random.Range(0, 2) == 0)
        {
            return new Vector3(Random.Range(-(lanes - 1) / 2, (lanes + 1) / 2), (lanes + 1) / 2 / Camera.main.aspect);
        }
        else
        {
            return new Vector3(Random.Range(-(lanes - 1) / 2, (lanes + 1) / 2), - (lanes + 1) / 2 / Camera.main.aspect);
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
