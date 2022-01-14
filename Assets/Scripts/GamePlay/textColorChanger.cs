using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class textColorChanger : MonoBehaviour
{
    [SerializeField] private int UnitIndex;

    TMP_Text priceText;
    private int unitPrice;
    //Text priceText; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.instance.unitsArrayType1[UnitIndex] != null || priceText == null)
        {
            instParameters();
        }

        if (priceText == null) { return; }
        if (GameManager.instance.currentBalance >= unitPrice)
        {
            priceText.color = Color.white;
        }
        else
        {
            priceText.color = Color.red;
        }
    }

    void instParameters()
    {
        switch (UnitIndex)
        {
            case 0:
                if (GameManager.instance.unitsArrayType1[0] == null) { return; }
                unitPrice = GameManager.instance.unitsArrayType1[0].GetComponent<MyUnit>().price;
                break;

            case 1:
                if (GameManager.instance.unitsArrayType2[0] == null) { return; }
                unitPrice = GameManager.instance.unitsArrayType2[0].GetComponent<MyUnit>().price;
                break;

            case 2:
                if (GameManager.instance.unitsArrayType3[0] == null) { return; }
                unitPrice = GameManager.instance.unitsArrayType3[0].GetComponent<MyUnit>().price;
                break;

            default:
                break;
        }
       
        priceText = gameObject.GetComponent<TMP_Text>();
        priceText.text = unitPrice.ToString();
    }
}
