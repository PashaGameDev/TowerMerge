using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text EnemyBalanceText;

    public int superShotPower = 30;
    public int currentBalance = 100;
    public int enemyBalance = 60; 
    public TMP_Text balanceText;
    public static GameManager instance = null;
    private List<CellManager> cells = new List<CellManager>();
    

    public GameObject[] unitsArrayType1 = null;
    public GameObject[] unitsArrayType2 = null;
    public GameObject[] unitsArrayType3 = null;
    public GameObject[] turretsArray = null;
    public GameObject gameOverPanel = null;

    public Image[] unitIconCountDown; 
    public GameObject getUnitToCreat(int unitType, int index)
    {
        switch (unitType)
        {
            case 0:
                if (index >= turretsArray.Length) { return null; }
                return turretsArray[index];
            case 1:
                if (index >= unitsArrayType1.Length) { return null; }
                return unitsArrayType1[index];
            case 2:
                if (index >= unitsArrayType2.Length) { return null; }
                return unitsArrayType2[index];
            case 3:
                if (index >= unitsArrayType3.Length) { return null; }
                return unitsArrayType3[index];
            default:
                return null;
                break;
        }
    }

    public List<GameObject> AllEnemies = new List<GameObject>();
    public List<GameObject> AllUnits = new List<GameObject>();

    private int killedEnemy = 0;
    private float buildCountDown = 1f;
    private int superShotAmount = 0;

    public void DecreaseEnemyBalace(int amount)
    {
        enemyBalance -= amount;
        EnemyBalanceText.text = enemyBalance.ToString();
    }

    public void IncreaseEnemyBalance(int amount)
    {
        enemyBalance += amount;
        EnemyBalanceText.text = enemyBalance.ToString();
    }

    public int GetSuperShotAmount()
    {
        return superShotAmount;
    }

    public void SetSuperShotAmount(int amount)
    {
        superShotAmount = amount;
    }

    public int GetKilledAmount()
    {
        return killedEnemy;
    }

    void Start()
    {
        if (instance == null)
        { 
            instance = this; 
        }
        else if (instance == this)
        { 
            Destroy(gameObject); 
        }
           GameObject enemyBase = GameObject.FindGameObjectWithTag("EnemyBase");
        
           GameObject playerBase = GameObject.FindGameObjectWithTag("PlayerBase");

        AllUnits.Add(playerBase);
        AllEnemies.Add(enemyBase);
       

        balanceText.text =  currentBalance.ToString();

        EnemyBase.CreatedEnemy += AddEnemy;
        Enemy.enemyDied += RemoveEnemy;

        MyUnit.unitCreated += AddUnit;
        MyUnit.unitDie += removeUnit;

        Turret.turretCreated += AddTurret;
        Turret.turretDie += RemoveTurret;

        BaseHelth.baseDistroyed += GameOver;

        var AllCellsOnScene = GameObject.FindObjectsOfType<CellManager>();

        foreach (var cell in AllCellsOnScene)
        {
            cells.Add(cell);
        }
    }

    public CellManager GetCell()
    {
        foreach (var cell in cells)
        {
            if (cell.GetUnitOnPlace() == null)
            {
                return cell;
               // break
            }
        }
        return null;
    }

    public bool isPurchaseble(int price)
    {
        if (price <= currentBalance)
        {
            return true;
        }
        else { return false; }
    }

    public void decreaseBalace(int amount)
    {
        currentBalance -= amount;
        if (balanceText !=null)
        balanceText.text = currentBalance.ToString();
    }

    public void IncreaseBalance(int amount)
    {
        currentBalance += amount;
        if (balanceText != null)
            balanceText.text = currentBalance.ToString();
    }

    public void OnDestroy()
    {
        EnemyBase.CreatedEnemy -= AddEnemy;
        Enemy.enemyDied -= RemoveEnemy;

        MyUnit.unitDie -= removeUnit;
        MyUnit.unitCreated -= AddUnit;

        Turret.turretCreated -= AddTurret;
        Turret.turretDie -= RemoveTurret;

        BaseHelth.baseDistroyed -= GameOver;
    }

    void AddEnemy(GameObject enemy)
    {
        AllEnemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        killedEnemy++;

        if(enemy.GetComponent<Enemy>() != null)
        IncreaseBalance(enemy.GetComponent<Enemy>().giveForKill);

        AllEnemies.Remove(enemy);
    }

    void AddUnit(GameObject unit)
    {
        AllUnits.Add(unit);
    }

    public void removeUnit(GameObject unit)
    {
        if (unit.GetComponent<Unit>() != null) { IncreaseEnemyBalance(unit.GetComponent<MyUnit>().giveForKill); }
        
        AllUnits.Remove(unit);
    }

    void AddTurret(GameObject turret)
    {
        if (turret.GetComponent<Unit>().isUnit)
        {
            AllUnits.Add(turret);
        }
        else { AllEnemies.Add(turret); }
    }

    public void RemoveTurret(GameObject turret)
    {
        if (turret.GetComponent<Unit>().isUnit)
        {
            IncreaseEnemyBalance(turret.GetComponent<Turret>().price);
            AllUnits.Remove(turret);
            
        }
        else
        {
            IncreaseBalance(turret.GetComponent<Turret>().price);
            AllEnemies.Remove(turret);
        }
        
    }

    

    public void StartCountDownBuilUnit(int btnIndex)
    {
        StartCoroutine(buildUnitCountDown(0.1f,btnIndex));
    }
    
    IEnumerator buildUnitCountDown(float waitTime, int btnIndex)
    {
        yield return new WaitForSeconds(waitTime);
       
        buildCountDown -= 0.1f;
        if (buildCountDown > 0f)
        {
            StartCoroutine(buildUnitCountDown(0.1f, btnIndex));
        }
    }

    public void GameOver(string result)
    {
        Time.timeScale = 0.1f;
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<GameOver>().resultTest.text = "You " + result + "!!!";
    }
}
