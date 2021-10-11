using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas = null;
    [SerializeField] List<string> TutorialHints = new List<string>();
    [SerializeField] TextMeshProUGUI HintText = null;
    [SerializeField] Image unitIcon = null;
    [SerializeField] GameObject runBtn = null;
    [SerializeField] GameObject arrow = null;
    [SerializeField] GameObject tutorialUnit = null;
    [SerializeField] GameObject cell1 = null;
    [SerializeField] GameObject cell2 = null;
    [SerializeField] GameObject targetEnemyPrefab = null;
    [SerializeField] Transform enemySpawnPoint = null;
    [SerializeField] float unitCreatingDelay = 2f;
    [SerializeField] LayerMask rayLayer = new LayerMask();
    [SerializeField] GameObject blackBG = null;


    GameObject unit1 = null;
    GameObject unit2 = null;

    Vector3 startMousPosition = Vector3.zero;

    bool isUnitCreated = false;

    private int tutorialStep = 0; 

    public void SwitchTextInHint(int StepIndex)
    {
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

    void CreatTheUnit()
    {
        if (tutorialCanvas != null)
        { tutorialCanvas.SetActive(false); }

        if (tutorialUnit == null || cell1 == null) { return; }
        unitIcon.fillAmount = 0;
        StartCoroutine(unitCreation());
        cell1.GetComponent<Renderer>().material.color = Color.green;
    }

    IEnumerator unitCreation()
    {
        yield return new WaitForSeconds(0.01f);
        unitCreatingDelay -= 0.01f;
        unitIcon.fillAmount = unitIcon.fillAmount + 0.01f;
        if (unitCreatingDelay <= 0)
        {
            unitIcon.fillAmount = 1;
            unit1 = Instantiate(tutorialUnit, cell1.transform.position, Quaternion.identity);
            tutorialCanvas.SetActive(true);
            isUnitCreated = true;
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
          //  lastMousePosition = Input.mousePosition;
          //  if (choosenUnit != null) { choosenUnit.layer = 6; }
          //  Merge();
        }
#else

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case UnityEngine.TouchPhase.Began:
                   
                    break;

                case UnityEngine.TouchPhase.Moved:
                   

                    break;

                case UnityEngine.TouchPhase.Ended:
                   

                    break;
            }
        }

#endif
    }

    void CheckTouchedObject(Vector3 mPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, rayLayer)) { return; }

       // if (choosenUnit != null)
       // {
       //     choosenUnit.transform.position = new Vector3(hit.point.x, choosenUnit.transform.position.y, hit.point.z);
      //  }

        if (hit.collider.TryGetComponent<TutorialUnit>(out TutorialUnit unit) && tutorialCanvas.active)
        {
            hit.transform.gameObject.GetComponent<TutorialUnit>().Touched();// SetChoosenUnit(hit.collider.gameObject);
            tutorialCanvas.SetActive(false);
            if (targetEnemyPrefab == null || enemySpawnPoint == null) { return; }
            GameObject _t = Instantiate(targetEnemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
            hit.transform.gameObject.GetComponent<TutorialUnit>().SetTarget(_t);

            if (blackBG != null)
            {
                blackBG.GetComponent<Renderer>().material.color = Color.black;
            }
            return;
        }
        else
        {
          //  intersectedObject = null;
        }

    }
}
