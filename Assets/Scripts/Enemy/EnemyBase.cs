using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float timeForThinking = 1.0f;
    [SerializeField] int defaultErningAmount = 5;
    [SerializeField] GameObject superGunObject = null;
    [SerializeField] LineRenderer laser = null;
    [SerializeField] GameObject superShotPostEffect = null;
    [SerializeField] GameObject laserVFX = null;

    private List<EnemyCellManager> allCells = new List<EnemyCellManager>();
  
    [SerializeField] GameObject enemyTurretPrefab;
    [SerializeField] GameObject[] enemyTurretCells; 

    [SerializeField] private Text countDownText;
    [SerializeField] private float timerCountDown = 3f;

    [SerializeField] private int superShotPower = 40;
    [SerializeField] private float superShotTimer = 3f;
    private float countDown = 0f;
    private int superShotAmount = 0;
   
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
        
        int actionIndex = UnityEngine.Random.Range(1,4);
       
        switch (actionIndex)
        {
            case 1:
                BuildUnit();
                break;
            case 2:
                  BuildUnit();
                  SendEnemyFight();
               // TryBuildTurrets();
                break;
            case 3:
                 TryBuildTurrets();
                break;
            default:
                break;
        }

        StartCoroutine(MakeDecision());
    }

    void SuperShotCheck()
    {
        if (UnityEngine.Random.Range(1, 3) > 1) { return; }
        if (GameManager.instance.AllUnits != null)
        {

            foreach (var unit in GameManager.instance.AllUnits)
            {
                if (unit.GetComponent<Unit>() != null && unit.GetComponent<Unit>().getHelth() <= superShotPower)
                {
                    SuperShotAction(unit);
                    superShotAmount--;
                    break;
                }
            }
        }       
    }

    void SuperShotAction(GameObject unitToKill)
    {
        LookAtTarget(unitToKill.transform);
        laserVFX.SetActive(true);
        laser.SetPosition(0, superGunObject.transform.position);
        laser.SetPosition(1, unitToKill.transform.position);

        StartCoroutine(turnOffLaser());
        if (superShotPostEffect != null)
        {
            GameObject pVfx = Instantiate(superShotPostEffect, unitToKill.transform.position, Quaternion.identity);
            Destroy(pVfx,1.5f);
        }
        unitToKill.GetComponent<Unit>().GetDemage(superShotPower);    
    }

    IEnumerator turnOffLaser()
    {
        yield return new WaitForSeconds(1f);
        laserVFX.SetActive(false);
        laser.SetPosition(0, Vector3.zero);
        laser.SetPosition(1, Vector3.zero);
    }
    void BuildUnit()
    {
        int unitIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToBuild = enemyPrefabs[unitIndex];
        
         
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
       

        laser.SetPosition(0, Vector3.zero);
        laser.SetPosition(1, Vector3.zero);
    }

    void Update()
    {
        if (timerCountDown > 0)
        {
            timerCountDown -= Time.deltaTime;
            if(countDownText != null)
            countDownText.text = Mathf.RoundToInt(timerCountDown).ToString();

            if (timerCountDown < 1) { countDownText.text = "GO!"; }
            return;
        }
        else
        {
            if (countDownText != null)
                countDownText.gameObject.SetActive(false);
        }

        if (superShotAmount <= 0)
        {
            if (countDown > 0)
            {
                countDown -= Time.deltaTime;
            }
            else
            {
                countDown = superShotTimer;
                superShotAmount++;
            }
        }
        else
        {
            SuperShotCheck();
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

    void LookAtTarget(Transform targetPoit)
    {
        Vector3 dir = targetPoit.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(superGunObject.transform.rotation, lookRotation, Time.deltaTime * (10 * 2.75f)).eulerAngles;
        superGunObject.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
