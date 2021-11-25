using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Helth : MonoBehaviour
{
    [SerializeField] private Image helthBar;
   // [SerializeField] private GameObject helthCanvas;
    [SerializeField] private GameObject minusLifeText;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject targetHighLight;


    public void TargetHighLigt()
    {
        
        if (targetHighLight == null)
        { return; }

        int helth = gameObject.GetComponent<Unit>().getHelth();

        if (GameManager.instance.superShotPower < helth || GameManager.instance.GetSuperShotAmount() <= 0)
        {
            helthBar.color = Color.green;
            return;
        }
       // targetHighLight.SetActive(true);

        helthBar.color = Color.red;
    }
    
    public void dispalyHelth(int maxHlth, int currentHelth)
    {
        TargetHighLigt();
       
        if (canvas != null && helthBar != null)
        {
            helthBar.fillAmount = (float)currentHelth / (float)maxHlth;
            if (helthBar.fillAmount <= 0f) { canvas.SetActive(false); }
            return;
        }
        if ((maxHlth - currentHelth) > 0 && minusLifeText != null)
        {
            GameObject minusHPText = Instantiate(minusLifeText, canvas.transform.position, Quaternion.identity, canvas.transform);
            if (minusHPText == null) { return; }
            minusHPText.GetComponent<TextMeshPro>().text = "-" + (maxHlth - currentHelth);
            Destroy(minusHPText, 0.8f);
        }
        
    }
}
