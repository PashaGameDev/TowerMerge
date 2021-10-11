using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBtnClick : MonoBehaviour
{
    public int unitID = 0;
    public bool isChousen = false;

    [SerializeField] private Animator btnAnimator  = null;
    [SerializeField] private StartSceneCanvas canvas;
    [SerializeField] private Image stateImg;
    // Start is called before the first frame update

    private void Start()
    {
        if (stateImg == null) { return; }
        changeStateStatus();
    }

    void changeStateStatus()
    {
        if (!isChousen)
        {
            stateImg.color = Color.white;
        }
        else
        {
            stateImg.color = Color.black;
        }
    }

    public void btnClickReaction()
    {
        btnAnimator.SetBool("isClicked", true);
     //   StartCoroutine(turnOffAnimation());
      //  AddUnitToCollection();
    }

    IEnumerator turnOffAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        btnAnimator.SetBool("isClicked", false);
    }

    void AddUnitToCollection()
    {
    //    if (isChousen) { return; }
     //   changeStateStatus();
     //   if (canvas.sqaudCollection[unitID] == null) { return;}
    }
}
