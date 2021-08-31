using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public static event Action<GameObject> enemyDied;
    public int price = 10;

    [SerializeField] private bool isTurret = false;

    private void Start()
    {
       
        SetAllPointsList("EnemyPoints");
        SetEnemyBase();

        if (isTurret) { return; }
        isCanMove = true;
    }
    private void Update()
    {
        Move();
    }

    private void OnDestroy()
    {
        enemyDied?.Invoke(gameObject);
    }
   
}
