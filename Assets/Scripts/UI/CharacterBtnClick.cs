using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBtnClick : MonoBehaviour
{
    public int unitID = 0;
    public bool isChousen = false;

    [SerializeField] private Animator btnAnimator  = null;
    [SerializeField] private Image stateImg;

    [SerializeField] private AudioClip clickSFX = null;

    [SerializeField] private AudioSource audio = null;

    [SerializeField] private MainMenuController mController = null; 
    // Start is called before the first frame update

    private void Start()
    {
        if (stateImg == null) { return; }
        //changeStateStatus();

        mController = GameObject.FindGameObjectWithTag("mController").GetComponent<MainMenuController>();
        checkState();
    }

    void checkState()
    {
       int chosen =  PlayerPrefs.GetInt("UnitType"+unitID);
       
        if (chosen > 0)
        {
            btnClickReaction(false);
        }
        else
        {
            btnClickReaction(true);
        }
    }

    public void btnPressed()
    {
        btnClickReaction(isChousen);
    }
    void btnClickReaction(bool currentState)
    {
        if (currentState)
        {
            stateImg.gameObject.SetActive(false);
            mController.removeFromSquad(unitID);
            isChousen = false;
        }
        else
        {
            stateImg.gameObject.SetActive(true);
            mController.addToSquad(unitID);
            isChousen = true;
        }
    }

    /*public void btnClickReaction()
    {
        if (audio != null)
        {
            audio.clip = clickSFX;
            audio.Play();
        }
        btnAnimator.SetBool("isClicked", true);
        StartCoroutine(turnOffAnimation());
        if (isChousen)
        {
            stateImg.gameObject.SetActive(false);
            mController.removeFromSquad(unitID);
        }
        else
        {
            stateImg.gameObject.SetActive(true);
            mController.addToSquad(unitID);
        }
      //  AddUnitToCollection();
    }*/

    IEnumerator turnOffAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        btnAnimator.SetBool("isClicked", false);
    }

    void AddUnitToCollection()
    {
    //    if  (isChousen) { return; }
     //   changeStateStatus();
     //   if (canvas.sqaudCollection[unitID] == null) { return;}
    }
}
