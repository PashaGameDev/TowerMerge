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

    
    public void dispalyHelth(int maxHlth, int currentHelth)
    {
        // helthBar.fillAmount = (float)currentHelth / (float)maxHlth;
        // if (helthBar.fillAmount <= 0f) { helthCanvas.SetActive(false); }
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
