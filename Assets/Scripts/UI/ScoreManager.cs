using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerExpText = null;
    [SerializeField] TextMeshProUGUI PlayerLvlText = null;
    [SerializeField] Image expProgressImg = null;

    public int playerExperiance = 1200;
    public int playerLvl = 0;

    private int palyerExpCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerExperiance = PlayerPrefs.GetInt("PlayerExp", 0);
        while (playerExperiance >= 1000)
        {
            playerExperiance -= 1000;
            playerLvl++;
        }

        if (playerLvl <= 0) { playerLvl = 1; }
        if (PlayerLvlText == null || PlayerExpText == null || expProgressImg == null) { return; }
       
        
        PlayerLvlText.text = playerLvl.ToString();
        expProgressImg.fillAmount = 0f; //(
        float amount =   (float)playerExperiance / 1000f;
        StartCoroutine(filledImage(2f, amount));
    }

    IEnumerator filledImage(float waitTime, float amount)
    {
        yield return new WaitForSeconds(waitTime);
        expProgressImg.fillAmount += 0.01f;
        if (expProgressImg.fillAmount < amount)
        {
            StartCoroutine(filledImage(0.002f,amount));
            PlayerExpText.text = "Exp: "+palyerExpCounter.ToString() + "/1000";
            palyerExpCounter++;
        }
        else
        {
            PlayerExpText.text = "Exp: "+playerExperiance.ToString() + "/1000";
            expProgressImg.fillAmount = amount;
        }
    }

}
