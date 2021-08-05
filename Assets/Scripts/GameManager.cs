using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentBalance = 100;
    public Text balanceText;
    public static GameManager instance = null;
    private List<CellManager> cells = new List<CellManager>();

    public GameObject[] unitsArrayType1 = null;
    public GameObject[] unitsArrayType2 = null;
    public GameObject[] unitsArrayType3 = null;

    public Image[] unitIconCountDown; 
    public GameObject getUnitToCreat(int unitType, int index)
    {
        switch (unitType)
        {
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
        balanceText.text = currentBalance.ToString();
    }

    /*
    public void CreatUnit(int index)
    {
        //if (currentBalance < price) { return; }
        foreach (var cell in cells)
        {
            if (cell.GetUnitOnPlace() == null)
            {

                
                Vector3 pos = cell.gameObject.transform.position;
                pos.y = pos.y + 1;
                GameObject newUnit = getUnitToCreat(index);
                int price = newUnit.GetComponent<MyUnit>().price;
                if (currentBalance < price) { return; }

                GameObject unit = Instantiate(newUnit, pos, Quaternion.identity);
             
                unit.GetComponent<MyUnit>().SetCell(cell);
                currentBalance -= price;
                balanceText.text = currentBalance.ToString();
                break;
            }
        }
        
    }*/

    public void OnDestroy()
    {
        EnemyBase.CreatedEnemy -= AddEnemy;
        Enemy.enemyDied -= RemoveEnemy;

        MyUnit.unitDie -= removeUnit;
        MyUnit.unitCreated -= AddUnit;

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
        currentBalance += enemy.GetComponent<Enemy>().price;

        balanceText.text = currentBalance.ToString();

        AllEnemies.Remove(enemy);
    }

    void AddUnit(GameObject unit)
    {
        AllUnits.Add(unit);
    }

    public void removeUnit(GameObject unit)
    {
        AllUnits.Remove(unit);
    }

    public void GameOver(string result)
    {
        if (result == "Win")
        {
            Debug.Log("Won");
        }
        else
        {
            Debug.Log("Lose");
        }
    }
    public void StartCountDownBuilUnit(int btnIndex)
    {
        StartCoroutine(buildUnitCountDown(0.1f,btnIndex));
    }
    
    IEnumerator buildUnitCountDown(float waitTime, int btnIndex)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Coroutine");
        buildCountDown -= 0.1f;
        if (buildCountDown > 0f)
        {
            StartCoroutine(buildUnitCountDown(0.1f, btnIndex));
        }
        else
        {

        }
    }

}
