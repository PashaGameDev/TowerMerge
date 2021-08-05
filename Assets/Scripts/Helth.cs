using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helth : MonoBehaviour
{
    [SerializeField] private Image helthBar;
    [SerializeField] private GameObject helthCanvas;

    
    public void dispalyHelth(int maxHlth, int currentHelth)
    {
        helthBar.fillAmount = (float)currentHelth / (float)maxHlth;
        if (helthBar.fillAmount <= 0f) { helthCanvas.SetActive(false); }
    }
}
