 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnit : Unit
{
    public Color unitColor = new Color();
    public CellManager myCell = null;
    public static event Action<GameObject> unitDie;
    public static event Action<GameObject> unitCreated;
  

    public int price = 30;
    public int giveForKill = 20;

    private void Start()
    {
        unitCreated?.Invoke(gameObject);
        SetAllPointsList("Points");
        SetEnemyBase();
        GetComponent<Unit>().playSFX("created");
    }

    public void SetCell(CellManager cell)
    {
        myCell = cell;
        myCell.SetuUnitOnPlace(this.gameObject, unitColor); 
    }

    public CellManager GetCell()
    {
        return myCell;
    }

    
    void Update()
    {
        Move();
        if (GameManager.instance.isGameOver)
        {
           
            GetComponent<Unit>().playSFX("Stop");
        }

        if (isCanMove)
        {
            gameObject.layer = 2;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
           // myCell.GetComponent<CellManager>().SetuUnitOnPlace(null);
        }
        else { gameObject.GetComponent<Rigidbody>().isKinematic = true; }
       
    }

    public void cleanCell()
    {
        myCell.SetuUnitOnPlace(null, Color.white);
    }

    private void OnDestroy()
    {
        unitDie?.Invoke(gameObject);
    }


}
