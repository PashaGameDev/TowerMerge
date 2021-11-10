using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUnit : MonoBehaviour
{
    public float speed = 10f;

    [SerializeField] Animator anim = null;
    [SerializeField] GameObject arrowPointer = null;
    [SerializeField] GameObject shootFVX = null;

    private float offset = 0.5f;
    private int pointIndex = 0;

    private GameObject targetEnemy = null; 
    private Transform targetPoit = null;
    private List<Transform> allPoints = new List<Transform>();
    private TutorialController tutorialController = null;
    private bool isCanMove = false;

    public void SetController(TutorialController _t)
    {
        tutorialController = _t;
    }
    public void SetTarget(GameObject _t)
    {
        targetEnemy = _t;
    }

    public void Touched()
    {
        isCanMove = true;
    }
    private void Start()
    {
        SetAllPointsList("Points");
        
    }
    private void Update()
    {
        if (tutorialController.tutorialStep == 2|| tutorialController.tutorialStep == 6) { arrowPointer.SetActive(true); } else { arrowPointer.SetActive(false); }
       
        if (!isCanMove && anim.GetBool("isShoot")) { arrowPointer.SetActive(false); return; }
        if (!isCanMove) { return; }
        arrowPointer.SetActive(false);
        Move();
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

    private void Move()
    {
        if (targetPoit == null) { GetNextPoint(); return; }

        if (Vector3.Distance(transform.position, targetPoit.position) <= offset) { GetNextPoint(); return; }
       
        Vector3 dir = targetPoit.position - transform.position;
        RotateToTarget(dir, transform, speed);
        dir = new Vector3(dir.x, dir.y, dir.z);
      
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    void GetNextPoint()
    {
        int index = 0;
        float minDis = Vector3.Distance(transform.position, allPoints[0].transform.position);

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

        if (pointIndex >= allPoints.Count)
        {
            isCanMove = false;
           
            StartCoroutine(AttachEnemy());
            anim.SetBool("isShoot", true);
            shootFVX.SetActive(true);
            return;
        }
        targetPoit = allPoints[pointIndex];
        anim.SetBool("isMove", true);
    }

    IEnumerator AttachEnemy()
    {
        yield return new WaitForSeconds(0.2f);
        if (targetEnemy!= null)
        targetEnemy.GetComponent<DummyEnemy>().GetDemage(10f);
        if (targetEnemy != null)
        {
            StartCoroutine(AttachEnemy());
        }
        else
        {
          //  tutorialController.SwitchTextInHint(tutorialController.tutorialStep);
            Destroy(this.gameObject);
        }

    }

    public void RotateToTarget(Vector3 dir, Transform partToRotate, float rSpeed)
    {
        if (partToRotate == null) { partToRotate = transform; }
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * (rSpeed * 2.75f)).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
