using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Unit
{
   
    private GameObject newTarget = null;
    private float chaseTimer = 0;

    public static event Action<GameObject> turretDie;
    public static event Action<GameObject> turretCreated;

    public CellTurret cell;

    private void Start()
    {
        turretCreated?.Invoke(gameObject);
    }

    
    private void Update()
    {
        if (newTarget == null)
        {
            FindNewTarget();
        }
        else
        {
            ChaseNewTarget();
        }
    }

    void FindNewTarget()
    {
        foreach (var enemy in GameManager.instance.AllEnemies)
        {
            float dis = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            if (chasingDistance >= dis)
            {
                newTarget = enemy;
                break;
            }
        }
    }

    void ChaseNewTarget()
    {
        float dis = Vector3.Distance(gameObject.transform.position, newTarget.transform.position);
        if (chasingDistance < dis)
        {
            chaseTimer = 0;
            newTarget = null;
            return;
        }
        RotateToTarget(newTarget.transform.position - transform.position);
        if (chaseTimer <= 0)
        {
            newTarget.GetComponent<Unit>().GetDemage(demage);
            chaseTimer = chasingRate; 
        }
        else
        {
            chaseTimer -= Time.deltaTime;
        }
    }
    private void OnDestroy()
    {
        turretDie?.Invoke(gameObject);
        if (cell != null)
        {
            cell.GetComponent<CellTurret>().turretOnPlace = null;
        }
    }


}
