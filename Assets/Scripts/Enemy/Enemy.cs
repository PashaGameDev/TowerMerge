using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public static event Action<GameObject> enemyDied;
    public EnemyCellManager cell; 
    public int price = 10;
    public int giveForKill = 20;
    public bool isOnBase = true;

    [SerializeField] private bool isTurret = false;
   

    private void Start()
    {
       
        SetAllPointsList("EnemyPoints");
        SetEnemyBase();
       // if (isTurret) { return; }
       // isCanMove = true;
    }
    private void Update()
    {
        Move();

        if (!isOnBase)
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    private void OnDestroy()
    {
        enemyDied?.Invoke(gameObject);
    }
   
}
