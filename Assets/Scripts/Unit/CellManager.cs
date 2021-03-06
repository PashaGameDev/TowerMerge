using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    private GameObject unitOnPlace = null;
    [SerializeField] private GameObject child = null;
    [SerializeField] private GameObject mergeEffectSpaner = null;
    [SerializeField] private GameObject unitSpawnEffect = null;

    public GameObject GetUnitOnPlace()
    {
        return unitOnPlace;
    }

    public void SetuUnitOnPlace(GameObject obj, Color color)
    {
        unitOnPlace = obj;
        if (child == null) { return; }
        child.GetComponent<Renderer>().material.color = color;
    }

    public void SpawnMegreEffect()
    {
        if (mergeEffectSpaner == null) { return; }
        mergeEffectSpaner.SetActive(true);
        StartCoroutine(hideSpawner(mergeEffectSpaner));
    }

    public void UnitSpawnEffectShow()
    {
        if (unitSpawnEffect == null) { return; }
        unitSpawnEffect.SetActive(true);
        hideSpawner(unitSpawnEffect);
    }
    IEnumerator hideSpawner(GameObject obj)
    {
        yield return new WaitForSeconds(1f);

        obj.SetActive(false);
    }
}
