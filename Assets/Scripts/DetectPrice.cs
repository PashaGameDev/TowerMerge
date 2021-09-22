using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectPrice : MonoBehaviour
{
    [SerializeField] GameObject objectWithPrice = null;
    [SerializeField] Text priceText = null;  
    // Start is called before the first frame update
    void Start()
    {
        if (objectWithPrice == null) { return;}
        if (priceText == null) { return; }

        priceText.text =  objectWithPrice.GetComponent<Turret>().price.ToString();
    }
}
