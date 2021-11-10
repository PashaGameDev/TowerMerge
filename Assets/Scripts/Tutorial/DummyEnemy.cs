using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyEnemy : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    [SerializeField] float Maxhelth = 100f;
    [SerializeField] Image hpImg = null;

    private TutorialController tConrol; 

    private float currentHelth = 0f;

    public void SetTControl(TutorialController t)
    {
        tConrol = t;
       
    }
    private void Awake()
    {
        currentHelth = Maxhelth; 
    }

    public void SetHelth()
    {
        GetDemage(currentHelth*0.7f);
        anim.SetBool("isGetDemage", false);

    }
    public void GetDemage(float demage)
    {
        anim.SetBool("isGetDemage", true);
        currentHelth -= demage;
        hpImg.fillAmount = ((100f/Maxhelth) * currentHelth)/100;
        if (currentHelth < 40f)
        { hpImg.color = Color.red; }
       
        if (currentHelth <= 0)
        {
            if (tConrol.tutorialStep == 2)
            {
                tConrol.SetPopupState(true, "Create two same Units",0f);
                tConrol.arrow.SetActive(true);
                tConrol.tutorialStep = 3;
            }
            else if (tConrol.tutorialStep == 6)
            {
                tConrol.tutorialStep = 7;
                tConrol.RunSuperShotStep();
            }
            else if (tConrol.tutorialStep == 7)
            {
                tConrol.SetPopupState(false,"",0f);
                tConrol.showFinalPopup();
            }
            Destroy(this.gameObject);
        }
    }
}
