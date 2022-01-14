using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform squadPanel = null;
    [SerializeField] private List<Image> iconsColection = new List<Image>();
    [SerializeField] private List<Image> squadSpots = new List<Image>();
    [SerializeField] private GameObject[] squad; 
    

    public void addToSquad(int id)
    {
       // if (squad.Length >= 3) { return; }
        GameObject unit = Instantiate(iconsColection[id].gameObject, squadSpots[id].gameObject.transform.position, Quaternion.identity,squadPanel);
        squad[id] = unit;
        PlayerPrefs.SetInt("UnitType"+id, 1);
    }

    public void removeFromSquad(int id)
    {
        if (squad.Length <= 0) { return; }
        if (squad[id] == null) { return; }
        Destroy(squad[id]);
       // squad.Remove(squad[id]);
        PlayerPrefs.SetInt("UnitType"+id, 0);
    }
} 
