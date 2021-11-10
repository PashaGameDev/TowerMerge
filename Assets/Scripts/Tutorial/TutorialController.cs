using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    
    [SerializeField] GameObject tutorialCanvas = null;
    [SerializeField] GameObject finalPanel = null;
    [SerializeField] List<string> TutorialHints = new List<string>();
    [SerializeField] TextMeshProUGUI HintText = null;
    [SerializeField] Image unitIcon = null;
    [SerializeField] GameObject runBtn = null;
    public GameObject arrow = null;
    [SerializeField] GameObject tutorialUnit = null;
    public GameObject tutorialUnit2 = null;
    public  GameObject cell1 = null;
    public  GameObject cell2 = null;
    [SerializeField] GameObject targetEnemyPrefab = null;
    [SerializeField] Transform enemySpawnPoint = null;
    [SerializeField] float unitCreatingDelay = 2f;
    [SerializeField] LayerMask rayLayer = new LayerMask();
    [SerializeField] GameObject blackBG = null;


    GameObject unit1 = null;
    GameObject unit2 = null;
    GameObject buildSpot = null;

    public GameObject intersectedObject = null;
    public GameObject choosenUnit = null;


    private int createdUnits = 0;
    private int amountEnemy = 0;

    Vector3 startPositionChoosenUnit = Vector3.zero;
    Vector3 startMousPosition = Vector3.zero;
    Vector3 lastMousePosition = Vector3.zero;

    public bool isUnitCreated = false;

    public int tutorialStep = 0;


    public void SetPopupState(bool state, string newHintText, float bgAlpha)
    {
        HintText.text = newHintText;
        tutorialCanvas.SetActive(state);
        tutorialCanvas.GetComponent<Image>().color = new Color(0f,0f,0f,bgAlpha);
        runBtn.SetActive(false);
    }

    public void Step1RunTutorial()
    {
        SetPopupState(true, "Create the Unit",0.5f);
        tutorialStep = 1;
       if (arrow != null)
        arrow.SetActive(true);  
    }

    public void CreatUnitBtn()
    {
        if (tutorialStep == 1)
        { CreatTheUnit(2, cell1); }
        else
        if (tutorialStep == 3)
        { CreatTheUnit(4, cell1); }
        else
        if (tutorialStep == 4)
        { CreatTheUnit(5, cell2); }
        else { return; }
    }

    public void CreatTheUnit(int newStepNumber, GameObject cellType)
    {
        StartCoroutine(unitCreation());
        unitIcon.fillAmount = 0;
        buildSpot = cellType;
        buildSpot.GetComponent<Renderer>().material.color = Color.green;
        tutorialStep = newStepNumber;
    }

    public void RunSuperShotStep()
    {
        SetPopupState(true, "Finish Him",0f);
        CreatEnemy(null);
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
            unit1.GetComponent<TutorialUnit>().SetController(gameObject.GetComponent<TutorialController>());
            arrow.SetActive(false);
            unitCreatingDelay = 1f;
           
            SetPopupState(true, TutorialHints[tutorialStep], 0f);
            if (tutorialStep == 2)
            { CreatEnemy(unit1.GetComponent<TutorialUnit>()); }
            else if(tutorialStep == 4) {arrow.SetActive(true); }
        }
        else
        {
            StartCoroutine(unitCreation());
        }

    }

    public void CreatEnemy(TutorialUnit unit)
    {
        GameObject enemy = Instantiate(targetEnemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        enemy.GetComponent<DummyEnemy>().SetTControl(gameObject.GetComponent<TutorialController>());

        if (unit != null)
        {
            unit.SetTarget(enemy);
        }
        else
        {
            enemy.GetComponent<DummyEnemy>().SetHelth();
            enemy.layer = 9;
        }

    }

    public void showFinalPopup()
    {
        StartCoroutine(showFinalPopupDelay());
    }

    IEnumerator showFinalPopupDelay()
    {
        yield return new WaitForSeconds(2f);
        finalPanel.SetActive(true);
    }

    public void GoHomeBtn()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
}
