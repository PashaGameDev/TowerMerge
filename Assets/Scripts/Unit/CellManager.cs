using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    private GameObject unitOnPlace = null;
    [SerializeField] private GameObject child = null;

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
}
