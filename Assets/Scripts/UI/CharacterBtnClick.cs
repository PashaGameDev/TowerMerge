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

    public int SpotID = 0;

    private void Start()
    {
        checkIfInSquad();
    }
    public void btnPressed()
    {
        if (mController == null) { return; }
        bool isCh = mController.unitData.checkInSquad(unitID);
        btnClickReaction(isCh);
        btnAnimator.SetBool("isClicked", true);
        StartCoroutine(turnOffAnimation());
    }

    void btnClickReaction(bool currentState)
    {
        if (currentState)
        {
            mController.removeFromSquad(unitID, this);
        }
        else
        {
            mController.addToSquad(unitID, this);
        }
    }

    void checkIfInSquad()
    {
        foreach (var unit in mController.unitData.squad)
        {
            if (unit.GetComponent<MyUnit>().unitType == unitID)
            {
                SwitchMarker(true);
                isChousen = true;
                break;
            }
        }
    }

    public void SwitchMarker(bool markerState)
    {
        stateImg.gameObject.SetActive(markerState);
    }

     IEnumerator turnOffAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        btnAnimator.SetBool("isClicked", false);
    }

   
}
