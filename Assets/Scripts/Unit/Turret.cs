using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Unit
{
    [SerializeField] private Transform partToRotate = null;
    [SerializeField] private GameObject dieVFXPrefab = null; 
         
    private GameObject newTarget = null;
    private float chaseTimer = 0;

    public static event Action<GameObject> turretDie;
    public static event Action<GameObject> turretCreated;


    public int price = 15;
    public CellTurret cell;

    private void Start()
    {
        turretCreated?.Invoke(gameObject);
        if (isUnit)
        gameObject.layer = 10;
    }

    
    private void Update()
    {
        if (newTarget == null)
        {
            FindNewTarget();
            TurnOnOfShootVFX(false);
        }
        else
        {
            ChaseNewTarget();
        }
    }

    void FindNewTarget()
    {
        if (isUnit)
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
        else
        {
            foreach (var enemy in GameManager.instance.AllUnits)
            {
                float dis = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
                if (chasingDistance >= dis)
                {
                    newTarget = enemy;
                    break;
                }
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

        RotateToTarget(newTarget.transform.position - transform.position, partToRotate, 50f);
        if (chaseTimer <= 0)
        {
            newTarget.GetComponent<Unit>().GetDemage(demage);
            TurnOnOfShootVFX(true);
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
            cell.GetComponent<CellTurret>().SetState(false);
            cell.GetComponent<CellTurret>().turretOnPlace = null;
        }

        int currentHelth = getHelth();
        if (currentHelth <= 0 && dieVFXPrefab != null)
        {
            GameObject dieVFX = Instantiate(dieVFXPrefab, transform.position, Quaternion.identity);
            Destroy(dieVFX, 2f);
        }
    }

    private void TurnOnOfShootVFX(bool state)
    {
        if (shootVFX == null) { return; } 
        shootVFX.SetActive(state);
    }


}
