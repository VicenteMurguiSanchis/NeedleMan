using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int currentWave = 0;

    [SerializeField] int[] waveToAppearEnemy = new int[3];
    [SerializeField] float[] timeToSpawnEnemy = new float[3];

    float currentTimeToSpawnEnemy;

    [SerializeField] GameObject[] enemies = new GameObject[3];

    float spawnerTimer;

    [SerializeField] Text waveText;

    private void Awake()
    {
        currentTimeToSpawnEnemy = timeToSpawnEnemy[0];
        currentWave = 1;
    }

    private void Update()
    {
        spawnerTimer -= Time.deltaTime;

        waveText.text = "Wave : " + (currentWave - 1).ToString();

        Debug.Log(GameManager.enemiesInScene);
        if(GameManager.enemiesInScene == 0 && spawnerTimer < 0)
        {
            DependOnTheWave();
            currentWave++;
            spawnerTimer = 1.0f;
        }
    }

    void DependOnTheWave()
    {
        if(currentWave <= waveToAppearEnemy[0])
        {
            currentTimeToSpawnEnemy = timeToSpawnEnemy[0];
            SpawnFirstEnemy(HowMany(0));
        }
        else if (currentWave <= waveToAppearEnemy[1])
        {
            currentTimeToSpawnEnemy = timeToSpawnEnemy[0];
            SpawnFirstEnemy(HowMany(0));
            currentTimeToSpawnEnemy = timeToSpawnEnemy[1];
            SpawnSecondEnemy(HowMany(1));
        }
        else if(currentWave > waveToAppearEnemy[1])
        {
            currentTimeToSpawnEnemy = timeToSpawnEnemy[0];
            SpawnFirstEnemy(HowMany(0));
            currentTimeToSpawnEnemy = timeToSpawnEnemy[1];
            SpawnSecondEnemy(HowMany(1));
            currentTimeToSpawnEnemy = timeToSpawnEnemy[2];
            SpawnThirdEnemy(HowMany(2));
        }
    }

    int HowMany(int type)
    {
        if(currentWave <= waveToAppearEnemy[0])
        {
            return currentWave;
        }else if (currentWave <= waveToAppearEnemy[1])
        {
            if (type == 0)
                return currentWave / 2 + 1;
            else
                return currentWave / 2;
        }
        else if (currentWave > waveToAppearEnemy[1])
        {
            if (type == 0)
                return currentWave / 2 + 1;
            else if (type == 1)
                return currentWave / 3;
            else
                return (currentWave - 1) / 4;
        }
        else
        {
            return 0;
        }
    }

    void SpawnFirstEnemy(int howMany)
    {
        float timeToWait = currentTimeToSpawnEnemy;

        for (int i = 0; i < howMany; i++)
        {
            StartCoroutine(SpawnFirstEnemyCo(timeToWait * i));
        }
    }
    IEnumerator SpawnFirstEnemyCo(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Vector3 pos = new Vector3(Random.Range(-80, 80) / 10, 12, 10);
        Instantiate(enemies[0], pos, transform.rotation);
    }

    void SpawnSecondEnemy(int howMany)
    {
        float timeToWait = currentTimeToSpawnEnemy;

        for (int i = 0; i < howMany; i++)
        {
            StartCoroutine(SpawnSecondEnemyCo(timeToWait * i));
        }
    }
    IEnumerator SpawnSecondEnemyCo(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        int pos = Random.Range(0, 3);

        switch (pos)
        {
            case 0:
                Instantiate(enemies[1], new Vector3(-6.2f,12,10), transform.rotation);
                break;
            case 1:
                Instantiate(enemies[1], new Vector3(0f, 12,10), transform.rotation);
                break;
            case 2:
                Instantiate(enemies[1], new Vector3(6.2f, 12,10), transform.rotation);
                break;
        }
        
    }

    void SpawnThirdEnemy(int howMany)
    {
        float timeToWait = currentTimeToSpawnEnemy;

        for (int i = 0; i < howMany; i++)
        {
            StartCoroutine(SpawnThirdEnemyCo(timeToWait * i));
        }
    }
    IEnumerator SpawnThirdEnemyCo(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Vector3 pos = new Vector3(Random.Range(-80, 80) / 10, 12, 10);
        Instantiate(enemies[2], pos, transform.rotation);
    }
}
