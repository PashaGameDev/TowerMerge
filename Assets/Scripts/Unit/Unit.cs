using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public int maxHelth = 100; 
    public int level = 1;
    public int unitType = 1;
    public float offset = 0.2f;
    public float minSpeed = 1.5f;
    public float maxSpeed = 5f;
    
    public int demage = 10;
    public float chasingRate = 2f;
    public float chasingDistance = 2f;
    public bool isUnit = true;
    public bool isCanMove = false;
    public Helth helthView = null;
    public Animator anim;
    public GameObject shootVFX = null;
    public Transform getDemagePosition = null;
    public GameObject gettingDemageVFX = null;

    private float speed = 5f;
    private int helth = 100;
    private GameObject target = null;
    private List<Transform> allPoints = new List<Transform>();
    private Transform targetPoit = null;
    private int pointIndex = 0;
    private float chasingT = 0f;
    private List<GameObject> targetList = new List<GameObject>();
    private GameObject enemyBase;

    public int getHelth()
    {
        return helth;
    }
    public void SetEnemyBase()
    {
        helth = maxHelth;
        speed = Random.Range(minSpeed,maxSpeed);
        if (isUnit)
        {
            enemyBase = GameObject.FindGameObjectWithTag("EnemyBase");
        }
        else
        {
            enemyBase = GameObject.FindGameObjectWithTag("PlayerBase");
        }
    }
    public void SetAllPointsList(string pointsTag)
    {
        Transform pointsParent = GameObject.FindGameObjectWithTag(pointsTag).transform;

        for (int i = 0; i < pointsParent.childCount; i++)
        {
            allPoints.Add(pointsParent.GetChild(i));
        }
        targetPoit = allPoints[0];
    }

    private void GettinDemageVisualisation()
    {
        if (getDemagePosition == null || gettingDemageVFX == null) { return; }
        Instantiate(gettingDemageVFX, getDemagePosition.position, Quaternion.identity, getDemagePosition);
    }
    public void GetDemage(int demageAmount)
    { 
        helth -= demageAmount;
        if(helthView != null)
        helthView.dispalyHelth(maxHelth, helth);
        GettinDemageVisualisation();
        if (helth <= GameManager.instance.superShotPower)
        {
            helthView.TargetHighLigt();
        }
       
        if (helth <= 0)
        {
            Die();
        }


       
    }

    public void Move()
    {
        if (!isCanMove) { return; }
        
        FindTarget();

        if (target != null) { return; }

       
        if (targetPoit == null) {  GetNextPoint(); return; }
   
        if (Vector3.Distance(transform.position, targetPoit.position) <= offset) { GetNextPoint(); return; }
        if (shootVFX != null) { shootVFX.SetActive(false); }
        Vector3 dir = targetPoit.position - transform.position;
        RotateToTarget(dir, transform);
        dir = new Vector3(dir.x, dir.y, dir.z);
        SwitchAnimation("isMove", true);
        SwitchAnimation("isShoot", false);
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    void GetNextPoint()
    {
       
        if (enemyBase == null) { return; }
        float minDis = Vector3.Distance(transform.position, allPoints[0].transform.position);
        int index = 0;
       
        foreach (var point in allPoints)
        {
            index++;
            float dis = Vector3.Distance(transform.position, point.transform.position);
            if (dis <= minDis)
            {
                minDis = dis;
                pointIndex = index;
            }
        }
 
       if (pointIndex >= allPoints.Count && enemyBase != null) { targetPoit = enemyBase.transform; return; }
        targetPoit = allPoints[pointIndex];

    }

    void AttackBase()
    { 
        if (chasingT <= 0)
        {
            if (shootVFX != null) { shootVFX.SetActive(true); }
            if (enemyBase == null) { return; }
            enemyBase.GetComponent<BaseHelth>().GetDemage(demage);
            chasingT = chasingRate;
        }
        else
        {
            chasingT -= Time.deltaTime;
        }
        Rigidbody rd = gameObject.GetComponent<Rigidbody>();
        if (rd != null) rd.velocity = Vector3.zero;
    }

    void FindTarget()
    {
        if (!isUnit)
        {
            targetList = GameManager.instance.AllUnits;
        }
        else
        {
            targetList = GameManager.instance.AllEnemies;
        }

        if (target != null) { if (shootVFX != null) { shootVFX.SetActive(true); } ChaseTarget();return; }
        foreach (GameObject enemy in targetList)
        {
            if (enemy == null) { return; }
            float dis = Vector3.Distance(gameObject.transform.position,enemy.transform.position);
            
            if (dis <= chasingDistance)
            {
                target = enemy;
                Vector3 dir = target.transform.position - transform.position;
                RotateToTarget(dir, transform);
                SwitchAnimation("isShoot", true);
                break;
            }
        }
    }

    void ChaseTarget()
    {
        if (target == null ) {  return; } //|| target == enemyBase)
        if (chasingT <= 0)
        {
            if (target.GetComponent<Unit>() == null)
            { enemyBase.GetComponent<BaseHelth>().GetDemage(demage); }
            else { target.GetComponent<Unit>().GetDemage(demage); }

            
            chasingT = chasingRate;
        }
        else
        {
            chasingT -= Time.deltaTime;
        }
    }

    public void Die()
    {
        
        if (shootVFX != null) { shootVFX.SetActive(false); }
        if (isUnit && gameObject.GetComponent<Rigidbody>() != null )
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            SwitchAnimation("isDied", true);
            isCanMove = false;
            Destroy(this.gameObject, 2f);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void RotateToTarget(Vector3 dir, Transform partToRotate)
    {
        if (partToRotate == null) { partToRotate = transform; }
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * (speed * 2.75f)).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    public void SwitchAnimation(string animTag, bool state)
    {
        if (anim == null) { return; }

        anim.SetBool(animTag,state);
    }
    
   
}
