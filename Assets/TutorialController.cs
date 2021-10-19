using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    [SerializeField] float mouseOffset = 0.7f;
    [SerializeField] GameObject tutorialCanvas = null;
    [SerializeField] List<string> TutorialHints = new List<string>();
    [SerializeField] TextMeshProUGUI HintText = null;
    [SerializeField] Image unitIcon = null;
    [SerializeField] GameObject runBtn = null;
    [SerializeField] GameObject arrow = null;
    [SerializeField] GameObject tutorialUnit = null;
    [SerializeField] GameObject tutorialUnit2 = null;
    [SerializeField] GameObject cell1 = null;
    [SerializeField] GameObject cell2 = null;
    [SerializeField] GameObject targetEnemyPrefab = null;
    [SerializeField] Transform enemySpawnPoint = null;
    [SerializeField] float unitCreatingDelay = 2f;
    [SerializeField] LayerMask rayLayer = new LayerMask();
    [SerializeField] GameObject blackBG = null;


    GameObject unit1 = null;
    GameObject unit2 = null;
    GameObject buildSpot = null;

    GameObject intersectedObject = null;
    GameObject choosenUnit = null;


    private int createdUnits = 0;

    Vector3 startPositionChoosenUnit = Vector3.zero;
    Vector3 startMousPosition = Vector3.zero;
    Vector3 lastMousePosition = Vector3.zero;

    bool isUnitCreated = false;

    public int tutorialStep = 0; 

    public void SwitchTextInHint(int StepIndex)
    {
        Debug.Log("StepIdex = "+StepIndex);

        if (StepIndex == 4)
        {
            SuperShotStep();
            return;
        }

        if (StepIndex == 1 && tutorialStep == 3)
        {
           CreatTheUnit();
        }
        if (StepIndex != tutorialStep) { return; }
        tutorialStep++;
        
        if (tutorialStep >= TutorialHints.Count) { return; }
        HintText.text = TutorialHints[tutorialStep];
       
        RunTutorialStep();
       
        
    }

    void RunTutorialStep()
    {
        switch (tutorialStep)
        {
            case 1:
                Step1();
                break;
            case 2:
                Step2();
                break;
            case 3:
                Step3();
                break;
           case 5:
                Step4();
                break;
            default:
                break;
        }
    }

    void Step1()
    {
        if (arrow != null)
            arrow.SetActive(true);
        runBtn.SetActive(false);
    }

    void Step2()
    {
        if (arrow != null)
        arrow.SetActive(false);
        CreatTheUnit();
    }

    void Step3()
    {
        tutorialCanvas.SetActive(true);
    }

    void Step4()
    {
        tutorialCanvas.SetActive(true);
    }

    void CreatTheUnit()
    {
        if (tutorialCanvas != null)
        { tutorialCanvas.SetActive(false); }

        if (tutorialUnit == null || cell1 == null) { return; }
        if (createdUnits > 2) { return; }
        unitIcon.fillAmount = 0;
        StartCoroutine(unitCreation());
        if (createdUnits <= 1) { buildSpot = cell1; } else { buildSpot = cell2; }
        buildSpot.GetComponent<Renderer>().material.color = Color.green;
    }

    IEnumerator unitCreation()
    {
        yield return new WaitForSeconds(0.01f);
        unitCreatingDelay -= 0.01f;
        unitIcon.fillAmount = unitIcon.fillAmount + 0.01f;
        if (unitCreatingDelay <= 0)
        {
            unitIcon.fillAmount = 1;  
            unit1 = Instantiate(tutorialUnit, buildSpot.transform.position, Quaternion.identity);
            unit1.GetComponent<TutorialUnit>().SetController(this.gameObject.GetComponent<TutorialController>());
            tutorialCanvas.SetActive(true);
            isUnitCreated = true;
            unitCreatingDelay = 1f;
            createdUnits++;
            if (createdUnits > 2) { SwitchTextInHint(tutorialStep); }
        }
        else
        {
            StartCoroutine(unitCreation());
        }
    }

    private void Update()
    {
        
        if (!isUnitCreated) { return; }

#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startMousPosition = Input.mousePosition;
            
        }
        if (Mouse.current.leftButton.isPressed)
        {
            CheckTouchedObject(Input.mousePosition);
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            lastMousePosition = Input.mousePosition;
            if (choosenUnit != null) { choosenUnit.layer = 6;}
            
             Merge();
        }
