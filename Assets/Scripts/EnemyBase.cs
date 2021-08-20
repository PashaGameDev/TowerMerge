using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] GameObject [] enemyPrefab;
    [SerializeField] Transform enemySpawnPoint;
    [SerializeField] float timeRate = 1f;

    [SerializeField] private Text countDownText;
    [SerializeField] private float timerCountDown = 3f;



    public static event Action<GameObject> CreatedEnemy;

    private float t = 0f;
    private int spawnedEnemiesInWave = 0;
   


    int waveNum = 1;
    int spawnedEnemiesTotal = 0;
    int type1inWave = 0;
    int type2inWave = 0;
    int type3inWave = 0;
    
    
    void Update()
    {
        if (timerCountDown > 0)
        {
            timerCountDown -= Time.deltaTime;
            countDownText.text = Mathf.RoundToInt(timerCountDown).ToString();
            if (timerCountDown < 1) { countDownText.text = "GO!"; }
            return;
        }
        else
        {
            countDownText.gameObject.SetActive(false);
        }

        if (GameManager.instance.AllEnemies.Count > 5) { return; }
        if (t <= 0)
        {
            SpawnWaves();
            // CreatEnemy();
            t = timeRate;
        }
        else
        {
            t -= Time.deltaTime;
        }
    }
    void CreatEnemy(int typeEnemy)
    {
        //if (GameManager.instance.AllEnemies.Count >= 2) { return; }
        // GameObject enemy = Instantiate(SetEnemyToBuild(), enemySpawnPoint.position, Quaternion.identity);
        GameObject enemy = Instantiate(enemyPrefab[typeEnemy], enemySpawnPoint.position, Quaternion.identity);
        CreatedEnemy?.Invoke(enemy);
        spawnedEnemiesInWave++;
    }

    GameObject SetEnemyToBuild()
    {
        int index = 0;

        if (spawnedEnemiesInWave == 3)
        {
            index = 1;
        }
        else if (spawnedEnemiesInWave > 3)
        {
            index = 2;
        }

        if (spawnedEnemiesInWave >= 5)
            spawnedEnemiesInWave = 0;
        GameObject enemyToBuild = enemyPrefab[index];

        return enemyToBuild;
    }

    void SpawnWaves()
    {
        if (spawnedEnemiesTotal <= 5)
        {
            spawnedEnemiesTotal++;
            CreatEnemy(0);
        }
        else if (spawnedEnemiesTotal > 5 && spawnedEnemiesTotal <= 10)
        {
            if (type1inWave < 3)
            { CreatEnemy(0); type1inWave++; }
            else { CreatEnemy(1); type2inWave++; }

            if (type2inWave > 2)
            { type2inWave = 0; type1inWave = 0; }

            spawnedEnemiesTotal++;
        }
        else if (spawnedEnemiesTotal > 10)
        {
            if (type1inWave <= 3)
            {
                CreatEnemy(0);
                type1inWave++;
            }
            else if (type2inWave <= 3)
            {
                CreatEnemy(1);
                type2inWave++;
            }
            else if (type3inWave <= 3)
            {
                CreatEnemy(2);
                type3inWave++;
            }
            else
            {
                type1inWave = 0;
                type2inWave = 0;
                type3inWave = 0;
            }
        }

    }
}
