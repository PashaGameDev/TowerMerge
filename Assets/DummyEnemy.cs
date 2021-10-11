using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    [SerializeField] int Maxhelth = 100;

    private int currentHelth = 0;

    private void Awake()
    {
        currentHelth = Maxhelth; 
    }

    public void GetDemage(int demage)
    {
        currentHelth -= demage;
        if (currentHelth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
