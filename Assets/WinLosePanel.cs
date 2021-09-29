using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinLosePanel : MonoBehaviour
{
    [SerializeField] private GameObject blackPanel = null;

    [SerializeField] private TextMeshProUGUI totalDemageUnit = null;
    [SerializeField] private TextMeshProUGUI totalDemageEnemy = null;

    [SerializeField] private List<TextMeshProUGUI> unitStatistics = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> enemyStatistics = new List<TextMeshProUGUI>();

    [SerializeField] private List<Image> unitStatisticsBars = new List<Image>();
    [SerializeField] private List<Image> enemyStatisticsBars = new List<Image>();

    private void Start()
    {
        if (blackPanel != null) { blackPanel.SetActive(true); }

        totalDemageUnit.text = GameManager.instance.totalUnitDemageMade.ToString();
        totalDemageEnemy.text = GameManager.instance.totalEnemyDemageMade.ToString();

        for (int i = 0; i < unitStatistics.Count; i++)
        {
            float totalDemegeUnitInt = GameManager.instance.GetDemageGaveUnis(i + 1);
            float totalDemage = GameManager.instance.totalUnitDemageMade;
            unitStatistics[i].text = totalDemegeUnitInt.ToString();

            if (totalDemage <= 0) { return; }
            float amount = ((100 / totalDemage) * totalDemegeUnitInt)/ 100;
            StartCoroutine(fillAmountAnimation(unitStatisticsBars[i], 0.01f, amount,0.1f));
        }

        for (int i = 0; i < enemyStatistics.Count; i++)
        {

            float totalDemegeEnemyInt = GameManager.instance.GetDemageGaveEnemy(i + 1);
            float totalDemage = GameManager.instance.totalEnemyDemageMade;

            if (totalDemage <= 0) { return; }

            enemyStatistics[i].text = totalDemegeEnemyInt.ToString();
            //enemyStatisticsBars[i].fillAmount
              float amount  = ((100 / totalDemage) * totalDemegeEnemyInt) / 100;
            StartCoroutine(fillAmountAnimation(enemyStatisticsBars[i], 0.01f, amount,0.1f)) ;
        }
    }

    IEnumerator fillAmountAnimation(Image progressBar, float step, float amount, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        progressBar.fillAmount += step;

        if (progressBar.fillAmount < amount)
        {
            StartCoroutine(fillAmountAnimation(progressBar, step, amount, 0.001f));
        }
        else
        {
            progressBar.fillAmount = amount;
        }

    }


}
