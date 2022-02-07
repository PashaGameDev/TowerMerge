using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerName = null;
    [SerializeField] private Transform squadPanel = null;
    [SerializeField] private List<Image> iconsColection = new List<Image>();
    [SerializeField] public List<Image> squadSpots = new List<Image>();
    [SerializeField] private GameObject[] squad;
    [SerializeField] private Text squadLable = null;
    [SerializeField] public UnitDataBase unitData = null;

    private void Start()
    {
        fillTheSquad();  
    }
    public void addToSquad(int id, CharacterBtnClick btn)
    {

        if (unitData == null) { return; }
        if (!unitData.isSquadHasSpot()) { return; }
       
        unitData.addToSquad(id);
        btn.SwitchMarker(true);

        int idSpot = 0;
        foreach (var spot in squadSpots)
        {
            idSpot++;
            SetUnitImage spotTemp = spot.gameObject.GetComponent<SetUnitImage>();
            if (spotTemp.isAvailable)
            {
                spotTemp.setImage(unitData.Icons[id - 1],id);
                spotTemp.isAvailable = false;
                btn.SpotID = idSpot;
                break;
            }
            
        }
        updateSquadLable();
    }

    void fillTheSquad()
    {
        string name = PlayerPrefs.GetString("UserName");
        if (name == "")
        {
            playerName.text = "YOUR NAME";
            PlayerPrefs.SetString("UserName", "YOUR NAME");
        }
        else
        {
            playerName.text = PlayerPrefs.GetString("UserName");
        }

        int tillFor = unitData.GetSquadSize();
        for (int i = 0; i < tillFor; i++)
        {
            if (unitData.squad.Count > i)
            {
                squad[i] = unitData.squad[i];
                Sprite img = unitData.Icons[squad[i].GetComponent<MyUnit>().unitType -1];
                for (int j = 0; j < squadSpots.Count; j++)
                {
                    if (squadSpots[i].GetComponent<SetUnitImage>().isAvailable)
                    squadSpots[i].GetComponent<SetUnitImage>().setImage(img, squad[i].GetComponent<MyUnit>().unitType);
                   
                    updateSquadLable();
                }
                
            }
           
        }
    }

    
    public void removeFromSquad(int id, CharacterBtnClick btn)
    {
        if (!unitData.checkInSquad(id)) { return; }

        unitData.RemoveUnitFormSquad(id);
        btn.SwitchMarker(false);
        foreach (var item in squadSpots)
        {
            if (btn.unitID == item.GetComponent<SetUnitImage>().ID)
            { item.gameObject.GetComponent<SetUnitImage>().removeImage(); }
        }
        updateSquadLable();
        
    }

    void updateSquadLable()
    {
        if (squadLable == null) { return; }
        squadLable.text = "SQUAD " + unitData.squad.Count + "/" + unitData.GetSquadSize();
    }

    public void ResetName()
    {
        PlayerPrefs.SetString("UserName",playerName.text);
    }
} 
