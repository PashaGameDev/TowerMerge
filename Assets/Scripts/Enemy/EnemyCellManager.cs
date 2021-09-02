using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCellManager : MonoBehaviour
{
    private GameObject unitOnPlace = null;

    public GameObject GetUnitOnPlace()
    {
        return unitOnPlace;
    }

    public void SetuUnitOnPlace(GameObject obj)
    {
        unitOnPlace = obj;
    }
}
