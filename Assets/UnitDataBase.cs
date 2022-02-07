using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UnitDataBase", menuName = "ScriptableObjects/UnitDataBase", order = 1)]

public class UnitDataBase : ScriptableObject
{
    [SerializeField] private string userName = "Your Name";
    [SerializeField] private int squadMaxSize = 3;
    public List<GameObject> unitsType1 = new List<GameObject>();
    public List<GameObject> unitsType2 = new List<GameObject>();
    public List<GameObject> unitsType3 = new List<GameObject>();
    public List<GameObject> unitsType4 = new List<GameObject>();

    public List<Sprite> Icons = new List<Sprite>();
    public List<GameObject> squad = new List<GameObject>();

    public string GetUserName()
    {
        return userName; 
    }

    public void SetUserName(string newName)
    {
        userName = newName; 
    }

    public int GetSquadSize()
    {
        return squadMaxSize;
    }

    public List<GameObject> GetArray(int id)
    {
        switch (id)
        {
            case 1:
                return unitsType1;
            case 2:
                return unitsType2;
            case 3:
                return unitsType3;
            case 4:
                return unitsType4;
                break;

            default:
                break;
        }
        return null;
    }

    public void addToSquad(int ID)
    {
        GameObject unitToAdd = null;
        switch (ID)
        {
            case 1:
                unitToAdd = unitsType1[0];
                break;
            case 2:
                unitToAdd = unitsType2[0];
                break;
            case 3:
                unitToAdd = unitsType3[0];
                break;
            case 4:
                unitToAdd = unitsType4[0];
               break;

            default:
                break;
        }

        if (unitToAdd == null) { return; }
        if (!CanBeAdded(unitToAdd)) { return; }
        if (squad.Count >= squadMaxSize) { return; }

        squad.Add(unitToAdd);
    }

    public bool CanBeAdded(GameObject unitToCheck)
    {
        if (squad.Count >= squadMaxSize) { return false; }
        if (squad.Count == 0) { return true; }
        if (unitToCheck.GetComponent<Unit>() == null) { return false; }

        int id = unitToCheck.GetComponent<Unit>().unitType;
        foreach (var unit in squad)
        {
            if (unit.GetComponent<Unit>().unitType == id) { return false; break; }
        }
        return true;
    }

    public bool checkInSquad(int ID)
    {
        if (squad.Count == 0) { return false; }


        foreach (var unit in squad)
        {
            if (unit.GetComponent<Unit>().unitType == ID) { return true; break; }
        }
        return false; 
    }

    public void RemoveUnitFormSquad(int ID)
    {
       // if (squad.Count == 0) { return; }

        foreach (var unit in squad)
        {
            if (unit.GetComponent<Unit>() != null && unit.GetComponent<Unit>().unitType == ID)
            {
                squad.Remove(unit);
                break;
            }
            
        }
    }
    public bool isSquadHasSpot()
    {
        if (squad.Count < squadMaxSize) { return true; }
        return false;
    }

}
