using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float timeForThinking = 1.0f;
    [SerializeField] int defaultErningAmount = 5; 
    private List<EnemyCellManager> allCells = new List<EnemyCellManager>();
  
    [SerializeField] GameObject enemyTurretPrefab;
    [SerializeField] GameObject[] enemyTurretCells; 

    [SerializeField] private Text countDownText;
    [SerializeField] private float timerCountDown = 3f;
   
    public static event Action<GameObject> CreatedEnemy;

    IEnumerator DefaultErning()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.instance.IncreaseBalance(defaultErningAmount);
        GameManager.instance.IncreaseEnemyBalance(defaultErningAmount);

        StartCoroutine(DefaultErning());
    }
    IEnumerator MakeDecision()
    {
        yield return new WaitForSeconds(timeForThinking);
        
        int actionIndex = UnityEngine.Random.Range(1,3);
       
        switch (actionIndex)
        {
            case 1:
                BuildUnit();
                break;
            case 2:
               // BuildUnit();
                  SendEnemyFight();
                TryBuildTurrets();
                break;
            case 3:
                TryBuildTurrets();
                break;
            default:
                break;
        }

        StartCoroutine(MakeDecision());
    }

    void BuildUnit()
    {
        GameObject enemyToBuild = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length - 1 )];
         
        if (GameManager.instance.enemyBalance >= enemyToBuild.GetComponent<Enemy>().price)
        {
            int countCells = 0;
            foreach (var cell in allCells)
            {
                countCells++;
                if (cell.GetUnitOnPlace() == null)
                {
                    GameObject enemy = Instantiate(enemyToBuild, cell.gameObject.transform.position, Quaternion.identity);
                    cell.SetuUnitOnPlace(enemy);
                    enemy.GetComponent<Enemy>().cell = cell;
                    GameManager.instance.DecreaseEnemyBalace(enemyToBuild.GetComponent<Enemy>().price);
                    CreatedEnemy?.Invoke(enemy);
                    break;
                }
                if (countCells == allCells.Count) { SendEnemyFight(); }
            } 
        }
    }

    void SendEnemyFight()
    {
        foreach (var enemy in GameManager.instance.AllEnemies)
        {
            if (enemy == null) { return; }
            if (enemy.GetComponent<Enemy>() != null
                && !enemy.GetComponent<Enemy>().isCanMove
                && enemy.GetComponent<Enemy>().isOnBase)
            {
                enemy.GetComponent<Enemy>().isCanMove = true;
                enemy.GetComponent<Enemy>().isOnBase = false;
                enemy.GetComponent<Enemy>().cell.SetuUnitOnPlace(null);
                break;
            }
        }
    }
    void Merge()
    {
        Debug.Log("Merge");
    }

    private void Start()
    {
        
            var Cells = GameObject.FindObjectsOfType<EnemyCellManager>();
            foreach (var cell in Cells)
            {
                allCells.Add(cell);
            }
        
        StartCoroutine(MakeDecision());
        StartCoroutine(DefaultErning());
    }

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

       
    }

    void TryBuildTurrets()
    {
        foreach (var cell in enemyTurretCells)
        {
            if (cell.GetComponent<CellTurret>().turretOnPlace == null)
            {
                buildTurret(cell);
                break; 
            }
        }
    }

    void buildTurret(GameObject cellToBuild)
    {
        GameObject turret = Instantiate(enemyTurretPrefab, cellToBuild.transform.position, Quaternion.identity);
        cellToBuild.GetComponent<CellTurret>().SetState(true);
        cellToBuild.GetComponent<CellTurret>().turretOnPlace = turret;
    }

   
}
