using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyEnemy : MonoBehaviour
{
    [SerializeField] float Maxhelth = 100f;
    [SerializeField] Image hpImg = null;
   

    private float currentHelth = 0f;

    private void Awake()
    {
        currentHelth = Maxhelth; 
    }

    public void SetHelth()
    {
        GetDemage(currentHelth*0.7f);

    }
    public void GetDemage(float demage)
    {
        currentHelth -= demage;
        hpImg.fillAmount = ((100f/Maxhelth) * currentHelth)/100;
        if (currentHelth < 40f)
        { hpImg.color = Color.red; }
       
        if (currentHelth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
