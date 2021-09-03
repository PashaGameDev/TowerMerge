using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helth : MonoBehaviour
{
    [SerializeField] private Image helthBar;
    [SerializeField] private GameObject helthCanvas;
    [SerializeField] private GameObject minusLifeText;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject targetHighLight;


    public void TargetHighLigt()
    {
        if (targetHighLight == null)
        { return; }

        int helth = gameObject.GetComponent<Unit>().getHelth();

        if (GameManager.instance.superShotPower < helth || GameManager.instance.GetSuperShotAmount() <= 0)
        { targetHighLight.SetActive(false); return; }

        targetHighLight.SetActive(true);
    }
    
    public void dispalyHelth(int maxHlth, int currentHelth)
    {
       
        if (canvas == null)
        {
            helthBar.fillAmount = (float)currentHelth / (float)maxHlth;
            if (helthBar.fillAmount <= 0f) { helthCanvas.SetActive(false); }
            return;
        }
        GameObject minusHPText = Instantiate(minusLifeText, canvas.transform.position, Quaternion.identity, canvas.transform);
        minusHPText.GetComponent<Text>().text = "-" + (maxHlth - currentHelth);
        Destroy(minusHPText,2f);
    }
}
