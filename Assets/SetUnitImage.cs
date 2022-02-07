using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUnitImage : MonoBehaviour
{
    [SerializeField] Image unitImage = null;
    [SerializeField] Animator anim; 
    public bool isAvailable = true;
    public int ID = 0;

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }
    public void setImage(Sprite img, int unitType)
    {
        unitImage.gameObject.SetActive(true);
        unitImage.sprite = img;
        isAvailable = false;
        ID = unitType;
        anim.SetBool("isClicked", true);
        StartCoroutine(turnOffAnimation());
    }

    public void removeImage()
    {
        unitImage.gameObject.SetActive(false);
        isAvailable = true;
        ID = 0;
    }

    IEnumerator turnOffAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        anim.SetBool("isClicked", false);
    }
}
