using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBtnClick : MonoBehaviour
{
    [SerializeField] private Animator btnAnimator  = null;
    // Start is called before the first frame update

    public void btnClickReaction()
    {
        btnAnimator.SetBool("isClicked", true);
        StartCoroutine(turnOffAnimation());
    }

    IEnumerator turnOffAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        btnAnimator.SetBool("isClicked", false);
    }
}
