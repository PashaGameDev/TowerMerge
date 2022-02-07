using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuidUnitBtn : MonoBehaviour
{
    [SerializeField] int spotID;
    [SerializeField] int unitType;
    [SerializeField] Image unitIcon;
    [SerializeField] private float buildDelay = 3f;
    [SerializeField] AudioClip clickSFX = null;
    [SerializeField] AudioClip unitCreationSFX = null;
    [SerializeField] AudioSource audio = null;

    private GameObject unitPrefab;
    private int price;
    private float timer = 3f;
    private bool isBusy = false;

    CellManager cell;
    GameObject unitRef = null;


    public void TryBuild()
    {
        
        unitPrefab = GameManager.instance.getUnitToCreat(spotID, 0);
        if (unitPrefab == null) {  return; }

        price = unitPrefab.GetComponent<MyUnit>().price;

        if (GameManager.instance.isPurchaseble(price) && !isBusy)
        {
            if (audio != null && clickSFX != null) { audio.clip = clickSFX; audio.Play(); }
            cell = GameManager.instance.GetCell();
            if (cell == null) { return; }
            isBusy = true;
           
            GameManager.instance.decreaseBalace(price);
            buildUnit();
            StartCoroutine(buildUnitCountDown(0.01f));
        }
    }

    IEnumerator buildUnitCountDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        timer -= 0.1f;
        if (timer <= 0)
        {
            //buildUnit();
            makeUnitVisible();
            timer = buildDelay;
          //  unitIcon.fillAmount = 0;
        }
        else
        {
            unitIcon.fillAmount = 1 - timer / buildDelay;
            StartCoroutine(buildUnitCountDown(0.01f));
        }
    }

    void buildUnit()
    {
            Vector3 pos = cell.gameObject.transform.position;
            pos.y = pos.y + 1;
            unitRef = Instantiate(unitPrefab, pos, Quaternion.identity);
            unitRef.GetComponent<MyUnit>().SetCell(cell);
            unitRef.SetActive(false);
    }

    void makeUnitVisible()
    {
        unitRef.SetActive(true);
        unitRef.GetComponent<MyUnit>().GetCell().UnitSpawnEffectShow();
        unitRef = null;
        isBusy = false;

    }
}
