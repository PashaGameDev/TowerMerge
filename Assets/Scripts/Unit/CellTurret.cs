using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CellTurret : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private int turretPrice = 35;
    [SerializeField] private GameObject canvas = null;

    public GameObject turretOnPlace = null;

    private void Update()
    {
        if (priceText == null) { return; }
        if (turretPrice >= GameManager.instance.currentBalance)
        {
            priceText.color = Color.red;
        }
        else
        {
            priceText.color = Color.white;
        }

    }

    public void PriceViewState(bool state)
    {
        if (canvas != null)
            canvas.SetActive(state);
    }

    public void SetState(bool state)
    {
        animator.SetBool("isOpen", state);
        
    }
}