#else

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case UnityEngine.TouchPhase.Began:
                    startMousPosition = touch.position;
                    CheckTouchedObject(touch.position);
                    break;

                case UnityEngine.TouchPhase.Moved:
                   CheckTouchedObject(touch.position);

                    break;

                case UnityEngine.TouchPhase.Ended:
                    lastMousePosition = touch.position;
                    if (choosenUnit != null) { choosenUnit.layer = 6; }
                    Merge()

                    break;
            }
        }

#endif
    }
    void CreatEnemy(TutorialUnit unit)
    {
        if (unit == null) { return; }
        GameObject enemy = Instantiate(targetEnemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        unit.SetTarget(enemy);
    }
    void Merge()
    {
        float mouseMoveDis = Vector3.Distance(startMousPosition, lastMousePosition);
        if (mouseMoveDis <= mouseOffset && choosenUnit != null)
        {
            if (tutorialStep == 2 || tutorialStep == 4)
            {
                choosenUnit.GetComponent<TutorialUnit>().Touched();
                CreatEnemy(choosenUnit.GetComponent<TutorialUnit>());
                tutorialCanvas.SetActive(false);
                // choosenUnit.GetComponent<MyUnit>().myCell.SetuUnitOnPlace(null, Color.white);
                choosenUnit = null;
                cell1.GetComponent<Renderer>().material.color = Color.white;
                cell2.GetComponent<Renderer>().material.color = Color.white;

                CleanChoosenUnit();
                return;
            }
        }
        

        if (choosenUnit != null && intersectedObject != null)
        {
            Vector3 instPost = cell2.transform.position;
            GameObject unit3 =  Instantiate(tutorialUnit2, instPost, Quaternion.identity);
            unit3.GetComponent<TutorialUnit>().SetController(this.gameObject.GetComponent<TutorialController>());
            
            HintText.text = TutorialHints[tutorialStep+1];
            cell1.GetComponent<Renderer>().material.color = Color.white;
            cell2.GetComponent<Renderer>().material.color = Color.yellow;
            Destroy(choosenUnit);
            Destroy(intersectedObject);
        
        }
        else
        {
            CleanChoosenUnit();
        }
    }

    void CheckTouchedObject(Vector3 mPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, rayLayer)) { return; }

        if (choosenUnit != null)
        {
            choosenUnit.transform.position = new Vector3(hit.point.x, choosenUnit.transform.position.y, hit.point.z);
            tutorialCanvas.SetActive(false);
        }

        if (hit.collider.TryGetComponent<TutorialUnit>(out TutorialUnit tUnit))
        {
            SetChoosenUnit(hit.collider.gameObject);
        }
        else
        {
            intersectedObject = null;
        }

    }

    void SetChoosenUnit(GameObject t)
    { 
        if (choosenUnit != null) { intersectedObject = t; return; }

        choosenUnit = t;
        choosenUnit.layer = 8;

        startPositionChoosenUnit = choosenUnit.transform.position;
    }

    void CleanChoosenUnit()
    {
        if (choosenUnit != null) { tutorialCanvas.SetActive(true); choosenUnit.transform.position = startPositionChoosenUnit; }

        choosenUnit = null;

        intersectedObject = null;
    }

    void SuperShotStep()
    {
        GameObject enemy = Instantiate(targetEnemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        enemy.GetComponent<DummyEnemy>().SetHelth();
    }

}
