using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] GameObject [] enemyPrefab;
    [SerializeField] Transform enemySpawnPoint;
    [SerializeField] float timeRate = 2f;

    [SerializeField] private Text countDownText;
    [SerializeField] private float timerCountDown = 3f;



    public static event Action<GameObject> CreatedEnemy;

    private float t = 0f;
    private int spawnedEnemiesInWave = 0;
    
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
            CreatEnemy();
            t = timeRate;
        }
        else
        {
            t -= Time.deltaTime;
        }
    }
    void CreatEnemy()
    {
        //if (GameManager.instance.AllEnemies.Count >= 2) { return; }
       GameObject enemy = Instantiate(SetEnemyToBuild(), enemySpawnPoint.position, Quaternion.identity);
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
    
}
