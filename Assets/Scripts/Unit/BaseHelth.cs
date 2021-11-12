using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHelth : MonoBehaviour
{
    [SerializeField] int maxHelth = 100;
    private int helth = 100;
    [SerializeField] Helth helthView = null;
    [SerializeField] GameObject VFXExplosin = null;
   
    public static event Action<string> baseDistroyed;

    private void Start()
    {
        helth = maxHelth;
    }
    public void GetDemage(int demage)
    {
        helth -= demage;
        helthView.dispalyHelth(maxHelth,helth);

        if (gameObject.tag == "EnemyBase")
        {
            gameObject.GetComponent<EnemyBase>().ActivateAllUnits();
        }
        if (helth <= 0)
        {
            BaseDestroyed();
        }
    }

    public void BaseDestroyed()
    {
        if (gameObject.tag == "EnemyBase")
        {
            baseDistroyed?.Invoke("Win");
            GameManager.instance.RemoveEnemy(gameObject);
        }
        else {
            GameManager.instance.removeUnit(gameObject);
            baseDistroyed?.Invoke("Lose");
        }

        if (VFXExplosin != null)
        { Instantiate(VFXExplosin, gameObject.transform.position, Quaternion.identity); }
        Destroy(gameObject);
    }

}
