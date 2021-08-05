using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public static event Action<GameObject> enemyDied;
    public int price = 10;

    private void Start()
    {
        SetAllPointsList("EnemyPoints");
        isCanMove = true;
        SetEnemyBase();
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
